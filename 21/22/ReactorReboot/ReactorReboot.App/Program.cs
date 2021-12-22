using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ReactorReboot.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 22;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST);

            Cube cube = new Cube(-50,50);
            IList<Cuboid> gen0 = new List<Cuboid>();
            foreach(string d in data) {
                Regex rx = new Regex(@"(.+) x=(.+)\.\.(.+),y=(.+)\.\.(.+),z=(.+)\.\.(.+)");            

                MatchCollection matches = rx.Matches(d);

                bool on = matches[0].Groups[1].Value.Equals("on");
                int x1 = Convert.ToInt32(matches[0].Groups[2].Value);
                int x2 = Convert.ToInt32(matches[0].Groups[3].Value);
                int y1 = Convert.ToInt32(matches[0].Groups[4].Value);
                int y2 = Convert.ToInt32(matches[0].Groups[5].Value);
                int z1 = Convert.ToInt32(matches[0].Groups[6].Value);
                int z2 = Convert.ToInt32(matches[0].Groups[7].Value);

                gen0.Add(new Cuboid((x1,y1,z1),(x2,y2,z2), on));

                if(new List<int> { x1,y1,z1,x2,y2,z2}.Any(d => d < -50 || d > 50))
                    continue; 
                
                for(int i = x1; i <= x2; ++i) { 
                    for(int j = y1; j <= y2; ++j) {
                        for(int k = z1; k <= z2; ++k) {
                            cube.Switch((i,j,k), on);
                        }
                    }
                }
            }

            Console.WriteLine(cube.NumberActive); 

            List<Cuboid> generalpopulation = new List<Cuboid>(); 
            foreach(Cuboid cuboid in gen0) {
                IList<Cuboid> overlaps = 
                    generalpopulation
                        .Where(gp => cuboid.HasOverlaps(gp))
                        .Select(prev => cuboid.Overlaps(prev, !prev.IsOn))
                        .ToList();                    
                generalpopulation.AddRange(overlaps);
                if(cuboid.IsOn)
                    generalpopulation.Add(cuboid);                    
            }

            ulong sum = 0;
            foreach(Cuboid c in generalpopulation) 
                sum += c.Assay;
            Console.WriteLine(sum);
            

        }

        class Cube { 
            readonly int _min; 
            readonly int _max; 
            readonly IList<bool> _state; 


            int Dim => _max+1 - _min;
            public int NumberActive => _state.Count(c => c == true);

            internal Cube(int min, int max) {
                _max = max;
                _min = min; 
                _state = Enumerable.Range(0, (int)Math.Pow(Dim, 3)).Select((_) => false).ToList();
            }

            internal void Switch((int x, int y, int z) value, bool state) {
                _state[Transform(value)] = state;
            }

            int Transform((int x, int y, int z) value) {
                return ZTransform(value.z - _min) + YTransform(value.y - _min) + value.x - _min;
            }

            int YTransform(int y) {
                return y * Dim;
            }

            int ZTransform(int z) {
                return z * Dim * Dim;
            }

        }

        class Cuboid {
            readonly (int x, int y, int z) _lowCorner; 
            readonly (int x, int y, int z) _highCorner;

            bool _on;  
            public bool IsOn => _on;

            public ulong Assay => 
                  (ulong)(_on ? 1 : -1)
                  *(ulong)(1+_highCorner.x-_lowCorner.x)
                  *(ulong)(1+_highCorner.y-_lowCorner.y)
                  *(ulong)(1+_highCorner.z-_lowCorner.z) ;

            internal Cuboid((int x, int y, int z) c1, (int x, int y, int z) c2, bool state) {
                _lowCorner = c1; //Lower(c1,c2);
                _highCorner = c2; // Upper(c1,c2);
                _on = state;
            }

            public bool HasOverlaps(Cuboid other) =>
                 !(_highCorner.x < other._lowCorner.x || _lowCorner.x > other._highCorner.x 
                || _highCorner.y < other._lowCorner.y || _lowCorner.y > other._highCorner.y
                || _highCorner.z < other._lowCorner.z || _lowCorner.z > other._highCorner.z);

            public Cuboid Overlaps(Cuboid other, bool state)
            {
                return new Cuboid(
                    (Math.Max(_lowCorner.x, other._lowCorner.x), 
                     Math.Max(_lowCorner.y, other._lowCorner.y), 
                     Math.Max(_lowCorner.z, other._lowCorner.z)),
                    (Math.Min(_highCorner.x, other._highCorner.x), 
                    Math.Min(_highCorner.y, other._highCorner.y), 
                    Math.Min(_highCorner.z, other._highCorner.z)), state);
            }

            public override string ToString()
            {
                return $"{_lowCorner} => {_highCorner} ({_on}) = {Assay}";
            }
        }
    }

 
    
}