using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;

namespace GiantSquid.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 6;
            const bool TEST = false;            
            bool partone = false;

            IList<int> data = TODAY.CsvToIntegers(TEST);
            data = TODAY.CsvToIntegers(TEST);
            IDictionary<int, long> counters = data.Distinct().ToDictionary(x => x, x => (long)(data.Count(y => x == y)));
            for(int i = 0; i < (partone ? 80 : 256); ++i) {
                IDictionary<int, long> newcounters = counters.ToDictionary(k => k.Key-1, k => k.Value);                
                if(newcounters.ContainsKey(-1)) {
                    newcounters[8] = newcounters[-1];
                    if(!newcounters.ContainsKey(6))
                        newcounters[6] = 0;
                    newcounters[6] += newcounters[-1];
                    newcounters.Remove(-1);
                }
                counters = newcounters;
            }
            Console.WriteLine(counters.Select(k => k.Value).Sum());
            // 1632779838045 - 1.6*10^12 ~ 2^41 - so within a long 
        }
    }
}