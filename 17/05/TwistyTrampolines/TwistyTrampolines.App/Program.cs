using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TwistyTrampolines.App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IList<int> testdata = new List<int> {  0, 3, 0, 1, -3 };

            Debug.Assert(PartOne(Clone(testdata)) == 5);
            IList<int> today = File.ReadAllLines("data.txt").Select(l => System.Convert.ToInt32(l)).ToList();
            Console.WriteLine($"Part One: {PartOne(Clone(today))}");

            Debug.Assert(PartTwo(Clone(testdata)) == 10);
            Console.WriteLine($"Part One: {PartTwo(Clone(today))}");

            Console.ReadKey();
        }

        static IList<int> Clone(IList<int> items)
        {
            return items.Select(i => i).ToList();   
        }

        private static int PartOne(IList<int> data, bool partone = true)
        {
            int index = 0;
            int stepcount = 0;
            while(index < data.Count)
            {
                int jump = data[index];
                if (partone)
                    ++data[index];
                else
                    data[index] += jump >= 3 ? -1 : 1; 

                index += jump;
                ++stepcount;
            }
            return stepcount;
        }

        static int PartTwo(IList<int> testdata)
        {
            return PartOne(testdata, false);
        }
    }
}
