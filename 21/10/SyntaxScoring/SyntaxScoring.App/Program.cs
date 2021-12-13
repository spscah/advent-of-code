using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;

namespace SyntaxScoring.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 10;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST);

            IDictionary<char, int> part1scores = new Dictionary<char, int> { { ')', 3 }, { ']', 57 }, { '}', 1197 }, { '>', 25137 } };
            IDictionary<char, int> part2scores = new Dictionary<char, int> { { ')', 1 }, { ']', 2 }, { '}', 3 }, { '>', 4} };
            IDictionary<char, char> pairs = new Dictionary<char, char> { { '(', ')' }, { '[',']' }, {'{', '}'}, {'<', '>' } };

            int partone = 0;
            IList<long> part2 = new List<long>();
            foreach (string s in data)
            {
                Stack<char> stack = new Stack<char>();
                int score = 0;
                foreach(char c in s)
                {
                    if (pairs.ContainsKey(c))
                        stack.Push(pairs[c]);
                    else
                    {
                        char head = stack.Pop();
                        if(head != c)
                        {
                            score = part1scores[c];
                            break;
                        }
                    }
                }
                if(score == 0 && stack.Count > 0)
                {
                    Queue<char> q = new Queue<char>();
                    while (stack.Count > 0)
                        q.Enqueue(stack.Pop());
                    long parttwo = 0;
                    while(q.Count > 0)
                    {
                        parttwo *= 5;
                        parttwo += part2scores[q.Dequeue()];
                    }
                    part2.Add(parttwo);
                }


                partone += score;
            }
            Console.WriteLine(partone);
            Console.Write(part2.OrderBy(p => p).ToList()[(part2.Count - 1) / 2]);
            // 264368900 is too low 
        }
    }
}
