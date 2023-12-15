using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace LensLibrary.App
{
    class Program
    {
        static void Main(string[] args)
        {
            Debug.Assert(Hash("HASH") == 52);
            Debug.Assert(Hash("rn=1") == 30);
            Debug.Assert(Hash("ot=7") == 231);

            const int TODAY = 15;
            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test) == (1320, 145));

            IList<string> real = TODAY.AsListOfStrings(false);
            (int partone, int parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (int partone, int parttwo) Result(IList<string> real)
        {
            IList<string> tokens = real[0].Split(',').ToList();

            int partone = tokens.Select(t => Hash(t)).Sum();

            return (partone, PartTwo(tokens));
        }

        static int Hash(string input)
        {
            int value = 0;
            foreach (char c in input)
            {
                value += c;
                value *= 17;
                value %= 256;
            }
            return value;
        }

        static int PartTwo(IList<string> tokens)
        {
            IList<List<(string, int)>> boxes = Enumerable.Range(0, 256).Select(__ => new List<(string, int)>()).ToList();

            foreach (string token in tokens)
            {
                int labellength = token.Length - ((token.Last() == '-') ? 1 : 2);

                string label = token.Substring(0, labellength);
                int hash = Hash(label);
                if (token[labellength] == '=')
                {
                    IList<(string label, int fl)> lenses = boxes[hash];
                    bool found = false;
                    int index = 0;
                    (string, int) lens = (label, token[labellength + 1] - '0');
                    while (index < lenses.Count)
                    {
                        if (lenses[index].label == label)
                        {
                            lenses[index] = lens;
                            found = true;
                            break;
                        }
                        ++index;
                    }
                    if (!found)
                        lenses.Add(lens);
                }
                else if (token[labellength] == '-')
                {
                    IList<(string label, int fl)> lenses = boxes[hash];
                    for (int i = 0; i < lenses.Count; ++i)
                    {
                        if (lenses[i].label == label)
                        {
                            lenses.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            int value = 0;
            for (int box = 0; box < boxes.Count; ++box)
                for (int slot = 0; slot < boxes[box].Count; ++slot)
                    value += (box + 1) * (slot + 1) * boxes[box][slot].Item2;

            return value;
            // 54931 -- too low 
        }
    }
}