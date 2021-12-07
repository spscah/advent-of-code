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
            const int TODAY = 7;
            const bool TEST = false;
            IList<int> data = TODAY.CsvToIntegers(TEST);
            Console.WriteLine(data.Distinct().Select(d => TotalFuel(d, data, PartOne)).Min());
            Console.WriteLine(data.Distinct().Select(d => TotalFuel(d, data, PartTwo)).Min());
        }

        static int TotalFuel(int alignment, IList<int> data, Func<int,int,int> fn)
        {
            return data.Select(i => fn(alignment,i)).Sum();
        }

        static int PartOne(int alignment, int i)
        {
            return Math.Abs(alignment - i);
        }

        static int PartTwo(int alignment, int i)
        {
            int a = Math.Abs(alignment - i);
            return a * (a + 1) / 2;
        }


    }
}