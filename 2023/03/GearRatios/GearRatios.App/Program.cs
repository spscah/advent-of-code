using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace GearRatios.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 0;
            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test) == (4361, 467835));

            IList<string> real = TODAY.AsListOfStrings(false);
            (int partone, int parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (int partone, int parttwo) Result(IList<string> real)
        {
            var symbols = GetSymbols(real);
            var parts = new List<(int i, int r, int c)>();
            var adjacent = new List<(int i,char c)>();
            int row = 0;
            foreach(string line in real) {
                string thisline = line;
                foreach((char s, int r, int c) in symbols) {
                    if(thisline.Contains(s)) {
                        thisline = thisline.Replace(s.ToString(), ".");                        
                    }
                }
                while(thisline.Contains("..")) 
                    thisline = thisline.Replace("..", ".");
                IList<int> numbers = thisline.Split('.').Where(x => x.Length > 0).Select(x => int.Parse(x)).ToList();
                foreach(int number in numbers) {                    
                    int col = line.IndexOf(number.ToString());
                    parts.Add((number, row, col));
                }
/*
                foreach(int number in numbers) {                    
                    int col = line.IndexOf(number.ToString());
                    var hits = symbols
                        .Where(s => Math.Abs(row-s.r) <=1)
                        .Where(s => s.c >= col-1 && s.c < col+1+(number.ToString().Length))
                        .Select(s => (number, s.s)).ToList();
                        
                    if(hits.Count >0)
                        adjacent = adjacent.Concat(hits).ToList();                                       
                    }
                        */
                
                ++row;
            }

            foreach(var symbol in symbols) {
                var hits = parts
                    .Where(p => Math.Abs(p.r-symbol.r) <=1)
                    .Where(p => symbol.c >= p.c-1 && symbol.c < p.c+1+(p.i.ToString().Length))
                    .Select(p => (p.i, symbol.s)).ToList();
                if(hits.Count >0)
                    adjacent = adjacent.Concat(hits).ToList();
            }
            

            int sch = adjacent
            
                .Select(a => a.i)
                // .Distinct()
                .Sum();
             
            // 525119
            File.WriteAllText("output.txt", string.Join("\n", adjacent.Select(a => $"{a.i} {a.c}")));            
            return (sch, 0);
            /*
            chris@playroom:~/github/sps/advent-of-code/2023/03/GearRatios/GearRatios.App$ python3 reddit.py 
            (525119, 76504829)
            */
        }
        static IList<(char s, int r, int c)> GetSymbols(IList<string> real)
        {
            var symbols = new List<(char s, int r, int c)>();
            for (int r = 0; r < real.Count; r++)
            {
                for (int c = 0; c < real[r].Length; c++)
                {
                    if ((real[r][c] < '0' || real[r][c] > '9') && real[r][c] != '.')
                    {
                        symbols.Add((real[r][c], r, c));
                    }
                }
            }
            return symbols;
        }
    }
}