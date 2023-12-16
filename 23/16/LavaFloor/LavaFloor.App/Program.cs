using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Data;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using System.ComponentModel;

namespace LavaFloor.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 16;
            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test) == (46, 51));

            IList<string> real = TODAY.AsListOfStrings(false);
            (int partone, int parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (int partone, int parttwo) Result(IList<string> real)
        {
            int partone = Energise(real, (0, 0, 'E'));

            IList<int> scores = new List<int>();

            for (int c = 0; c < real[0].Length; ++c)
            {
                scores.Add(Energise(real, (0, c, 'S')));
                scores.Add(Energise(real, (real.Count - 1, c, 'N')));
            }
            for (int r = 0; r < real.Count; ++r)
            {
                scores.Add(Energise(real, (r, 0, 'E')));
                scores.Add(Energise(real, (r, real[0].Length - 1, 'W')));
            }


            return (partone, scores.Max());
        }

        static int Energise(IList<string> real, (int r, int c, char heading) initial)
        {
            IList<(int r, int c, char heading)> beams = new List<(int r, int c, char heading)> { initial };
            HashSet<(int r, int c, char heading)> visited = new HashSet<(int r, int c, char heading)> { initial };
            IList<string> copy = real.Select(s => s).ToList();
            while (beams.Count > 0)
            {
                var beam = beams[0];
                beams.RemoveAt(0);
                //                Output(copy, beam);
                var moves = Result(real, beam.r, beam.c, beam.heading);
                foreach (var move in moves)
                {
                    if (!visited.Contains((move.r, move.c, move.heading)))
                    {
                        visited.Add((move.r, move.c, move.heading));
                        beams.Add(move);
                    }
                }
            }
            return visited.Select(v => (v.r, v.c)).Distinct().Count();
        }

        static void Output(IList<string> copy, (int r, int c, char heading) beam)
        {
            IDictionary<char, char> headings = new Dictionary<char, char> { { 'N', '^' }, { 'S', 'v' }, { 'E', '>' }, { 'W', '<' } };
            if (copy[beam.r][beam.c] == '.')
            {
                string row = copy[beam.r];
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < row.Length; i++)
                    sb.Append(i == beam.c ? headings[beam.heading] : row[i]);
                copy[beam.r] = sb.ToString();
            }

            foreach (string row in copy)
                Console.WriteLine(row);
            Console.WriteLine();
        }



        static IList<(int r, int c, char heading)> Result(IList<string> real, int r, int c, char heading)
        {
            IList<(int r, int c, char heading)> rv = new List<(int r, int c, char heading)>();

            // not sure there's a better way to do this than brute force
            switch (real[r][c])
            {
                case '|':
                case '-':
                    return Filter(Split(real[r][c], (r, c), heading), real.Count, real[0].Length);
                case '\\':
                case '/':
                    return Filter(Turn(real[r][c], (r, c), heading), real.Count, real[0].Length);
                case '.':
                    return Filter(Continue(real[r][c], (r, c), heading), real.Count, real[0].Length);
                default:
                    throw new Exception("Invalid cell");
            }
        }

        private static IList<(int r, int c, char heading)> Filter(IList<(int r, int c, char heading)> list, int count, int length)
        {
            return list.Where(x => x.r >= 0 && x.r < count && x.c >= 0 && x.c < length).ToList();
        }

        private static IList<(int r, int c, char heading)> Turn(char cell, (int r, int c) value, char heading)
        {
            if (cell == '/')
            {
                switch (heading)
                {
                    case 'N':
                        return new List<(int r, int c, char heading)> { (value.r, value.c + 1, 'E') };
                    case 'S':
                        return new List<(int r, int c, char heading)> { (value.r, value.c - 1, 'W') };
                    case 'E':
                        return new List<(int r, int c, char heading)> { (value.r - 1, value.c, 'N') };
                    case 'W':
                        return new List<(int r, int c, char heading)> { (value.r + 1, value.c, 'S') };
                    default:
                        throw new Exception("Invalid heading");
                }

            }
            // must be a \
            switch (heading)
            {
                case 'N':
                    return new List<(int r, int c, char heading)> { (value.r, value.c - 1, 'W') };
                case 'S':
                    return new List<(int r, int c, char heading)> { (value.r, value.c + 1, 'E') };
                case 'E':
                    return new List<(int r, int c, char heading)> { (value.r + 1, value.c, 'S') };
                case 'W':
                    return new List<(int r, int c, char heading)> { (value.r - 1, value.c, 'N') };
                default:
                    throw new Exception("Invalid heading");
            }

        }

        static IList<(int r, int c, char heading)> Split(char cell, (int r, int c) pos, char heading)
        {
            if (cell == '-')
            {
                if ("EW".Contains(heading))

                    return new List<(int r, int c, char heading)> { (pos.r, pos.c + (heading == 'E' ? 1 : -1), heading) };

                return new List<(int r, int c, char heading)> { (pos.r, pos.c - 1, 'W'), (pos.r, pos.c + 1, 'E') };
            }
            // must be a "|" 
            if ("NS".Contains(heading))
            {
                return new List<(int r, int c, char heading)> { (pos.r + (heading == 'S' ? 1 : -1), pos.c, heading) };
            }
            return new List<(int r, int c, char heading)> { (pos.r - 1, pos.c, 'N'), (pos.r + 1, pos.c, 'S') };

        }

        static IList<(int r, int c, char heading)> Continue(char cell, (int r, int c) pos, char heading)
        {
            switch (heading)
            {
                case 'N':
                    return new List<(int r, int c, char heading)> { (pos.r - 1, pos.c, heading) };
                case 'S':
                    return new List<(int r, int c, char heading)> { (pos.r + 1, pos.c, heading) };
                case 'E':
                    return new List<(int r, int c, char heading)> { (pos.r, pos.c + 1, heading) };
                case 'W':
                    return new List<(int r, int c, char heading)> { (pos.r, pos.c - 1, heading) };
                default:
                    throw new Exception("Invalid heading");
            }

        }
    }
}