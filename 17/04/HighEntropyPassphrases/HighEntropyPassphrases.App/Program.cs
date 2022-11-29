using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace HighEntropyPassPhrases.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 4;
            IList<string> testdata = TODAY.AsListOfStrings(true);

            Debug.Assert(PartOne(testdata) == 2);

            Console.WriteLine($"PartOne: {PartOne(TODAY.AsListOfStrings(false))}");
            Console.WriteLine($"PartTwo: {PartTwo(TODAY.AsListOfStrings(false))}");
        }

        private static int PartOne(IList<string> data)
        {
            int count = 0;
            foreach (string passphrase in data)
            {
                IList<string> split = passphrase.Split(' ').ToList();
                if (split.Count == split.Distinct().Count())
                    ++count;
            }
            return count;
        }

        static int PartTwo(IList<string> data)
        {
            IList<string> sorted = new List<string>();
            foreach (string u in data)
            {
                string s = string.Join(" ", u.Split(' ').Select(s => s.ToList<char>()).Select(x => new string(x.OrderBy(c => c).ToArray())));
                sorted.Add(s);
            }
            return PartOne(sorted);

        }

    }
}