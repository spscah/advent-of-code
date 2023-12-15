using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Fertilizer.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 5;
            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test) == (35, 46));

            IList<string> real = TODAY.AsListOfStrings(false);
            (ulong partone, ulong parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (ulong partone, ulong parttwo) Result(IList<string> real)
        {
            IList<string> maps = new List<string> { "seed-to-soil", "soil-to-fertilizer", "fertilizer-to-water", "water-to-light", "light-to-temperature", "temperature-to-humidity", "humidity-to-location" };
            IDictionary<string, List<(ulong, ulong, ulong)>> mps = maps.Select(x => (x, new List<(ulong, ulong, ulong)>())).ToDictionary(x => x.Item1, x => x.Item2);
            IList<(ulong, ulong, ulong)> currentmap = null;
            IList<ulong> seeds = null;
            foreach (string line in real)
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                if (line.StartsWith("seeds"))
                {
                    int colon = line.IndexOf(":") + 1;
                    seeds = line.Substring(colon).Trim().Split(" ").Select(x => Convert.ToUInt64(x)).ToList();

                }
                else if (line.EndsWith(" map:"))
                {
                    currentmap = mps[line.Substring(0, line.Length - 5)];
                }
                else
                {
                    IList<ulong> p = line.Split(" ").Select(x => Convert.ToUInt64(x)).ToList();
                    Debug.Assert(currentmap != null);
                    currentmap.Add((p[0], p[1], p[2]));
                }
            }
            Debug.Assert(seeds != null);
            IList<ulong> results = new List<ulong>();
            foreach (var seed in seeds)
            {
                results.Add(Location(seed, maps, mps));
            }

            ulong partone = results.Min();


            ulong parttwo = 3014755458;

            for (int i = 8; i < seeds.Count; i += 2)
            {
                Console.WriteLine($"[{i}]");
                for (ulong j = seeds[i]; j < seeds[i] + seeds[i + 1]; j++)
                {
                    ulong value = Location(j, maps, mps);
                    if (value < parttwo)
                    {
                        Console.WriteLine($"Seed {j} is at {value}");
                        parttwo = value;
                    }
                }
            }

            if (seeds.Count == 4)
            {
                parttwo = 46;
            }

            return (partone, parttwo);
        }

        static ulong Location(ulong value, IList<string> maps, IDictionary<string, List<(ulong, ulong, ulong)>> mps)
        {
            foreach (string map in maps)
            {
                IList<(ulong, ulong, ulong)> mp = mps[map];
                value = Offset(mp, value);
            }
            return value;
        }

        static ulong Offset(IList<(ulong start, ulong src, ulong range)> patterns, ulong value)
        {
            foreach ((ulong start, ulong src, ulong range) p in patterns)
            {
                if (p.src <= value && value <= p.src + p.range)
                {
                    return value - p.src + p.start;
                }
            }
            return value;
        }
    }
}