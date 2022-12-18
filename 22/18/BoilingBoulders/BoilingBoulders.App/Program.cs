using AdventOfCode.Lib;
using System.Diagnostics;

namespace BoilingBoulders.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 18;
            IList<(int x, int y, int z)> test = TODAY.AsIntegerTriplets(true);
            Debug.Assert(Result(test) == (64, 58));

            IList<(int x, int y, int z)> real = TODAY.AsIntegerTriplets(false);
            (int partone, int parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (int partone, int parttwo) Result(IList<(int x, int y, int z)> real)
        {
            int partone = 6 * real.Count - Score(real);

            (int x, int y, int z) bottom = (real.Min(p => p.x), real.Min(p => p.y), real.Min(p => p.z));
            (int x, int y, int z) top = (real.Max(p => p.x), real.Max(p => p.y), real.Max(p => p.z));

            HashSet<(int x, int y, int z)> accessible = new();
            HashSet<(int x, int y, int z)> inaccessible = new();

            // generate all possible cubes
            for (int i = bottom.x; i <= top.x; ++i)
            {
                for (int j = bottom.y; j <= top.y; ++j)
                {
                    for (int k = bottom.z; k <= top.z; ++k)
                    {
                        // if the cube exists or we've already generated it, skip it
                        if (real.Contains((i, j, k)) || accessible.Contains((i, j, k)) || inaccessible.Contains((i, j, k)))
                            continue;

                        // generate all cubes linked from this cube - and whether they touch the surface. if one does, they all do. 
                        (bool connected, HashSet<(int x, int y, int z)>) path = Generate((i, j, k), real, accessible, bottom, top);
                        if (path.connected)
                            accessible = accessible.Union(path.Item2).ToHashSet();
                        else
                            inaccessible = inaccessible.Union(path.Item2).ToHashSet();
                    }
                }
            }

            int inacc = 6 * inaccessible.Count - Score(inaccessible.ToList());

            int parttwo = partone - inacc;
            return (partone, parttwo);
        }

        private static int Score(IList<(int x, int y, int z)> real)
        {
            int score = 0;
            for (int i = 0; i < real.Count; ++i)
                for (int j = 0; j < real.Count; ++j)
                    if (AreJoined(real, i, j))
                        ++score;
            return score;
        }

        static (bool connected, HashSet<(int x, int y, int z)>) Generate((int i, int j, int k) value,
            IList<(int x, int y, int z)> real, HashSet<(int x, int y, int z)> accessible,
            (int x, int y, int z) bottom, (int x, int y, int z) top)
        {
            HashSet<(int x, int y, int z)> join = new();
            Stack<(int x, int y, int z)> stack = new();
            stack.Push(value);

            bool surface = false;
            while (stack.Count > 0)
            {
                (int x, int y, int z) current = stack.Pop();
                join.Add(current);

                if (accessible.Contains(current))
                    surface = true;

                if (current.x == bottom.x || current.y == bottom.y || current.z == bottom.z)
                    surface = true;

                if (current.x == top.x || current.y == top.y || current.z == top.z)
                    surface = true;

                if (current.x < top.x)
                    GenerateCandidate((current.x + 1, current.y, current.z), real, join, stack);
                if (current.y < top.y)
                    GenerateCandidate((current.x, current.y + 1, current.z), real, join, stack);
                if (current.z < top.z)
                    GenerateCandidate((current.x, current.y, current.z + 1), real, join, stack);

                if (current.x > bottom.x)
                    GenerateCandidate((current.x - 1, current.y, current.z), real, join, stack);
                if (current.y > bottom.y)
                    GenerateCandidate((current.x, current.y - 1, current.z), real, join, stack);
                if (current.z > bottom.z)
                    GenerateCandidate((current.x, current.y, current.z - 1), real, join, stack);
            }
            return (surface, join);
        }

        private static void GenerateCandidate((int cx, int cy, int cz) candidate, IList<(int x, int y, int z)> real, HashSet<(int x, int y, int z)> join, Stack<(int x, int y, int z)> stack)
        {
            if (!real.Contains(candidate) && !join.Contains(candidate))
                stack.Push(candidate);
        }

        static bool AreJoined(IList<(int x, int y, int z)> positions, int i1, int i2)
        {
            return (Math.Abs(positions[i1].x - positions[i2].x)
                  + Math.Abs(positions[i1].y - positions[i2].y)
                  + Math.Abs(positions[i1].z - positions[i2].z)) == 1;
        }
    }
}