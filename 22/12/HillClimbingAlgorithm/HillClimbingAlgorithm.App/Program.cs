using System.Diagnostics;

Debug.Assert(Results("test.txt") == (31, 29));
(int p1, int p2) result = Results("today.txt");
Console.WriteLine($"part one: {result.p1}{Environment.NewLine}part two: {result.p2}");

(int, int) Results(string filename)
{
    IList<string> data = File.ReadAllLines(filename).Select(l => l.Trim()).ToList();
    return (Result1(data), Result2(data));
}


int Result1(IList<string> data) {
    (int x, int y) start = WhereIs(data, 'S');
    (int x, int y) end = WhereIs(data, 'E');

    IList<string>grid = data.Select(l => l.Replace('S', 'a')).Select(l => l.Replace('E', 'z')).ToList();
    IList<(int,int)> previously = new List<(int,int)> () {  start};

    Queue<(int x, int y, int steps)> queue = new Queue<(int, int, int)> ();
    queue.Enqueue((start.x, start.y, 0));

    while(queue.Count > 0) {
        (int x, int y, int s) next = queue.Dequeue();      
        foreach ((int x, int y) offset in new[] { (0, -1), (0,1), (-1, 0), (1, 0) }) // up down left right 
        {
            (int x, int y, int s)? neighbour = Enqueue(queue, next, offset.x, offset.y, grid);
            if (neighbour.HasValue)
            {
                (int x, int y) candidate = (neighbour.Value.x, neighbour.Value.y);
                if (candidate.x == end.x && candidate.y == end.y) 
                    return neighbour.Value.s;
                if (!previously.Contains(candidate)) {
                    previously.Add(candidate);
                    queue.Enqueue(neighbour.Value);
                }
            }
        }
    }

    throw new Exception("shouldn't get here");
}


int Result2(IList<string> data)
{
    (int x, int y) start = WhereIs(data, 'E');

    IList<string> grid = data.Select(l => l.Replace('S', 'a')).Select(l => l.Replace('E', 'z')).ToList();
    IList<(int, int)> previously = new List<(int, int)>() { start };

    Queue<(int x, int y, int steps)> queue = new Queue<(int, int, int)>();
    queue.Enqueue((start.x, start.y, 0));

    while (queue.Count > 0)
    {
        (int x, int y, int s) next = queue.Dequeue();
        foreach ((int x, int y) offset in new[] { (0, -1), (0, 1), (-1, 0), (1, 0) }) // up down left right 
        {
            (int x, int y, int s)? neighbour = Enqueue(queue, next, offset.x, offset.y, grid, parttwo: true);
            if (neighbour.HasValue)
            {
                (int x, int y) candidate = (neighbour.Value.x, neighbour.Value.y);
                if(grid[candidate.y][candidate.x] == 'a')
                    return neighbour.Value.s;
                if (!previously.Contains(candidate))
                {
                    previously.Add(candidate);
                    queue.Enqueue(neighbour.Value);
                }
            }
        }
    }

    throw new Exception("shouldn't get here");
}



(int,int,int)? Enqueue(Queue<(int,int,int)> q, (int x,int y, int s) me, int x, int y, IList<string> grid, bool parttwo = false)
{
    if ((me.y + y < 0) || (me.y + y >= grid.Count))
        return null;
    if ((me.x + x < 0) || (me.x + x >= grid[0].Length))
        return null;
    if (parttwo)
    {
        if (grid[me.y][me.x] - (grid[me.y + y][me.x + x]) > 1)
            return null;
    }
    else
    {
        if ((grid[me.y + y][me.x + x] - grid[me.y][me.x]) > 1)
            return null;
    }

    return (me.x + x, me.y+y, me.s+1);
}

(int, int) WhereIs(IList<string> data, char v)
{
    for(int r = 0; r < data.Count; r++)
    {
        for(int c = 0; c < data[r].Length; c++)
        {
            if (data[r][c] == v)
                return (c, r);
        }
    }
    throw new ArgumentOutOfRangeException();
}