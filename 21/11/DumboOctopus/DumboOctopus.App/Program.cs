using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;

namespace DumboOctopus.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 11;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST);

            IList<List<int>> grid = data.Select(d => d.Select(c => c-'0').ToList()).ToList();

            bool partone = false;

            int cFlashed = 0;
            int step = 0;
            while(true) {
                IList<(int,int)> flashed = IncrementGrid(grid);
                FlashACell(grid, flashed);
                cFlashed += flashed.Count;
                Reset(grid);
                ++step;
                if(partone && step == 100) {
                    Console.WriteLine(cFlashed);     
                    break;               
                }
                if(!partone && flashed.Count == grid.Count * grid[1].Count)
                {
                    Console.WriteLine(step);
                    break;
                }
                //Console.WriteLine($"Step {step} / {cFlashed}");
                //Console.WriteLine(string.Join("\n", grid.Select(row => string.Join("", row))));
            }

        }

        static IList<(int,int)> IncrementGrid(IList<List<int>> grid) {
            IList<(int,int)> needToFlash = new List<(int,int)>();
            for(int r = 0; r < grid.Count; ++r) {
                List<int> row = grid[r];
                for(int c = 0; c < row.Count; ++c) {
                    ++row[c];
                    if(row[c] > 9)
                        needToFlash.Add((r,c));
                }
            }
            return needToFlash;
        }

        static void Reset(IList<List<int>> grid) {
            for(int r = 0; r < grid.Count; ++r) {
                List<int> row = grid[r];
                for(int c = 0; c < row.Count; ++c) {
                    if(row[c] > 9)
                        row[c] = 0;
                }
            }
        }

        static void FlashACell(IList<List<int>> grid, IList<(int,int)> cells, int idx=0) {
            if(idx >= cells.Count)
                return; 
            (int, int) cell = cells[idx];
            for(int rd = -1; rd <= 1; ++rd) { 
                for(int cd = -1; cd <= 1; ++cd) { 
                    if(rd == 0 && cd == 0) 
                        continue;
                    if((cell.Item1 + rd >= 0) && (cell.Item1 + rd) < grid[0].Count && 
                       (cell.Item2 + cd >= 0) && (cell.Item2 + cd) < grid.Count) {
                            ++grid[cell.Item1+rd][cell.Item2+cd];
                            if(grid[cell.Item1+rd][cell.Item2+cd] > 9 && 
                                !cells.Contains((cell.Item1+rd, cell.Item2+cd)) ) {
                                    cells.Add((cell.Item1+rd, cell.Item2+cd));
                            }
                       }
                }
            } 
            FlashACell(grid, cells, idx+1);
        }
    }
}