using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MemoryReallocation.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IList<int> testdata = new List<int> { 0, 2, 7, 0 };

            Debug.Assert(PartOne(Clone(testdata)) == 5);
            IList<int> today = File.ReadAllLines("data.txt").Select(l => System.Convert.ToInt32(l)).ToList();
            Console.WriteLine($"Part One: {PartOne(Clone(today))}");

            Debug.Assert(PartTwo(Clone(testdata)) == 4);
            Console.WriteLine($"Part Two: {PartTwo(Clone(today))}");

            Console.ReadKey();
        }

        private static int PartOne(IList<int> list, bool partone = true)
        {
            IList<int> hashes = new List<int> { list.GetSequenceHashCode() };
            while (true)
            {
                int m = list.Max();
                int i = list.IndexOf(m);
                list[i] = 0;
                for (int add = 0; add < m; ++add)
                {
                    ++list[(i + add + 1) % list.Count];
                }

                int hash = list.GetSequenceHashCode();
                if (hashes.Contains(hash))
                {
                    if (partone)
                        return hashes.Count;
                    else
                        return hashes.Count - hashes.IndexOf(hash);
                }
                else
                    hashes.Add(hash);
            }
        }

        static int PartTwo(IList<int> items)
        {
            return PartOne(items, false);
        }

        static IList<int> Clone(IList<int> items)
        {
            return items.Select(i => i).ToList();
        }
    }
    public static class Tools {
        // citation: https://stackoverflow.com/a/30758270 
        public static int GetSequenceHashCode<T>(this IList<T> sequence)
        {
            const int seed = 487;
            const int modifier = 31;

            unchecked
            {
                return sequence.Aggregate(seed, (current, item) =>
                    (current * modifier) + item.GetHashCode());
            }
        }
    }
}
