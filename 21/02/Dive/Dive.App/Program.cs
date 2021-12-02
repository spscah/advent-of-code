using AdventOfCode.Lib;
using System;
using System.Collections.Generic;

namespace Dive.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 2;
            IList<(string, int)> data = TODAY.AsStringIntegerPairs();

            int f = 0;
            int d = 0;
            foreach ((string, int) p in data) {
                if (p.Item1 == "down")
                    d += p.Item2;
                if (p.Item1 == "up")
                    d -= p.Item2;
                if (p.Item1 == "forward")
                    f += p.Item2;
            }
            Console.WriteLine($"{d} * {f} = {d * f}");

            f = 0;
            d = 0;
            int aim = 0;

            foreach ((string, int) p in data)
            {
                if (p.Item1 == "down")
                    aim += p.Item2;
                if (p.Item1 == "up")
                    aim -= p.Item2;
                if (p.Item1 == "forward")
                {

                    f += p.Item2;
                    d += (aim * p.Item2);
                }
            }
            Console.WriteLine($"{d} * {f} = {d * f}");
        }
    }
}
