using System.Diagnostics;

//Debug.Assert(Results("test.txt") == (7, 0));
(int p1, int p2) result = Results("today.txt");
Console.WriteLine($"part one: {result.p1}{Environment.NewLine}part two: {result.p2}");

(int, int) Results(string filename)
{
    string data = File.ReadAllText(filename);

    int partone = 0;
    int parttwo = 0;
    for(int i = 0; i < data.Length-4; ++i)
    {
        string bit = data.Substring(i, 4);
        if(bit.ToCharArray().Distinct().Count() == 4) {
            partone = i + 4;
            break;
        }
    }
    for (int i = 0; i < data.Length - 14; ++i)
    {
        string bit = data.Substring(i, 14);
        if (bit.ToCharArray().Distinct().Count() == 14)
        {
            parttwo = i + 14;
            break;
        }
    }
    return (partone, parttwo);
}
