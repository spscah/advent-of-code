using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace Scratchcards.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 4;
            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test) == (13, 30));

            IList<string> real = TODAY.AsListOfStrings(false);
            (int partone, int parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (int partone, int parttwo) Result(IList<string> real)
        {
            int score = 0;
            IList<int> cards = Enumerable.Range(1, real.Count).Select(x => 1).ToList();
            int index = 0;
            foreach (string line in real)
            {
                var halves = line.Split(':')[1].Split("|");
                var first = halves[0].Trim().Split(" ").Where(x => x != "").Select(x => int.Parse(x.Trim())).ToList();
                var second = halves[1].Trim().Split(" ").Where(x => x != "").Select(x => int.Parse(x.Trim())).ToList();
                var winners = first.Intersect(second).ToList();
                if (winners.Count > 0)
                {
                    score += (int)Math.Pow(2, winners.Count() - 1);
                    for (int i = 1; i <= winners.Count; ++i)
                    {
                        cards[index + i] += cards[index];
                    }
                }
                ++index;
            }
            return (score, cards.Sum());

        }
    }
}