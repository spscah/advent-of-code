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
            Debug.Assert(PartOne(1) == 0);
            Debug.Assert(PartOne(12) == 3);
            Debug.Assert(PartOne(23) == 2);
            Debug.Assert(PartOne(1024) == 31);

            Console.WriteLine($"Part One: {PartOne(289326)}");
        }

        static int PartOne(int x)
        {
            // the shells go up with the odd numbers, 1, 3, 5 and so on 
            if (x == 1)
                return 0;
            // first, which odd square is bigger than x 
            int square = 1;
            while (square * square < x)
                square += 2;

            // start at the square on the previous shell 
            int current = (square - 2) * (square - 2);
            // each side of the shell has (square-1) 
            while (current + square - 1 < x)
                current += square - 1;

            int extent = x - current;
            extent -= (square - 1) / 2;

            extent = Math.Abs(extent);

            return (square - 1) / 2 + extent;
        }
    }
}