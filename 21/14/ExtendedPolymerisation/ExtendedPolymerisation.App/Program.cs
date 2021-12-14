using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;

namespace ExtendedPolymerisation.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 14;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST);

            IList<char> polymer = data[0].ToList();

            IDictionary<(char, char), char> lookup = new Dictionary<(char, char), char>();
            foreach(string m in data.Skip(2))
            {
                lookup[(m[0], m[1])] = m[6];
            }
            bool partone = false; 
           

            IDictionary<(char, char), ulong> tracker = new Dictionary<(char, char), ulong>();
            polymer = data[0].ToList();
            for(int i = 1; i < polymer.Count; ++i)
            {
                (char,char) k = (polymer[i - 1], polymer[i]);
                if (!tracker.ContainsKey(k))
                    tracker[k] = 0;
                ++tracker[k];
            }

            for (int steps = 0; steps < (partone ? 10 : 40); ++steps)
            {
                IDictionary<(char, char), ulong> newgeneration = new Dictionary<(char, char), ulong>();
                foreach(KeyValuePair<(char,char),ulong> kvp in tracker)
                {
                    (char, char) a = (kvp.Key.Item1, lookup[kvp.Key]);
                    (char, char) b = (lookup[kvp.Key], kvp.Key.Item2);
                    if (!newgeneration.ContainsKey(a))
                        newgeneration[a] = 0;
                    if (!newgeneration.ContainsKey(b))
                        newgeneration[b] = 0;
                    newgeneration[a] += kvp.Value;
                    newgeneration[b] += kvp.Value;
                }
                tracker = newgeneration;
                
            }

            IDictionary<char, ulong> counters = lookup.Values.Distinct().Select(l => l).ToDictionary(l => l, l => (ulong)0);
            foreach (KeyValuePair<(char, char), ulong> kvp in tracker)
            {
                counters[kvp.Key.Item1] += kvp.Value;
                counters[kvp.Key.Item2] += kvp.Value;
            }
            ++counters[polymer.First()];
            ++counters[polymer.Last()];

            Console.WriteLine((counters.Max(k => k.Value) - counters.Min(k => k.Value)) / 2);

        }
    }
}