using System.Diagnostics;
using AdventOfCode.Lib;
using Microsoft.VisualBasic;

IList<string> test = CommonFunctions.AsListOfStrings(true);
IList<string> today = CommonFunctions.AsListOfStrings(false);

Debug.Assert(Part1(test) == 3);
Console.WriteLine(Part1(today));
Debug.Assert(Part2(test) == 14);
Console.WriteLine(Part2(today));

(ulong,ulong) Parse(string s)
{
    int dash = s.IndexOf('-');
    ulong a = ulong.Parse(s.Substring(0,dash));
    ulong b = ulong.Parse(s.Substring(dash+1));
    return (a,b);
}

int Part1(IList<string> input)
{
    IList<(ulong,ulong)> ranges = input
        .Where(i => i.Contains("-"))
        .Select(Parse)
        .ToList();
    int count = 0;
    foreach(ulong ingredient in input.Where(i => i.Length > 0).Where(i => !i.Contains("-")).Select(i => ulong.Parse(i)))
    {
        foreach(var r in ranges)
        {
            if(ingredient >= r.Item1 && ingredient <= r.Item2){
                ++count;
                break;
            }

        }
    }
    return count;
}

(ulong, ulong)? Overlap((ulong lower,ulong upper) first, (ulong lower,ulong upper) second)
{

    if(first.lower >= second.lower && first.lower <= second.upper) 
        return (second.lower, second.upper > first.upper ? second.upper:first.upper);
    if(second.lower >= first.lower && second.lower <= first.upper) 
        return (first.lower, first.upper > second.upper ? first.upper:second.upper);
    return null;
}

ulong Part2(IList<string> input)
{
    IList<(ulong,ulong)> ranges = input
        .Where(i => i.Contains("-"))
        .Select(Parse)
        .ToList();

    ulong count = 0;
    while(ranges.Count > 1)
    {
        bool widened = false;
        for(int i = 1; i < ranges.Count; ++i)
        {
            (ulong, ulong)? overlap = Overlap(ranges[0], ranges[i]);
            if(overlap.HasValue)
            {
                ranges[0] = overlap.Value;
                ranges.RemoveAt(i);
                widened = true;
                break;
            }            
            
        }
        if(!widened)
        {
            count += ranges[0].Item2 - ranges[0].Item1 + 1;
            ranges.RemoveAt(0);
        }
    }
    count += ranges[0].Item2 - ranges[0].Item1 + 1;
    return count;
}