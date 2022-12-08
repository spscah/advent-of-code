using System.Diagnostics;

Debug.Assert(Results("test.txt") == (21, 8));
(int p1, int p2) result = Results("today.txt");
Console.WriteLine($"part one: {result.p1}{Environment.NewLine}part two: {result.p2}");

(int, int) Results(string filename)
{
    IList<string> data = File.ReadAllLines(filename).Select(l => l.Trim()).ToList();
    return (Result(data, true), Result(data, false));
}

int Result(IList<string> data, bool partone) {
    if (partone) {
        var down = ByColumn(data, false);
        var up = ByColumn(data, true);
        var right = ByRow(data, false);
        var left = ByRow(data, true);
        return down.Union(up).Union(left).Union(right).Distinct().Count();
    } else {
        return Visibility(data);        
    }
}

int Visibility(IList<string> data) {
    int value=-1;

    for (int r = 0; r < data.Count; r++)
    {
        for (int c = 0; c < data[r].Length; c++)
        {
            List<int> directions = new();
            int temp = 0;
            for(int up = r-1; up >= 0; --up)
            {
                ++temp;
                if (data[up][c] >= data[r][c])
                    up = -1;
            }
            directions.Add(temp);

            temp = 0;
            for (int left = c - 1; left >= 0; --left)
            {
                ++temp;
                if (data[r][left] >= data[r][c])
                    left = -1;
            }
            directions.Add(temp);

            temp = 0;
            for (int right = c + 1; right < data[r].Length; ++right)
            {
                ++temp;
                if (data[r][right] >= data[r][c])
                    right = data[r].Length;
            }
            directions.Add(temp);

            temp = 0;
            for (int down = r + 1; down < data.Count; ++down)
            {
                ++temp;
                if (data[down][c] >= data[r][c])
                    down = data.Count;
            }
            directions.Add(temp);

            int score = directions.Aggregate(1, (r, i) => r * i);
            if (score > value)
                value = score;
        }
    }
    return value;
}

IList<(int,int)> ByColumn(IList<string> data, bool reverse)
{
    List<(int, int)> visible = new();
    for (int c = 0; c < data[0].Length; ++c) {
        int from = reverse ? data.Count - 1 : 0;
        int to = reverse ? -1 : data.Count;
        int step = reverse ? -1 : +1;

        char highest = (char)('0' - 1);
        for (int r = from; r != to; r += step) {
            char ch = data[r][c];
            if (ch > highest) {
                visible.Add((r, c));
                highest = ch;
            }
        }
    }
    return visible;
}

IList<(int, int)> ByRow(IList<string> data, bool reverse)
{
    List<(int, int)> visible = new();
    for (int r = 0; r < data.Count ; ++r)
    {
        int from = reverse ? data.Count - 1 : 0;
        int to = reverse ? -1 : data.Count;
        int step = reverse ? -1 : +1;

        char highest = (char)('0' - 1);
        for (int c = from; c != to; c += step)
        {
            char ch = data[r][c];
            if (ch > highest)
            {
                visible.Add((r, c));
                highest = ch;
            }
        }
    }
    return visible;
}

/*
int ByRow(IList<string> data) {
    return data.Select(d => StringTwoWays(d)).Sum();
}

int StringTwoWays(string s)
{
    int value = 0;
    foreach (bool b in new[] { true, false })
    {
        string use = b ? string.Join("", s.Reverse()) : s;
        char highest = (char)('0' - 1);
        foreach (char c in use)
        {
            if (c > highest)
            {
                ++value;
                highest = c;
            }
            else
                break;
        }
    }
    return value;

}
*/