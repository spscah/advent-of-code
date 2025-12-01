using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace LavaductLagoon.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 18;
            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test) == (62, 952408144115));

            IList<string> real = TODAY.AsListOfStrings(false);
            (ulong partone, ulong parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (ulong partone, ulong parttwo) Result(IList<string> real)
        {
            HashSet<(long r, long c)> holes = new();
            (long r, long c) location = (0, 0);
            holes.Add(location);
            string pattern = @"(U|D|L|R) (\d+)"; // \(\#(\d{6,6})\)";
            Regex regex = new Regex(pattern);
            foreach (string line in real)
            {
                Match match = regex.Match(line);

                char direction = match.Groups[1].Value[0];
                for (long i = 0; i < long.Parse(match.Groups[2].Value); ++i)
                {
                    (long r, long c) = location;
                    location = direction switch
                    {
                        'U' => (r - 1, c),
                        'D' => (r + 1, c),
                        'L' => (r, c - 1),
                        'R' => (r, c + 1),
                        _ => throw new Exception($"Unknown direction {direction}")
                    };
                    holes.Add(location);
                }
            }
            ulong partone = PartOne(holes);

            holes = new();
            location = (0, 0);
            holes.Add(location);
            pattern = @"\(\#(.*)\)";
            regex = new Regex(pattern);
            foreach (string line in real)
            {
                Match match = regex.Match(line);

                char direction = match.Groups[1].Value[5];
                string hex = match.Groups[1].Value[..5];
                long extent = long.Parse(hex, System.Globalization.NumberStyles.HexNumber);

                for (long i = 0; i < extent; ++i)
                {
                    (long r, long c) = location;
                    location = direction switch
                    {
                        '3' => (r - 1, c),
                        '1' => (r + 1, c),
                        '2' => (r, c - 1),
                        '0' => (r, c + 1),
                        _ => throw new Exception($"Unknown direction {direction}")
                    };
                    holes.Add(location);
                }
            }
            ulong parttwo = PartOne(holes);

            return (partone, parttwo);
        }

        private static ulong PartOne(HashSet<(long r, long c)> holes)
        {
            (long, long) rrange = (holes.Min(h => h.r), holes.Max(h => h.r));
            (long, long) crange = (holes.Min(h => h.c), holes.Max(h => h.c));
            HashSet<(long, long)> edges = new();
            // vertical edges 
            for (long r = rrange.Item1; r <= rrange.Item2; ++r)
            {
                var e1 = (r, crange.Item1);
                if (!holes.Contains(e1))
                    edges.Add(e1);
                var e2 = (r, crange.Item2);
                if (!holes.Contains(e2))
                    edges.Add(e2);
            }
            // horizontal edges 
            for (long c = crange.Item1; c <= crange.Item2; ++c)
            {
                var e1 = (rrange.Item1, c);
                if (!holes.Contains(e1))
                    edges.Add(e1);
                var e2 = (rrange.Item2, c);
                if (!holes.Contains(e2))
                    edges.Add(e2);
            }
            Queue<(long, long)> queue = new(edges);
            while (queue.Count > 0)
            {
                (long r, long c) = queue.Dequeue();
                if (holes.Contains((r, c)))
                    continue;
                foreach ((long r, long c) neighbour in new (long, long)[] { (r - 1, c), (r + 1, c), (r, c - 1), (r, c + 1) })
                {
                    if (neighbour.r < rrange.Item1 || neighbour.r > rrange.Item2 || neighbour.c < crange.Item1 || neighbour.c > crange.Item2)
                        continue;
                    if (holes.Contains(neighbour) || edges.Contains(neighbour))
                        continue;
                    edges.Add(neighbour);
                    queue.Enqueue(neighbour);
                }
            }

            ulong partone = (ulong)(1 + rrange.Item2 - rrange.Item1) * (ulong)(1 + crange.Item2 - crange.Item1) - (ulong)edges.Count;
            return partone;
        }
    }
}