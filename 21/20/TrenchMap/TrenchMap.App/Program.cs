using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;

namespace TrenchMap.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 20;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST);

            StringBuilder sb  = new StringBuilder();
            IList<string> inputImage = null;

            for(int i = 0; i < data.Count; ++i) {
                if(string.IsNullOrEmpty(data[i])) {
                    inputImage = data.Skip(i+1).ToList();
                    break;
                }
                sb.Append(data[i]);
            }

            Image.Lookup = sb.ToString();

            Image gen = new Image(inputImage);

            bool partone = false;

            for(int i = 0; i < (partone ? 2 : 50); ++i) 
                gen = gen.Generate();
            
            Console.WriteLine(gen.CountSet); 
        }        
    }

    class Image {
        public static string Lookup { get; set;}
        readonly IList<string> _image;
        char _background;

        public Image(IList<string> d) : this(d, '.') {
        }

        Image(IList<string> d, char bg) {
            _image = new List<string>(d);            
            _background = bg;
        }

        public int CountSet => _image.Select(l => l.Count(p => p == '#')).Sum();
        public int Width => _image[0].Length;
        public int Height => _image.Count;

        Image Swell() {
            IList<string> rv = new List<string>();               
            string dots = string.Join("", Enumerable.Range(0, Width+2).Select((_) => _background));
            rv.Add(dots);
            foreach(string d in _image)
                rv.Add($"{_background}{d}{_background}");
            rv.Add(dots);
            // swell with the existing background 
            return new Image(rv,_background);
        }

        public override string ToString()
        {
            return string.Join('\n', _image);
        }

        int Index(int r, int c) {
            int rv = 0; 
            for(int rd = -1; rd <= 1; ++rd) {
                for(int cd = -1; cd <= 1; ++cd) {
                    rv <<= 1;
                    char p = (r+rd < 0 || c+cd<0 || r+rd >= Height || c+cd >= Width) 
                        ? _background
                        : p = _image[r+rd][c+cd];
                    rv += p == '#' ? 1 : 0;
                }                
            }
            return rv;
        }

        public Image Generate() {
            Image swollen = Swell();
            IList<string> rv = new List<string>();
            for(int r = 0; r < swollen.Height; ++r) {
                rv.Add(string.Join("",Enumerable.Range(0,swollen.Width).Select(c => Lookup[swollen.Index(r,c)])));
            }
            // flip the background, if necessary 
            return new Image(rv, Lookup[0] == '#' && swollen._background == '.' ? '#' : '.');
        }
    }

}