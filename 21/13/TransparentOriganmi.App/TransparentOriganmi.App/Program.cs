using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;

namespace TransparentOriganmi.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 13;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST);

            IList<(int, int)> originals = new List<(int, int)>();
            IList<(char, int)> actions = new List<(char, int)>();
            foreach(string d in data)
            {
                if(d.Contains(','))
                {
                    IList<int> xy = d.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                    originals.Add((xy[0], xy[1]));
                }
                if(d.Contains("along "))
                {
                    IList<string> action = d.TrimEnd().Split("along ")[1].Split('=').ToList();
                    actions.Add((action[0][0], Convert.ToInt32(action[1])));
                }
            }

            bool partone = true;
            IList<(int, int)> reflections = new List<(int, int)>(originals);
            foreach ((char, int) action in actions)
            {
                if(action.Item1 == 'y')
                {
                    int a = action.Item2;
                    IList<(int, int)> below = reflections.Where(xy => xy.Item2 <= a).ToList();
                    IList<(int, int)> above = reflections.Where(xy => xy.Item2 > a).ToList();
                    above = above.Select(xy => (xy.Item1, 2 * a - xy.Item2)).ToList();
                    reflections = below.Union(above).Distinct().ToList();
                }
                if (action.Item1 == 'x')
                {
                    int a = action.Item2;
                    IList<(int, int)> left = reflections.Where(xy => xy.Item1 <= a).ToList();
                    IList<(int, int)> right = reflections.Where(xy => xy.Item1 > a).ToList();
                    right = right.Select(xy => (2 * a - xy.Item1, xy.Item2)).ToList();
                    reflections = left.Union(right).Distinct().ToList();
                }
                if (partone)
                    break;
            }
            if (partone)
            {
                Console.WriteLine(reflections.Count);
            }
            else
            {
                int minX = reflections.Min(t => t.Item1);
                int maxX = reflections.Max(t => t.Item1);
                int minY = reflections.Min(t => t.Item2);
                int maxY = reflections.Max(t => t.Item2);

                StringBuilder sb = new StringBuilder();
                for (int y = minY; y <= maxY; ++y)
                {
                    for (int x = minX; x <= maxX; ++x)
                    {
                        sb.Append(reflections.Contains((x, y)) ? '#' : '.');
                    }
                    sb.AppendLine();
                }
                Console.WriteLine(sb.ToString());
            }
        }
    }
    
}
