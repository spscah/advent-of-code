using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace SupplyStacks.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 5;
            foreach (bool test in new List<bool> { true, false })
            {

                IList<string> data = TODAY.AsListOfStrings(test, true);

                (string p1, string p2) result = (Stacks(data, true), Stacks(data, false));

                if (test)
                {
                    Debug.Assert(result.p1 == "CMZ");
                    Debug.Assert(result.p2 == "MCD");
                }
                else
                {
                    Console.WriteLine($"Part One: {result.p1}");
                    Console.WriteLine($"Part Two: {result.p2}");
                }
            }
        }

        static string Stacks(IList<string> data, bool partone)
        {
            string result = string.Empty;

            bool header = true;

            IList<Stack<char>> stacks = null;
            Stack<string> initial = new();
            foreach (string line in data)
            {
                if (string.IsNullOrEmpty(line))
                    header = false;
                else if (line[1] == '1')
                {
                    // unpop the stack
                    stacks = Enumerable.Range(0, (line.Length + 1) / 4).Select(_ => new Stack<char>()).ToList();

                    while (initial.Count > 0)
                    {
                        string s = initial.Pop();
                        for (int index = 0; (4 * index) < s.Length; ++index)
                        {
                            string bit = s.Substring(index * 4);
                            if (!string.IsNullOrEmpty(bit) && bit[1] != ' ')
                                stacks[index].Push(bit[1]);
                        }
                    }
                }

                if (header)
                    initial.Push(line);
                else
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    // do the instructions 
                    IList<string> bits = line.Split(' ').ToList();
                    Debug.Assert(bits.Count == 6);
                    int num = Convert.ToInt32(bits[1]);
                    int f = Convert.ToInt32(bits[3]) - 1;
                    int t = Convert.ToInt32(bits[5]) - 1;

                    Debug.Assert(stacks != null);
                    Stack<char> localstack;
                    if (partone)
                        localstack = stacks[f];
                    else
                    {
                        localstack = new Stack<char>();
                        for (int _ = 0; _ < num; ++_)
                            localstack.Push(stacks[f].Pop());
                    }

                    for (int _ = 0; _ < num; ++_)
                    {
                        stacks[t].Push(localstack.Pop());
                    }

                }
            }
            Debug.Assert(stacks != null);
            return string.Join("", stacks.Select(s => s.Peek()));
        }
    }
}