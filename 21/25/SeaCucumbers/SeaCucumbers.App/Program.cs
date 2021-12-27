using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;

namespace SeaCucumbers.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 25;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST);

            IList<IList<char>> grid = data.Select(d => (IList<char>)d.ToList()).ToList();

            int step = 0;
            string current = Output(grid); 
            string previous = string.Empty;
            Console.WriteLine($"{step}:\n{current}");
            do {
                previous = current;
                ++step;
                Go(grid, true);
                Go(grid, false);
                current = Output(grid);
            } while (current != previous);
            Console.WriteLine(step);
        }

        static void Go(IList<IList<char>> grid, bool east) {
            HashSet<(int,int)> sources = new HashSet<(int, int)>();
            HashSet<(int,int)> destinations = new HashSet<(int, int)>();
            int extent = east ? grid[0].Count : grid.Count;
            char sc = east ? '>' : 'v';
            for(int r = 0; r < grid.Count; ++r) {
                for(int c = 0; c < grid[0].Count; ++c) {
                    (int r, int c) dest = Move(r, c, east, extent);
                    if(grid[r][c] == sc && grid[dest.r][dest.c] == '.') {
                        sources.Add((r,c));
                        destinations.Add(dest);
                    }
                }
            }

            foreach((int r, int c) src in sources) { 
                grid[src.r][src.c] = '.';
            }
            foreach((int r, int c) dest in destinations) { 
                grid[dest.r][dest.c] = sc;
            }


        } 

        static (int r, int c) Move(int r, int c, bool east, int extent) {
            if(east) 
                return (r, c + 1 == extent ? 0 : c+1);
            return (r + 1 == extent ? 0 : r+1, c);
            

        }

        static string Output(IList<IList<char>> grid) {
            StringBuilder sb = new StringBuilder();
            foreach(IList<char> r in grid)
                sb.AppendLine(string.Join("", r));
            return sb.ToString();
        }

    }
}