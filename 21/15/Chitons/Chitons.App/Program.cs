using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;

namespace Chitons.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 15;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST);
            bool parttwo = true;

            IDictionary<(int,int), int> grid = new Dictionary<(int, int), int>();
            int ymax = data[0].Length;
            int xmax = data.Count;
            for (int r = 0; r < xmax; ++r)
            {
                for(int c = 0; c < ymax; ++c)
                {
                    grid[(r, c)] = data[r][c] - '0';
                }
            }
            if(parttwo)
            {
                IDictionary<(int, int), int> biggrid = new Dictionary<(int, int), int>();

                for (int r = 0; r < 5; ++r)
                {
                    for (int c = 0; c < 5; ++c)
                    {

                        grid.Keys.ToList()
                            .ForEach(k =>
                            { biggrid[(k.Item1 + r * xmax, k.Item2 + c * ymax)] = grid[k]+r+c; });
                        biggrid = biggrid.Select(p => p).ToDictionary(p => p.Key, p => p.Value > 9 ? p.Value - 9 : p.Value); 
                    }
                }
                grid = biggrid;
                xmax *= 5;
                ymax *= 5;
            }

            Queue<(int, int, int)> q = new Queue<(int, int, int)>();
            q.Enqueue((0, 0, 0));
            int shortest = grid.Where(k => k.Key.Item1 ==0).Sum(c => c.Value) + grid.Where(k => k.Key.Item2 == ymax-1).Sum(c => c.Value);
            IDictionary<(int, int), int> seen = new Dictionary<(int, int), int>();
            while(q.Count >0)
            {
                (int, int, int) head = q.Dequeue();
                foreach((int,int) offset in new List<(int, int)> {  (0,1), (1, 0), (0,-1), (-1,0)}) {
                    int xd = offset.Item1;
                    int yd = offset.Item2;
                    (int, int) candidate = (head.Item1 + xd, head.Item2 + yd);
                    if(grid.ContainsKey(candidate) && head.Item3+grid[candidate] < shortest)
                    {
                        if (!seen.ContainsKey(candidate) || head.Item3 + grid[candidate] < seen[candidate]) {
                            seen[candidate] = head.Item3 + grid[candidate];
                            if (candidate.Item1 == xmax - 1 && candidate.Item2 == ymax - 1)
                            {
                                shortest = head.Item3 + grid[candidate];
                            }
                            else
                            {
                                q.Enqueue((candidate.Item1, candidate.Item2, head.Item3 + grid[candidate]));
                            }
                        }
                    }
                }
            }
            Console.WriteLine(shortest);

        }
    }
}