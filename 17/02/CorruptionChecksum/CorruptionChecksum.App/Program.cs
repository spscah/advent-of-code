// See https://aka.ms/new-console-template for more information

using System.Diagnostics;

string data = File.ReadAllText("data.txt");

string testdata = "5\t1\t9\t5\n7\t5\t3\n2\t4\t6\t8";

Debug.Assert(PartOne(testdata) == 18);

Console.WriteLine($"part one {PartOne(data)}");

testdata = "5\t9\t2\t8\n9\t4\t7\t3\n3\t8\t6\t5";

Debug.Assert(PartTwo(testdata) == 9);

Console.WriteLine($"part one {PartTwo(data)}");

Console.WriteLine($"done.{Environment.NewLine}<press any key>");

static int PartOne(string data)
{
    IList<string> rows = data.Split('\n');
    int result = 0;
    foreach (string row in rows) {
        IList<int> values = row.Split('\t').Select(i => System.Convert.ToInt32(i)).ToList();
        result += values.Max() - values.Min();
    }
    return result;
}

static int PartTwo(string data)
{
    IList<string> rows = data.Split('\n');
    int result = 0;
    foreach (string row in rows)
    {
        IList<int> values = row.Split('\t').Select(i => System.Convert.ToInt32(i)).ToList();
        foreach (int value in values)
        {
            IList<int> candidates = values.Where(v => value != v && value % v == 0).ToList();
            if (candidates.Count > 0)
            {
                result += candidates[0];
                break;
            }
        }
    }
    return result;
}
