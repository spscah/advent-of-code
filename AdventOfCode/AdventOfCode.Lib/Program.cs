using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GiantSquid.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 0;
            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test) == (26, 0));

            IList<string> real = TODAY.AsListOfStrings(false);
            (int partone, int parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (int partone, int parttwo) Result(IList<string> real)
        {
            throw new NotImplementedException();
        }
    }
}