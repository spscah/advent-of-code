using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MonkeyMap.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 22;
            IList<string> test = TODAY.AsListOfStrings(true, true);
            Debug.Assert(Result(test) == (6032, 0));

            IList<string> real = TODAY.AsListOfStrings(false, true);
            (int partone, int parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (int partone, int parttwo) Result(IList<string> real)
        {

            IList<string> map = real.Select(r => r.TrimEnd()).Take(real.Count - 2).ToList();
            int width = map.Select(m => m.Length).Max();
            int height = map.Count;

            map = map.Select(m => m.PadRight(width, ' ')).ToList();

            int col = map[0].IndexOf('.');
            int row = 0;

            List<(int r, int c)> directions = new List<(int r, int c)> { (0, 1), (1, 0), (0, -1), (-1, 0) };
            Direction dir = Direction.Right;

            string pattern = @"([0-9]+)([LR])?(.*)$";
            string description = real.Last().TrimEnd();
            while (description.Length > 0)
            {
                var match = System.Text.RegularExpressions.Regex.Match(description, pattern);
                int steps = int.Parse(match.Groups[1].Value);
                description = match.Groups[3].Value;
                for (int i = 0; i < steps; i++)
                {
                    int ccol = (col + directions[(int)dir].c + width) % width;
                    int crow = (row + directions[(int)dir].r + height) % height;
                    // if it's a dot, move there
                    if (map[crow][ccol] == '.')
                    {
                        (col, row) = (ccol, crow);
                    }
                    else
                    {
                        // if it's a space, rewind to the opposite edge 
                        if (map[crow][ccol] == ' ')
                        {
                            if (directions[(int)dir].c == 0)
                            {
                                // we're on a column 
                                if (directions[(int)dir].r == 1)
                                {
                                    // we're going down, so find smallest row with a . in this column                                     
                                    crow = Enumerable.Range(0, height).Where(r => map[r][col] != ' ').Min();
                                }
                                else
                                {
                                    // we're going up
                                    crow = Enumerable.Range(0, height).Where(r => map[r][col] != ' ').Max();
                                }
                                row = map[crow][col] == '.' ? crow : row;
                            }
                            else
                            {
                                // we're on a row
                                if (directions[(int)dir].c == 1)
                                {
                                    // we're going right, so find smallest column with a . in this column                                     
                                    ccol = Enumerable.Range(0, width).Where(c => map[row][c] != ' ').Min();
                                }
                                else
                                {
                                    // we're going left
                                    ccol = Enumerable.Range(0, width).Where(c => map[row][c] != ' ').Max();
                                }
                                col = map[row][ccol] == '.' ? ccol : col; ;
                            }
                        }
                    }
                }
                if (match.Groups[2].Success)
                    dir = match.Groups[2].Value == "L" ? (Direction)(((int)dir + 3) % 4) : (Direction)(((int)dir + 1) % 4);

            }
            int partone = 1000 * (row + 1) + 4 * (col + 1) + (int)dir;


            Dictionary<(int f, Direction d), (int f, int clockwise)> changes = new()
            {
                {(6,Direction.Up), (4,3)},
                {(6,Direction.Down), (2,1)},
                {(6,Direction.Right), (1,2)},
                {(6,Direction.Left), (5,0)},

                {(5, Direction.Up), (4,0)},
                {(5, Direction.Down), (2,2)},
                {(5, Direction.Left), (3,3)},
                {(5, Direction.Right), (6,0)},

                {(4,Direction.Up), (1,0)},
                {(4,Direction.Down), (5,0)},
                {(4,Direction.Left), (3,0)},
                {(4,Direction.Right), (6,3)},

                {(3,Direction.Up), (1,3)},
                {(3,Direction.Down), (5,1)},
                {(3,Direction.Left), (2,0)},
                {(3,Direction.Right), (4,3)},

                {(2,Direction.Up), (1,2)},
                {(2,Direction.Down), (5,2)},
                {(2,Direction.Left), (6,3)},
                {(2,Direction.Right), (3,0)},

                {(1,Direction.Up), (2,2)},
                {(1,Direction.Down), (4,0)},
                {(1,Direction.Left), (3,1)},
                {(1,Direction.Right), (6,2)},
            };

            int sWidth = (width + 1) / 4;
            Debug.Assert(sWidth == (height + 1) / 3);

            Dictionary<int, (int r, int c)> topsLeft = new() {
                {1, (0, 2*sWidth)},
                {2, (sWidth, 0)},
                {3 , (sWidth, sWidth)},
                {4 , (sWidth, sWidth*2)},
                {5 , (sWidth*2, sWidth*2)},
                {6 , (sWidth*2, sWidth*3)}
            };


            Dictionary<(int, int), IList<string>> faces = new();
            for (int f = 0; f < 6; f++)
            {
                faces[(f + 1, 0)] = GetFace(map, topsLeft[f + 1], sWidth);
                for (int r = 0; r < 3; ++r)
                    faces[(f + 1, r + 1)] = RotateOnce(faces[(f + 1, r)]);
            }

            (int f, int cl) faceOrientation = (1, 0);
            row = 0;
            col = 0;
            Direction actualDirection = Direction.Right;
            Direction theoreticalDirection = Direction.Right;

            description = real.Last().TrimEnd();
            while (description.Length > 0)
            {
                var match = System.Text.RegularExpressions.Regex.Match(description, pattern);
                int steps = int.Parse(match.Groups[1].Value);
                description = match.Groups[3].Value;
                var currentFace = faces[(faceOrientation.f, faceOrientation.cl)];
                for (int i = 0; i < steps; i++)
                {
                    int ccol = col + directions[(int)theoreticalDirection].c;
                    int crow = row + directions[(int)theoreticalDirection].r;

                    if (ccol >= 0 && ccol < sWidth && crow >= 0 && crow < sWidth)
                    {
                        if (currentFace[crow][ccol] == '.')
                        {
                            (col, row) = (ccol, crow);
                        } // else it's a wall, so we don't move
                    }
                    else
                    {
                        Direction ctd = theoreticalDirection;
                        (int f, int cl) cfo = faceOrientation;
                        // we're going off the edge, so we need to find the neighbouring face
                        if (ccol < 0)
                        {
                            ctd = (Direction)(((int)faceOrientation.cl - (int)Direction.Left + 4) % 4);
                            cfo = changes[(faceOrientation.f, ctd)];
                            ccol = sWidth - 1;
                        }
                        else if (ccol >= sWidth)
                        {
                            ctd = (Direction)(((int)faceOrientation.cl - (int)Direction.Right + 4) % 4);
                            cfo = changes[(faceOrientation.f, ctd)];
                            ccol = 0;
                        }
                        else if (crow < 0)
                        {
                            ctd = (Direction)(((int)faceOrientation.cl - (int)Direction.Up + 4) % 4);
                            cfo = changes[(faceOrientation.f, ctd)];
                            crow = sWidth - 1;
                        }
                        else if (crow >= sWidth)
                        {
                            ctd = (Direction)(((int)faceOrientation.cl - (int)Direction.Down + 4) % 4);
                            cfo = changes[(faceOrientation.f, ctd)];
                            crow = 0;
                        }
                        (crow, ccol) = Translate((crow, ccol), faceOrientation.cl, sWidth);
                        if (faces[cfo][crow][ccol] == '.')
                        {
                            (col, row) = (ccol, crow);
                            faceOrientation = cfo;
                            currentFace = faces[(faceOrientation.f, faceOrientation.cl)];
                            theoreticalDirection = ctd;
                            actualDirection = (Direction)(((int)ctd - faceOrientation.cl + 4) % 4);
                        }
                    }
                }
                if (match.Groups[2].Success)
                {
                    actualDirection = match.Groups[2].Value == "L" ? (Direction)(((int)actualDirection + 3) % 4) : (Direction)(((int)actualDirection + 1) % 4);
                    theoreticalDirection = match.Groups[2].Value == "L" ? (Direction)(((int)theoreticalDirection + 3) % 4) : (Direction)(((int)theoreticalDirection + 1) % 4);
                }
            }

            var actualposition = Translate((row, col), (4 - faceOrientation.cl) % 4, sWidth);
            var offsets = topsLeft[faceOrientation.f];
            int parttwo = (actualposition.r + offsets.r + 1) * 1000 + (actualposition.c + offsets.c + 1) + (int)actualDirection;

            return (partone, parttwo);
        }

        static IList<string> GetFace(IList<string> map, (int r, int c) topleft, int w)
        {
            List<string> face = new List<string>();
            for (int r = 0; r < w; r++)
            {
                face.Add(map[topleft.r + r].Substring(topleft.c, w));
            }
            return face;
        }

        static IList<string> RotateOnce(IList<string> face)
        {
            int w = face[0].Length;
            Dictionary<(int r, int c), char> rotated = new();

            for (int r = 0; r < w; r++)
            {
                for (int c = 0; c < w; c++)
                {
                    var n = Translate((r, c), 1, w);
                    rotated.Add(n, face[r][c]);
                }
            }

            List<string> rv = new();
            for (int r = 0; r < w; ++r)
            {
                StringBuilder sb = new();
                for (int c = 0; c < w; ++c)
                    sb.Append(rotated[(r, c)]);
                rv.Add(sb.ToString());
            }
            return rv;
        }


        static (int r, int c) Translate((int r, int c) pt, int clockwise, int w)
        {
            for (int rot = 0; rot < clockwise; rot++)
                pt = (pt.c, -pt.r + w - 1);
            return pt;
        }

        enum Direction { Up = 3, Right = 0, Down = 1, Left = 2 }
    }
}