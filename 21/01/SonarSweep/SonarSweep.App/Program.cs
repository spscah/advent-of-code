using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SonarSweep.App
{
    class Program
    {
        static void Main(string[] args)
        {
            IList<int> numbers = File.ReadAllLines("data.txt").Select(i => Convert.ToInt32(i)).ToList();
            int count = 0;
            int previous = numbers[0] + 1;
            foreach (int n in numbers) {
                if (n > previous)
                    count += 1;
                previous = n;
            }
            Console.WriteLine(count);

            count = 0;
            previous = numbers.Take(3).Sum() + 1;
            int a = 0;
            int b = numbers[0];
            int c = numbers[1];
            int current = a + b;
            foreach (int n in numbers.Skip(2)) {
                current -= a;
                a = b;
                b = c;
                c = n;
                current += c;
                if (current > previous)
                    count += 1;
                previous = current;
            }
            Console.WriteLine(count);
        }
    }
}
