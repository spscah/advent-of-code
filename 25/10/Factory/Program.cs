using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Lib;

IList<string> test = CommonFunctions.AsListOfStrings(true);
IList<string> today = CommonFunctions.AsListOfStrings(false);

Debug.Assert(Part1(test) == 7);
Console.WriteLine(Part1(today));
//Debug.Assert(Part2(test) == 25272);
//Console.WriteLine(Part2(today));
int Part1(IList<string> data)
{
    int rv = 0;
    foreach(string line in data) {
        string pattern = @"\[[^\]]+\]|\(\d+(?:,\d+)*\)|\{[^\}]+\}";
        
        MatchCollection matches = Regex.Matches(line, pattern);
        if (matches.Count == 0)
            return 0;
        
        string target = Contents(matches[0].Groups[0].Value);

        IList<IList<int>> presses = [];        
        foreach(Match m in matches.Skip(1).Take(matches.Count-2)) {
            IList<int> csv = Contents(m.Groups[0].Value).Split(',').Select(int.Parse).ToList();
            presses.Add(csv);
        }

        Queue<(int, string)> q = new Queue<(int, string)>();
        q.Enqueue((0,new String('.',target.Length)));
        while(q.Count > 0)
        {
            (int m, string p) = q.Dequeue();
            if(p == target){
                rv += m;
                break;
            }
            foreach(var pr in presses)
            {
                q.Enqueue((m+1, Press(p, pr)));
            }
        }        
    }  
    return rv;
} 

string Contents(string incoming) {
    return incoming.Substring(1).Remove(incoming.Length-2);
}


string Press(string incoming, IList<int> buttons) {
    StringBuilder sb = new StringBuilder();
    for(int i = 0; i < incoming.Length; ++i) {
        if(buttons.Contains(i)) 
            sb.Append(incoming[i] == '#' ? "." : "#");
        else 
            sb.Append(incoming[i].ToString());
    }
    return sb.ToString();
} 