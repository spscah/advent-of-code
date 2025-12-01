// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using AdventOfCode.Lib;

IList<string> test = CommonFunctions.AsListOfStrings(0, true);

Debug.Assert(Part1(test) == 3);
Debug.Assert(Part2(test) == 6);

IList<string> today = CommonFunctions.AsListOfStrings(0, false);

Console.WriteLine($"Part 1: {Part1(today)}");
Console.WriteLine($"Part 2: {Part2(today)}");

int Part1(IList<string> input)
{
    int result = 0;
    int position = 50;

    foreach (string line in input)
    {
        int direction = (line[0] == 'L') ? -1 : 1;
        int magnitude = int.Parse(line.Substring(1));
        position += direction * magnitude;
        position %= 100;
        if (position == 0) ++result;
    }

    return result;
}

int Part2(IList<string> input)
{
    int result = 0;
    int position = 50;
    foreach (string line in input)
    {
        int direction = (line[0] == 'L') ? -1 : 1;
        int magnitude = int.Parse(line.Substring(1));

        result += magnitude / 100;
        magnitude %= 100;

        (position, bool crossedZero) = Twist(position, direction * magnitude);
        if (position == 0 || crossedZero) ++result;

    }
    return result;
}

(int pos, bool crossed) Twist(int position, int offset)
{
    int result = position;
    bool crossedZero = false;

    result += offset;
    if (result < 0)
    {
        result += 100;
        crossedZero = true;
    }
    if (result >= 100)
    {
        result -= 100;
        crossedZero = true;
    }
    if (position == 0) crossedZero = false;

    return (result, crossedZero);
}
