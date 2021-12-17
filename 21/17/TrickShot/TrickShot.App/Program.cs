using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace TrickShot.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 17;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST);

            Regex rx = new Regex(@"^target area: x=(\d+)\.\.(\d+), y=-(\d+)\.\.-(\d+)");

            MatchCollection matches = rx.Matches(data[0]);

            int x1 = Convert.ToInt32(matches[0].Groups[1].Value);
            int x2 = Convert.ToInt32(matches[0].Groups[2].Value);
            int y2 = -Convert.ToInt32(matches[0].Groups[3].Value);
            int y1 = -Convert.ToInt32(matches[0].Groups[4].Value);

            if(TEST) {
                foreach((int,int,int?) c in new List<(int,int,int?)>{(7,2,3), (6,3,6), (9,0,0), (17,-4,null)}) {
                    int a=0;
                    bool res = DoesItHit(c.Item1,c.Item2,x1,x2,y1,y2, out a);                    
                    Debug.Assert(res == c.Item3.HasValue);
                    if(res)
                        Debug.Assert(a == c.Item3.Value);
                }
            }

            // quite literally hit and miss for the ranges of things to try 
            IList<int> apogees = new List<int>();
            IList<(int,int)> velocities = new List<(int,int)>();
            for(int xd = -20; xd < 250; ++xd) {
                for(int yd = 1000; yd >= -1000; --yd) {
                    if(DoesItHit(xd,yd,x1,x2,y1,y2, out int apogee)) {
                        apogees.Add(apogee);
                        velocities.Add((xd,yd));
                    }
                }
            }

            Console.WriteLine(apogees.Max()); // 0..50, -25..+25 - 325 is too low, -20..50, -50..+55 - 1540
            Console.WriteLine(velocities.Distinct().Count()); // more than 592 - much more - 4748 

        }

        static bool DoesItHit(int xd, int yd, int x1, int x2, int y1, int y2, out int apogee) {
            (int, int) position = (0,0);
            apogee = y2-1;
            while(true) {
                position = (position.Item1 + xd, position.Item2 + yd);
                if(position.Item2 > apogee)
                    apogee = position.Item2;
                if(position.Item1 >= x1 && position.Item1 <= x2 && position.Item2 >= y2 && position.Item2 <= y1)
                    return true;
                if(xd != 0)
                    xd += xd > 0 ? -1 : 1;
                yd -= 1;
                // question is then, when to call it a day? 
                if(yd < 0 && position.Item2 < y2)
                    return false;
                if(xd >= 0 && position.Item1 > x2)
                    return false;
                if(xd <= 0 && position.Item1 < x1)
                    return false;                 
            }
        }
    }
}