using System.Diagnostics;

Debug.Assert(Results("test.txt") == (7, 19));
(int p1, int p2) result = Results("today.txt");
Console.WriteLine($"part one: {result.p1}{Environment.NewLine}part two: {result.p2}");

int Result(string data, bool partone) {
    int step = partone ? 4 : 14;
    for (int i = 0; i < data.Length - step; ++i) {
        string bit = data.Substring(i, step);
        if (bit.ToCharArray().Distinct().Count() == step)
            return i + step;
    }
    throw new ArgumentOutOfRangeException("Didn't expect to get here");
}

(int, int) Results(string filename) {
    string data = File.ReadAllText(filename);
    return (Result(data, true), Result(data, false));
}
