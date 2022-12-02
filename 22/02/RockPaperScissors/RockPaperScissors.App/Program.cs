using System.Diagnostics;

Debug.Assert(Results("test.txt") == (15, 12));
(int p1, int p2) result = Results("data.txt");
Console.WriteLine($"part one: {result.p1}{Environment.NewLine}part two: {result.p2}");

(int, int) Results(string filename)
{
    List<(char them, char me)> data = File.ReadAllLines(filename).Select(s => s.Split(' ').ToList()).Select(p => (p[0][0], p[1][0])).ToList();

    int score = 0, score2 = 0;
    foreach(var item in data)
    {
        int them = 1 + item.them - 'A';
        int me = 1 + item.me - 'X';

        if (me == them) score += 3;
        else if (me > them % 3) score += 6;        
        score += me;

        switch(me) {
            case (2):
                score2 += 3 + them; break;
            case(1):            
                score2 += new List<int> { 3, 1, 2 }[them - 1];
                break;
            case(3):
                score2 += 6 + new List<int> { 2, 3, 1 }[them - 1];
                break;
        }
    }
    return (score, score2);
}