using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ProboscideaVolcanium.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 0;
            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test) == (1651, 1707));

            IList<string> real = TODAY.AsListOfStrings(false);
            (int partone, int parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (int partone, int parttwo) Result(IList<string> real)
        {
            int limit = 30;
            Dictionary<string, int> pressures = new();
            Dictionary<string, IList<string>> neighbours = new();
            List<string> identifiers = new();
            foreach (string line in real)
            {
                string[] parts = line.Split(" ");
                string id = parts[1];
                int pressure = int.Parse(parts[4].Substring(5).TrimEnd(';'));
                pressures.Add(id, pressure);
                neighbours.Add(id, new List<string>());
                if (pressure > 0)
                    identifiers.Add(id);
                for (int i = 9; i < parts.Length; i++)
                {
                    string neighbour = parts[i].TrimEnd(',');
                    neighbours[id].Add(neighbour);
                }
            }

            Dictionary<(string, string), int> shortestPaths = new();

            foreach (string id1 in identifiers)
                foreach (string id2 in identifiers)
                    if (id1 != id2)
                        shortestPaths.Add((id1, id2), ShortestPath(neighbours, id1, id2));

            foreach (string id in identifiers)
                shortestPaths.Add(("aa", id), ShortestPath(neighbours, "AA", id));

            int partone = PartOne(limit, pressures, identifiers, shortestPaths);

            limit = 26;
            int parttwo = 0;

            // loop from 0 to 2^n-1 to generate a bitmask for who has which identifier (no need to ignore the AA identifier, it's not zero)
            for (ulong mask = 0; mask < (1ul << identifiers.Count); mask++)
            {
                List<string> mine = new();
                List<string> his = new();
                for (int i = 0; i < identifiers.Count; i++)
                {
                    if ((mask & (1ul << i)) != 0)
                        mine.Add(identifiers[i]);
                    else
                        his.Add(identifiers[i]);
                }

                // then it's just a case of finding the part one solution based on those subsets 
                int flow1 = PartOne(limit, pressures, mine, shortestPaths);
                int flow2 = PartOne(limit, pressures, his, shortestPaths);

                if (flow1 + flow2 > parttwo)
                    parttwo = flow1 + flow2;
            }


            return (partone, parttwo);

        }

        static int PartOne(int limit, Dictionary<string, int> pressures, List<string> identifiers, Dictionary<(string, string), int> shortestPaths)
        {
            Queue<List<(string, int)>> queue = new();
            queue.Enqueue(new List<(string, int)> { ("aa", 0) });
            int partone = 0;

            while (queue.Count > 0)
            {
                var path = queue.Dequeue();
                foreach (string id in identifiers.Where(i => !path.Select(p => p.Item1).Contains(i)))
                {
                    var newPath = path.Select(p => p).ToList();
                    newPath.Add((id, shortestPaths[(path.Last().Item1, id)]));
                    if (newPath.Last().Item2 <= limit)
                    {
                        (int flow, int m) = Measure(newPath, pressures, limit);
                        if (flow > partone && m <= limit)
                            partone = flow;
                        if (m <= limit)
                            queue.Enqueue(newPath);
                    }
                }
            }

            return partone;
        }

        static int ShortestPath(Dictionary<string, IList<string>> neighbours, string i, string j)
        {
            (string, int) status = (i, 0);
            Queue<(string, int)> queue = new();
            queue.Enqueue(status);

            while (queue.Count > 0)
            {
                status = queue.Dequeue();
                if (status.Item1 == j)
                    return status.Item2;
                foreach (string neighbour in neighbours[status.Item1])
                {
                    queue.Enqueue((neighbour, status.Item2 + 1));
                }
            }

            return -1;
        }

        static (int flow, int minutes) Measure(List<(string, int)> path, Dictionary<string, int> pressures, int limit)
        {
            int flow = 0;
            int minutes = 0;
            foreach ((string id, int step) in path.Skip(1))
            {
                minutes += step + 1;
                flow += pressures[id] * (limit - minutes);
            }

            return (flow, minutes);
        }
    }
}