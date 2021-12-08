using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;

namespace GiantSquid.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 8;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST);

            IList<int> counters = Enumerable.Range(1, 8).Select(i => 0).ToList();

            foreach (string s in data)
            {
                if (!s.Contains("|")) break;
                string half = s.Split(" | ").ToList()[1];
                foreach (int i in half.TrimEnd().Split(' ').Select(n => n.Length))
                    ++counters[i];
            }


            Console.WriteLine($"{counters[2] + counters[4] + counters[3] + counters[7]}");
//            if (TEST)
//                data = new List<string> { "acedgfb cdfbe gcdfa fbcad dab cefabd cdfgeb eafb cagedb ab | cdfeb fcadb cdfeb cdbaf" };


            IList<string> allCombinations = Combinations(new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g' }).Select(v => string.Concat(v)).ToList();

            Console.WriteLine(data.Select(d => Match(d, allCombinations)).Sum());
        }

        static int Match(string s, IList<string> combinations)
        {
            foreach (string combination in combinations)
            {
                IList<string> patterns = new List<string>
                {
                    string.Concat(new List<char> { combination[0], combination[1], combination[2], combination[4], combination[5], combination[6]}),
                    string.Concat(new List<char> { combination[2], combination[5] }),
                    string.Concat(new List<char> { combination[0], combination[2], combination[3], combination[4], combination[6] }),
                    string.Concat(new List<char> { combination[0], combination[2], combination[3], combination[5], combination[6] }),
                    string.Concat(new List<char> { combination[1], combination[2], combination[3], combination[5] }),
                    string.Concat(new List<char> { combination[0], combination[1], combination[3], combination[5], combination[6] }),
                    string.Concat(new List<char> { combination[0], combination[1], combination[3], combination[4], combination[5], combination[6] }),
                    string.Concat(new List<char> { combination[0], combination[2], combination[5] }),
                    string.Concat(new List<char> { combination[0], combination[1], combination[2], combination[3], combination[4], combination[5], combination[6] }),
                    string.Concat(new List<char> { combination[0], combination[1], combination[2], combination[3], combination[5], combination[6] }),
                };
                for (int p = 0; p < patterns.Count; ++p)
                {
                    patterns[p] = string.Concat(patterns[p].OrderBy(k => k));
                }
                if (s.Contains("|"))
                {
                    IList<string> halves = s.Split(" | ").ToList();
                    IList<string> keys = halves[0].Trim().Split(' ').Select(k => String.Concat(k.OrderBy(c => c))).ToList();

                    if (keys.All(k => patterns.Contains(k)))
                    {
                        IList<string> values = halves[1].Trim().Split(' ').Select(k => String.Concat(k.OrderBy(c => c))).ToList();
                        int total = 0;
                        foreach (string v in values)
                        {
                            total *= 10;
                            total += patterns.IndexOf(v);
                        }
                        return total;
                    }
                }
            }
            throw new Exception($"cannot match: {s}");          

        }
        static IEnumerable<IList<T>> Combinations<T>(IList<T> input)
        {
            if (input.Count == 1)
                yield return input;
            else
            {
                IList<T> rest = input.Skip(1).ToList(); ;
                T head = input[0];
                foreach (IList<T> comb in Combinations(rest))
                {
                    IList<T> cp;
                    for (int loc = 0; loc < rest.Count; ++loc)
                    {
                        cp = comb.Select(c => c).ToList();
                        cp.Insert(loc, head);
                        yield return cp;
                    }
                    cp = comb.Select(c => c).ToList();
                    cp.Add(head);
                    yield return cp;
                }
            }


        }
    }
}