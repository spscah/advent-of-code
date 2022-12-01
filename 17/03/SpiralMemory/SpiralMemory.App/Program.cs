using System.Diagnostics;

namespace SpiralMemory.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Debug.Assert(PartOne(1) == 0);
            Debug.Assert(PartOne(12) == 3);
            Debug.Assert(PartOne(23) == 2);
            Debug.Assert(PartOne(1024) == 31);

            Console.WriteLine($"Part One: {PartOne(289326)}");

            Debug.Assert(PartTwo(100) == 122);
            Debug.Assert(PartTwo(750) == 806);

            Console.WriteLine($"Part One: {PartTwo(289326)}");
        }

        static int PartOne(int x)
        {
            // the shells go up with the odd numbers, 1, 3, 5 and so on 
            if (x == 1)
                return 0;
            // first, which odd square is bigger than x 
            int square = 1;
            while (square * square < x)
                square += 2;

            // start at the square on the previous shell 
            int current = (square - 2) * (square - 2);
            // each side of the shell has (square-1) 
            while (current + square - 1 < x)
                current += square - 1;

            int extent = x - current;
            extent -= (square - 1) / 2;

            extent = Math.Abs(extent);

            return (square - 1) / 2 + extent;
        }

        static int PartTwo(int x) { 
            IDictionary<(int,int), int> values = new Dictionary<(int, int), int> ();
            values.Add((0,0), 1);
            int square = 3; 
            (int x, int y) location = (0,0);
            while(true){
                int xd, yd;
                location = (location.x+1, location.y-1);
                for(int side = 0; side < 4; ++side) {
                    // 0 1 
                    // -1 0
                    // 0 -1 
                    // 1 0 
                    xd = 0; yd=0;
                    switch(side) {
                        case(0):
                            yd = 1;
                            break;
                        case(1):
                            xd = -1;
                            break;
                        case(2):
                            yd = -1;
                            break;
                        case(3):
                            xd = 1;
                            break;
                    }
                    for(int i = 0; i < square-1; ++i) {
                        location = (location.x + xd, location.y + yd);
                        int lc = LocationCount(values, location);
                        if(lc > x)
                            return lc;
                        values.Add(location, lc);
                    }
                }
                

                square +=2;
            }
        }

        static int LocationCount(IDictionary<(int, int), int> values, (int x, int y) location)
        {
            int tally = 0;
            for(int x = -1; x < 2; ++x) {
                for(int y = -1; y < 2; ++y) {
                    if(x == 0 && y == 0)
                        continue;
                    (int, int) target = (location.x + x, location.y + y);
                    if(values.ContainsKey(target))
                        tally += values[target];
                }    
            }
            return tally;
        }
    }
}