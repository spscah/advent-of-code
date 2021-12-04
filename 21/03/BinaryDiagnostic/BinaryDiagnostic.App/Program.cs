using AdventOfCode.Lib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BinaryDiagnostic.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 3;
            IList<string> input = CommonFunctions.AsListOfStrings(TODAY);
            //            input = File.ReadAllLines(@"test.txt");
            //            sz = input[0].Length;
            IList<int> values = input.Select(s => Convert.ToInt32(s, 2)).ToList();
            int sz = input[0].Length;
            IList<int> counters = Enumerable.Range(1, sz).Select(v => 0).ToList();
            foreach(string s in input)
            {
                for (int i = 0; i < sz; ++i)
                    if (s[i] == '1')
                        ++counters[i];
            }

            string majority = string.Join("", counters.Select(i => (i > input.Count / 2) ? '1' : '0'));
            ulong maj = Convert.ToUInt64(majority, 2);
            ulong min = ((ulong)Math.Pow(2, sz)) - 1 - maj;
            Console.WriteLine($"{maj} * {min} = {maj*min}");

            IList<string> mensheviks = input.Select(i => i).ToList();
            IList<string> bolsheviks = input.Select(i => i).ToList();
            for(int i = 0; i < sz; ++i)
            {
                int ones = mensheviks.Count(m => m[i] == '1');
                char target1 = ones >= (mensheviks.Count-ones) ? '1' : '0';

                ones = bolsheviks.Count(b => b[i] == '1');
                char target2 = ones < (bolsheviks.Count - ones) ? '1' : '0';

                if (mensheviks.Count > 1)
                    mensheviks = mensheviks.Where(m => m[i] == target1).ToList();
                if(bolsheviks.Count > 1)
                    bolsheviks = bolsheviks.Where(b => b[i] == target2).ToList();
            }
            int oxygen = Convert.ToInt32(mensheviks[0], 2);
            int co2 = Convert.ToInt32(bolsheviks[0], 2);
            Console.Write($"{oxygen} * {co2} = {oxygen * co2}");
            // 2651740 is too low 
            // 2862864 is too high 
        }
    }
}
