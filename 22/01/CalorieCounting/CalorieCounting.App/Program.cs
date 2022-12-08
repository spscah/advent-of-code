using System.Diagnostics;

Debug.Assert(Results("test.txt") == (24000, 45000));
(int p1, int p2) result = Results("data.txt");
Console.WriteLine($"part one: {result.p1}{Environment.NewLine}part two: {result.p2}");

(int, int) Results(string filename)
{
    List<string> data = File.ReadAllLines(filename).ToList();
    
    IList<int> results = new List<int> { 0, 0, 0};
    int current = 0; 
    foreach(string s in data) {   
        if (string.IsNullOrEmpty(s)) {
            for(int i = 0; i < 3; ++i) {
                if (current > results[i]) {
                    results.Insert(i, current);
                    results = results.Take(3).ToList();
                    break;
                }
            }
            current = 0;
        }
        else 
            current += Convert.ToInt32(s);
    }
    return (results.First(), results.Sum());
}
