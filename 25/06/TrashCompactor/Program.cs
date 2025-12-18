using System.ComponentModel.Design;
using System.Diagnostics;
using AdventOfCode.Lib;

IList<string> test = CommonFunctions.AsListOfStrings(true);
IList<string> today = CommonFunctions.AsListOfStrings(false);

Debug.Assert(Part1(test) == 4277556);
Console.WriteLine($"Part 1: {Part1(today)}");

IList<string> test2 = CommonFunctions.AsListOfUntrimmedStrings(true);
IList<string> today2 = CommonFunctions.AsListOfUntrimmedStrings(false);

Debug.Assert(Part2(test2) == 3263827);
Console.WriteLine($"Part 2: {Part2(today2)}");


long Part1(IList<string> input)
{
    IList<string> operators = input.Last().Split(' ').Where(s => s.Length > 0).Select(s => s.Trim()).ToList();
    List<List<int>> values = input
        .SkipLast(1)
        .Select(line =>
            line
                .Split(' ')
                .Where(s => s.Length > 0)
                .Select(s => int.Parse(s.Trim()))
                .ToList()
        ).ToList();
    long result = 0;
    for (int i = 0; i < operators.Count; ++i)
    {
        IList<long> numbers = values.Select(v => (long)v[i]).ToList();
        result += operators[i] switch
        {
            "+" => numbers.Sum(),
            "*" => numbers.Aggregate(1L, (a, b) => a * b),
            _ => throw new Exception($"Unknown operator {operators[i]}")
        };
    }
    return result;
    // 582201448 is too low 

}

long Part2(IList<string> input)
{
    string operators = input.Last();
    List<string> values = input.SkipLast(1).ToList();
    long rv = 0;
    long local = 0;
    string op = null;
    for (int i = 0; i < values[0].Length; ++i)
    {
        if (operators[i] != ' ')
        {
            op = operators[i].ToString();
            rv += local;
            local = op switch
            {
                "+" => 0,
                "*" => 1,
                _ => throw new Exception($"Unknown operator {operators[i]}")
            };
        }
        string column = values.Select(v => v[i]).Aggregate("", (a, b) => a + b).Trim().ToString();
        if (column.Length == 0) continue;
        int columnValue = int.Parse(column);
        local = op switch
        {
            "+" => local + columnValue,
            "*" => local * columnValue,
            _ => throw new Exception($"Unknown operator {operators[i]}")
        };
    }
    return rv + local;
    // 1301380177 is too low
}