using AdventOfCode.Lib;
using System.Diagnostics;

namespace ChangeMe.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 17;
            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test) == (102, 0));

            IList<string> real = TODAY.AsListOfStrings(false);
            (int partone, int parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (int partone, int parttwo) Result(IList<string> real)
        {
            IDictionary<(int r, int c), (HashSet<string> paths, int distance, bool finalised)> dijkstra = new Dictionary<(int r, int c), (HashSet<string> path, int distance, bool finalised)>();
            dijkstra[(0, 0)] = (new() { "" }, real.First().First() - '0', false);
            int partone = int.MaxValue;
            while (dijkstra.Any(kvp => !kvp.Value.finalised))
            {
                (int r, int c) = dijkstra.Where(kvp => !kvp.Value.finalised).OrderBy(kvp => kvp.Value.distance).First().Key;
                (HashSet<string> paths, int distance, bool finalised) = dijkstra[(r, c)];
                dijkstra[(r, c)] = (paths, distance, true);
                if (r == real.Count - 1 && c == real[0].Length - 1)
                {
                    partone = distance;
                    break;
                }
                foreach (char direction in "UDLR")
                {
                    foreach (string path in paths)
                    {
                        if (path.EndsWith($"{direction}{direction}{direction}")) continue;
                        (int r, int c) next = direction switch
                        {
                            'U' => (r - 1, c),
                            'D' => (r + 1, c),
                            'L' => (r, c - 1),
                            'R' => (r, c + 1),
                            _ => throw new Exception($"Unknown direction {direction}")
                        };
                        if (next.r < 0 || next.r >= real.Count || next.c < 0 || next.c >= real[0].Length)
                        {
                            continue;
                        }
                        if (dijkstra.ContainsKey(next))
                        {
                            int current = dijkstra[(r, c)].distance + real[next.r][next.c] - '0';

                            if (dijkstra[next].distance < current)
                                continue;
                            else if (dijkstra[next].distance == current)
                                dijkstra[next].paths.Add(path + direction);
                        }
                        dijkstra[next] = (new() { path + direction }, dijkstra[(r, c)].distance + real[next.r][next.c] - '0', false);
                    }
                }
            }
            return (partone, 0);
        }


    }
}