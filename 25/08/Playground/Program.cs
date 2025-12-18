using System.Diagnostics;
using AdventOfCode.Lib;

IList<string> test = CommonFunctions.AsListOfStrings(true);
IList<string> today = CommonFunctions.AsListOfStrings(false);

Debug.Assert(Part1(test, 10) == 40);
Console.WriteLine(Part1(today, 1000));
Debug.Assert(Part2(test) == 25272);
Console.WriteLine(Part2(today));
int Part1(IList<string> data, int n)
{
    IList<(int,int,int)> triples = data.Select(s => s.Split(',')).Select(s => (int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]))).ToList();
    IList<((int, int,int), (int, int,int), double)> distances =[];
    for(int a = 0; a < triples.Count; ++a) 
        for(int b = a+1; b < triples.Count; ++b)
            distances.Add((triples[a], triples[b], Distance(triples[a], triples[b])));
    distances = distances.OrderBy(t => t.Item3).ToList();

    IList<HashSet<(int,int,int)>> circuits = [];

    for(int t = 0; t < n; ++t)
    {
        int? c1 = null;
        int? c2 = null;

        for(int c = 0; c < circuits.Count; ++c)
        {
            
            if(circuits[c].Contains(distances[t].Item1))
                c1 = c;
            if(circuits[c].Contains(distances[t].Item2))
                c2 = c;
        }
        if(c1.HasValue)
            circuits[c1.Value].Add(distances[t].Item2);
        if(c2.HasValue)
            circuits[c2.Value].Add(distances[t].Item1);

        if(c1.HasValue && c2.HasValue && c1.Value != c2.Value)
        {
            HashSet<(int,int,int)> combined = new HashSet<(int, int, int)> (circuits[c1.Value].Union(circuits[c2.Value]));
            circuits.RemoveAt(c1.Value > c2.Value ? c1.Value : c2.Value);
            circuits.RemoveAt(c1.Value > c2.Value ? c2.Value : c1.Value);
            

            circuits.Add(combined);
        }
        if (!c1.HasValue && !c2.HasValue)
            circuits.Add(new HashSet<(int, int, int)> { distances[t].Item1, distances[t].Item2 }); 
    }

    int firstn = circuits.Select(c => c.Count).OrderByDescending(a => a).Take(3).Aggregate(1, (a,b)=> a*b);
    return firstn;
}

double Distance((int, int,int) a, (int, int, int) b)
{
    return Math.Sqrt(Math.Pow(a.Item1-b.Item1,2)+Math.Pow(a.Item2-b.Item2,2)+Math.Pow(a.Item3-b.Item3,2));
}

int Part2(IList<string> data)
{
    IList<(int,int,int)> triples = data.Select(s => s.Split(',')).Select(s => (int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]))).ToList();
    IList<((int, int,int), (int, int,int), double)> distances =[];
    for(int a = 0; a < triples.Count; ++a) 
        for(int b = a+1; b < triples.Count; ++b)
            distances.Add((triples[a], triples[b], Distance(triples[a], triples[b])));
    distances = distances.OrderBy(t => t.Item3).ToList();

    IList<HashSet<(int,int,int)>> circuits = [];
    HashSet<(int,int,int)> considered = [];
    
    for(int t = 0; ; ++t)
    {
        int? c1 = null;
        int? c2 = null;

        for(int c = 0; c < circuits.Count; ++c)
        {
            
            if(circuits[c].Contains(distances[t].Item1))
                c1 = c;
            if(circuits[c].Contains(distances[t].Item2))
                c2 = c;
        }
        if(c1.HasValue) {
            circuits[c1.Value].Add(distances[t].Item2);
            considered.Add(distances[t].Item2);
        }
        if(c2.HasValue) {
            circuits[c2.Value].Add(distances[t].Item1);
            considered.Add(distances[t].Item1);
        }
        if(circuits.Count == 1 && circuits[0].Count == triples.Count)
        {
            return distances[t].Item1.Item1 * distances[t].Item2.Item1; 
        }

        if(c1.HasValue && c2.HasValue && c1.Value != c2.Value)
        {
            HashSet<(int,int,int)> combined = new HashSet<(int, int, int)> (circuits[c1.Value].Union(circuits[c2.Value]));
            circuits.RemoveAt(c1.Value > c2.Value ? c1.Value : c2.Value);
            circuits.RemoveAt(c1.Value > c2.Value ? c2.Value : c1.Value);
            

            circuits.Add(combined);
        }
        if (!c1.HasValue && !c2.HasValue)
            circuits.Add(new HashSet<(int, int, int)> { distances[t].Item1, distances[t].Item2 }); 
    }

    int firstn = circuits.Select(c => c.Count).OrderByDescending(a => a).Take(3).Aggregate(1, (a,b)=> a*b);
    return firstn;
}
