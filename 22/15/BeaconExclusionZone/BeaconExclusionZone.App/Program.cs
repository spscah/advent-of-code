using AdventOfCode.Lib;
using System.Diagnostics;

namespace BeaconExclusionZone.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 15;

            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test, true) == (26, 56000011L));

            IList<string> real = TODAY.AsListOfStrings(false);
            var result = Result(real, false);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (int partone, long parttwo) Result(IList<string> data, bool test)
        {
            List<(int x, int y, int md)> sensors = new();
            HashSet<(int x, int y)> beacons = new();

            foreach (string line in data)
            {
                // todo: regex would be better
                int x = line.IndexOf("x=");
                int comma = line.IndexOf(",");
                int y = line.IndexOf("y=");
                int colon = line.IndexOf(":");
                (int x, int y) sensor = (int.Parse(line.Substring(x + 2, comma - x - 2)), int.Parse(line.Substring(y + 2, colon - y - 2)));

                x = line.IndexOf("x=", x + 2);
                comma = line.IndexOf(",", comma + 2);
                y = line.IndexOf("y=", y + 2);

                (int x, int y) beacon = (int.Parse(line.Substring(x + 2, comma - x - 2)), int.Parse(line.Substring(y + 2)));
                sensors.Add((sensor.x, sensor.y, ManhattanDistance(sensor, beacon)));
                beacons.Add(beacon);
            }

            int min_x = sensors.Select(s => s.x - s.md).Union(beacons.Select(b => b.x)).Min();
            int max_x = sensors.Select(s => s.x + s.md).Union(beacons.Select(b => b.x)).Max();

            HashSet<(int x, int y)> grid = new();

            int row = test ? 10 : 2000000;

            int partone = 0;
            for (int x = min_x; x <= max_x; ++x)
            {
                foreach ((int x, int y, int md) sensor in sensors)
                    if (ManhattanDistance((x, row), (sensor.x, sensor.y)) <= sensor.md && !beacons.Contains((x, row)))
                    {
                        ++partone;
                        break;
                    }
            }

            int limit = test ? 20 : 4000000;
            for (int y = 0; y < limit; ++y)
            {
                int? nc = NotCovered(sensors.Select(s => Feasible((s.x, s.y), y, s.md)).Where(f => f.HasValue).Select(f => f.Value).ToList(), limit);
                if (nc.HasValue)
                    return (partone, (long)nc.Value * 4000000 + (long)y);
            }
            throw new Exception("shouldn't get here");
        }

        static int ManhattanDistance((int x, int y) a, (int x, int y) b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        static (int f, int t)? Feasible((int x, int y) centre, int y, int md)
        {
            int remainder = md - Math.Abs(centre.y - y);
            return remainder < 0 ? null : (centre.x - remainder, centre.x + remainder);
        }

        static int? NotCovered(IList<(int f, int t)> ranges, int limit)
        {
            int? notcovered = null;
            int last = 0;
            foreach ((int f, int t) range in ranges.OrderBy(r => r.f))
            {
                if (range.f > last)
                {
                    notcovered = last;
                    break;
                }
                last = Math.Max(last, range.t + 1);
            }
            if (last < limit)
                notcovered = last;
            return notcovered;
        }
    }
}