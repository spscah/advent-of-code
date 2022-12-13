using System.Diagnostics;

Debug.Assert(Results("test.txt") == (13, 1));
(int p1, int p2) result = Results("today.txt");
Console.WriteLine($"part one: {result.p1}{Environment.NewLine}part two: {result.p2}");


(int, int) Results(string filename)
{
    IList<string> data = File.ReadAllLines(filename).Select(l => l.Trim()).ToList();

    List<(int x, int y)> headsPath = new() {  (0,0)};
    List<(int x, int y)> tailsPath = new();
<<<<<<< Updated upstream
    List<(int x, int y)> chain = new() { headsPath.First(), headsPath.First() };
    List<(int x, int y)> ten = new();
=======
    List<(int x, int y)> tens = new() {  headsPath.First()};
>>>>>>> Stashed changes

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
                tailsPath.Add(MoveTail(nhead, ctail));
            }
<<<<<<< Updated upstream

            if (i == 0)
                chain[chain.Count-1] = nhead;
            else 
                chain.Add(nhead);

            int index = 0;
            while(index +1 < chain.Count)
            {
                if (!AreNeighbours(chain[index], chain[index + 1]))                
                    chain[index + 1] = MoveTail(chain[index], chain[index + 1]);                
                else 
                    break;

                ++index;
            }

            (int, int) tenth = chain.Count < 10 ? chain.First() : chain[chain.Count-10];
            ten = ten.Union(new List<(int,int)>() { tenth }).ToList();
        }
    }

    return (tailsPath.Distinct().Count(), ten.Distinct().Count());
}

(int x, int y) MoveTail((int x, int y) nhead, (int x, int y) ctail)
{
    if (nhead.y == ctail.y + 2)
        return (nhead.x, nhead.y - 1);
    else if (nhead.y == ctail.y - 2)
        return (nhead.x, nhead.y + 1);
    else if (nhead.x == ctail.x + 2)
        return (nhead.x - 1, nhead.y);
    else if (nhead.x == ctail.x - 2)
        return (nhead.x + 1, nhead.y);
    return (0, 0);
=======
            if (tailsPath.Count > 10)
                tens.Add(tailsPath[tailsPath.Count - 10]);
        }
    }

    return (tailsPath.Distinct().Count(), tens.Distinct().Count());
>>>>>>> Stashed changes
}

bool AreNeighbours((int x, int y) h, (int x, int y) t)
{
    return Math.Abs(h.x-t.x) <= 1 && Math.Abs(h.y-t.y) <= 1;

}
