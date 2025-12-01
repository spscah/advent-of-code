using AdventOfCode.Lib;
using System.Diagnostics;
using System.Reflection.Emit;

namespace StepCounter.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 21;
            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test, 6) == (16, 0));

            IList<string> real = TODAY.AsListOfStrings(false);
            (uint partone, uint parttwo) result = Result(real, 64);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
            int x = -1 % 10;
        }

        static (uint partone, uint parttwo) Result(IList<string> grid, int limit)
        {
            uint partone = StepCounter(grid, limit);
            uint parttwo = 0;
            return (partone, parttwo);
        }

        static uint StepCounter(IList<string> grid, int limit)
        {
            HashSet<(int, int)> generation = new HashSet<(int, int)>();
            uint iteration = 0;
            for (int r = 0; r < grid.Count; ++r)
                for (int c = 0; c < grid[r].Length; ++c)
                    if (grid[r][c] == 'S')
                        generation.Add((r, c));

            for (iteration = 1; iteration <= limit; ++iteration)
            {
                HashSet<(int, int)> nextGeneration = new HashSet<(int, int)>();
                foreach ((int r, int c) in generation)
                {
                    foreach ((int dr, int dc) in new (int, int)[] { (-1, 0), (1, 0), (0, -1), (0, 1) })
                    {
                        if (dr == 0 && dc == 0)
                            continue;
                        if (r + dr < 0 || r + dr >= grid.Count)
                            continue;
                        if (c + dc < 0 || c + dc >= grid[r].Length)
                            continue;
                        if (grid[r + dr][c + dc] == '#')
                            continue;
                        nextGeneration.Add((r + dr, c + dc));
                    }
                }

                generation = nextGeneration;
            }

            return (uint)generation.Count;
        }
    }
}