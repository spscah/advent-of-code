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
            const int TODAY = 5;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST);

            int p1 = Work(data);
            if(TEST)
                Debug.Assert(p1 == 5);
            else
                Console.WriteLine(p1);
            int p2 = Work(data, false);
            if(TEST)
                Debug.Assert(p2 == 12);
            else
                Console.WriteLine(p2);
        }

        private static int Work(IList<string> data, bool part1 = true)
        {
            IList<(int,int)> froms = new List<(int,int)>();
            IList<(int,int)> tos = new List<(int,int)>();
            foreach(string s in data) {
                IList<string> halves = s.Split(" -> ").ToList();
                froms.Add(ToTuple(halves[0]));
                tos.Add(ToTuple(halves[1]));
            }
            int maxX = froms.Select(t => t.Item1).Union(tos.Select(t => t.Item1)).Max() + 1;
            int maxY = froms.Select(t => t.Item2).Union(tos.Select(t => t.Item2)).Max() + 1;
            IList<int> grid = Enumerable.Range(1, maxX*maxY).Select(i => 0).ToList();
            for(int i = 0; i < froms.Count; ++i) {                
                if(froms[i] == tos[i]) {
                    Increment(grid, maxX, froms[i]);
                } else {
                    if(froms[i].Item1 == tos[i].Item1) {
                        int f = froms[i].Item2; 
                        int t = tos[i].Item2; 
                        int lower = f < t ? f : t;
                        int upper = f < t ? t : f;
                        for(int j = lower; j <= upper; ++j)
                            Increment(grid, maxX, (froms[i].Item1, j));
                    }
                    if(froms[i].Item2 == tos[i].Item2) {
                        int f = froms[i].Item1; 
                        int t = tos[i].Item1; 
                        int lower = f < t ? f : t;
                        int upper = f < t ? t : f;
                        for(int j = lower; j <= upper; ++j)
                            Increment(grid, maxX, (j, froms[i].Item2));
                    }
                }
                if(!part1 && froms[i].Item1 != tos[i].Item1 && froms[i].Item2 != tos[i].Item2) {
                    int deltaX = froms[i].Item1 < tos[i].Item1 ? 1 : -1;
                    int deltaY = froms[i].Item2 < tos[i].Item2 ? 1 : -1;
                    for(int j = 0; j <= Math.Abs(froms[i].Item1 - tos[i].Item1); ++j) 
                        Increment(grid, maxX, (froms[i].Item1+(j*deltaX), froms[i].Item2+(j*deltaY)));


                }
            }
            return grid.Count(c => c > 1);
        }

        static string Output(IList<int> grid, int limX) {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < grid.Count; ++i){
                sb.Append(grid[i] == 0 ? "." : grid[i]);
                if((i + 1) % limX == 0)
                    sb.AppendLine();
            }
            sb.AppendLine();
            return sb.ToString();
        }


        static void Increment(IList<int> grid, int limX, (int,int) point) {
            grid[point.Item2 * limX + point.Item1]++;
        }

        static (int,int) ToTuple(string s) {
            int loc = s.IndexOf(',');
            int i1 = Convert.ToInt32(s.Substring(0,loc));
            int i2 = Convert.ToInt32(s.Substring(loc+1));
            return (i1, i2);
        }
    }
}