using System.Diagnostics;

Debug.Assert(Results("test.txt") == (24, 93));
(int p1, int p2) result = Results("today.txt");
Console.WriteLine($"part one: {result.p1}{Environment.NewLine}part two: {result.p2}");

(int, int) Results(string filename)
{
    IList<string> data = File.ReadAllLines(filename).Select(l => l.Trim()).ToList();
    return (Result1(data), Result1(data, true));
}

int Result1(IList<string> data, bool parttwo = false) {
    HashSet<(int, int)> rocks = BuildRocks(data);
    (int l, int r, int d) abyss = (rocks.Select(x => x.Item1).Min() - 1, rocks.Select(x => x.Item1).Max() + 1, rocks.Select(x => x.Item2).Max() + 1);
    if (parttwo) {
        abyss = (500 - abyss.d - 2, 500 + abyss.d + 2, rocks.Select(x => x.Item2).Max() + 2);
        // add "infinite" rocks on y + 2; 
        for (int x = abyss.l; x < abyss.r; ++x)
            rocks.Add((x, abyss.d));        
    }
    int cRocks = rocks.Count;
    (int x, int y) sandSource = (500, 0);
    while (true) {
        (int x, int y) next = sandSource;
        while (true) {
            (int x, int y) candidate = NextStep(next, rocks); 
            if (parttwo && candidate == sandSource)
                return rocks.ToHashSet().Count - cRocks + 1; 
            if (candidate == next) {
                rocks.Add(candidate);
                break;
            }
            if (candidate.x <= abyss.l || candidate.x >= abyss.r || candidate.y >= abyss.d)
                return rocks.ToHashSet().Count-cRocks;
            next = candidate;
        }
    }
}

HashSet<(int, int)> BuildRocks(IList<string> data) {
    HashSet<(int, int)> rocks = new HashSet<(int, int)> ();
    foreach (string line in data) {
        List<(int x, int y)> points = new();
        foreach (string pair in line.Split(" -> ")) {            
            var numbers = pair.Split(',').Select(x => int.Parse(x.Trim())).ToList();
            points.Add((numbers[0], numbers[1]));
        }
        for (int i = 0; i < points.Count - 1; ++i) {
            foreach ((int, int) p in GoFrom(points[i], points[i + 1]))
                rocks.Add(p);
            rocks.Add(points[i+1]);
        }        
    }
    return rocks;
}

// function to return the next step 
(int,int) NextStep((int x,int y) current, HashSet<(int x,int y)> used) {
    // straight down
    if (!used.Contains((current.x, current.y + 1)))
        return (current.x, current.y + 1);
    // left 
    if (!used.Contains((current.x-1, current.y + 1)))
        return (current.x-1, current.y + 1);
    // right
    if (!used.Contains((current.x + 1, current.y + 1)))
        return (current.x + 1, current.y + 1);
    return (current.x, current.y);
}

IEnumerable<(int, int)> GoFrom((int x, int y) p1, (int x, int y) p2) { 
    int dx = 0;
    int dy = 0;
    if (p1.x < p2.x) dx = 1;
    if (p1.x == p2.x) dx = 0;
    if (p1.x > p2.x) dx = -1;
    
    if (p1.y < p2.y) dy = 1;
    if (p1.y == p2.y) dy = 0;
    if (p1.y > p2.y) dy = -1;

    for(int x = p1.x, y = p1.y; x != p2.x || y != p2.y; x += dx, y += dy)
        yield return (x, y);
}