using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;

namespace SmokeBasin.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 9;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST);

            int risklevel = 0;
            IList<(int, int)> lowPoints = new List<(int, int)>();
            for(int x = 0; x < data[0].Length; ++x) {
                for (int y = 0; y < data.Count; ++y) {
                    if (IsLowPoint(data, x, y)) {
                        risklevel += 1 + (data[y][x] - '0');
                        lowPoints.Add((x, y));
                    }
                }
            }
            Console.WriteLine(risklevel);

            IList<int> basinSizes = new List<int>();
            foreach ((int, int) lp in lowPoints)
            {
                Queue<(int, int)> q = new Queue<(int, int)>();
                IList<(int, int)> region = new List<(int, int)>();
                q.Enqueue(lp);
                while (q.Count > 0)
                {
                    // dequeue the head 
                    (int, int) focus = q.Dequeue();
                    // if's it in not in the region, add it to the region 
                    if (!region.Contains(focus))
                    {
                        region.Add(focus);
                        // check the neighbours, if it's higher and less than 9, add to the region 
                        int x = focus.Item1, y = focus.Item2;
                        if (x > 0 && data[y][x - 1] > data[y][x] && data[y][x - 1] < '9')
                            q.Enqueue((x-1, y));

                        if (x < data[y].Length-1 && data[y][x + 1] > data[y][x] && data[y][x + 1] < '9')
                            q.Enqueue((x + 1, y));

                        if (y > 0 && data[y - 1][x] > data[y][x] && data[y-1][x] < '9')
                            q.Enqueue((x, y - 1));

                        if (y < data.Count-1 && data[y + 1][x] > data[y][x] && data[y+1][x] < '9')
                            q.Enqueue((x, y + 1));


                    }
                }
            
                basinSizes.Add(region.Count);
            }

            basinSizes = basinSizes.Where(b => b > 0).OrderByDescending(b => b).ToList();

            Console.WriteLine(basinSizes[0] * basinSizes[1] * basinSizes[2]);

        }

        static bool IsLowPoint(IList<string> data, int x, int y) {
            if (x > 0 && data[y][x - 1] <= data[y][x]) // the cell to the left is lower 
                return false;
            if (x < data[0].Length - 1 && data[y][x + 1] <= data[y][x]) // the cell to the right is lower 
                return false;
            if (y > 0 && data[y - 1][x] <= data[y][x]) // the cell above is lower 
                return false;
            if (y < data.Count - 1 && data[y + 1][x] <= data[y][x]) // the cell below is lower 
                return false;
            return true;
        }
    }
}