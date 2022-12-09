using System.Diagnostics;

Debug.Assert(Results("test.txt") == (13, 0));
(int p1, int p2) result = Results("today.txt");
Console.WriteLine($"part one: {result.p1}{Environment.NewLine}part two: {result.p2}");


(int, int) Results(string filename)
{
    IList<string> data = File.ReadAllLines(filename).Select(l => l.Trim()).ToList();
    return (Result(data, true), 0); //  Result(data, false));
}

int Result(IList<string> data, bool partone)
{
    List<(int x, int y)> headsPath = new() {  (0,0)};
    List<(int x, int y)> tailsPath = new();

    foreach(string line in data)
    {
        int x = 0, y = 0;
        switch(line[0])
        {
            case ('U'):
                y = 1;
                break;
            case ('D'):
                y = -1;
                break;
            case ('R'):
                x = 1;
                break;
            case ('L'):
                x = -1;
                break;
        }
        int repeat = Convert.ToInt32(line.Substring(2));
        for (int i = 0; i < repeat; i++)
        {
            (int x, int y) nhead = (headsPath.Last().x + x, headsPath.Last().y + y);
            headsPath.Add(nhead);

            (int x, int y) ctail = tailsPath.Count == 0 ? headsPath.First() : tailsPath.Last();

            if (!AreNeighbours(nhead, ctail))
            {
                if (nhead.y == ctail.y + 2)
                    tailsPath.Add((nhead.x, nhead.y - 1));
                else if (nhead.y == ctail.y - 2)
                    tailsPath.Add((nhead.x, nhead.y + 1));
                else if (nhead.x == ctail.x + 2)
                    tailsPath.Add((nhead.x - 1, nhead.y));
                else if (nhead.x == ctail.x - 2)
                    tailsPath.Add((nhead.x + 1, nhead.y));
            }
        }
    }

    return tailsPath.Distinct().Count();
}

bool AreNeighbours((int x, int y) h, (int x, int y) t)
{
    return Math.Abs(h.x-t.x) <= 1 && Math.Abs(h.y-t.y) <= 1;

}
