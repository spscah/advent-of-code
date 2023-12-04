using AdventOfCode.Lib;
using System.Diagnostics;

namespace CodeConundrum.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 2;
            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test) == (8, 2286));

            IList<string> real = TODAY.AsListOfStrings(false);
            (int partone, int parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (int partone, int parttwo) Result(IList<string> real)
        {
            IList<int> ids = new List<int>();
            IList<int> powers = new List<int>();
            IDictionary<string, int> limits = new Dictionary<string, int> { { "red", 12 }, { "green", 13 }, { "blue", 14 } };
            foreach (string line in real)
            {
                bool breached = false;
                int colon = line.IndexOf(':');
                int id = int.Parse(line.Substring(5, colon - 5));
                var grabs = line.Substring(colon + 2).Split("; ").ToList();
                IDictionary<string, IList<int>> shows = new Dictionary<string, IList<int>> { { "red", new List<int>() }, { "green", new List<int>() }, { "blue", new List<int>() } };
                foreach (string grab in grabs)
                {
                    var balls = grab.Split(", ").ToList();
                    foreach (string ball in balls)
                    {
                        var combo = ball.Split(" ").ToList();
                        int x = int.Parse(combo[0]);
                        string colour = combo[1];
                        if (x > limits[colour] && !ids.Contains(id))
                        {
                            breached = true;
                        }
                        shows[colour].Add(x);
                    }
                }
                int power = 1;
                foreach (IList<int> values in shows.Values)
                {
                    power *= values.Max();
                }
                powers.Add(power);

                if (!breached)
                {
                    ids.Add(id);
                }
            }


            return (ids.Sum(), powers.Sum());
        }
    }
}