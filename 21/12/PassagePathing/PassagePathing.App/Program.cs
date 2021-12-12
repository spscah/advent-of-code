using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;

namespace PassagePathing.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 12;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST);

            IList<(string,string)> connections = data.Select(d => d.Split('-')).Select(p => (p[0], p[1])).ToList();
            Queue<IList<string>> incomplete = new Queue<IList<string>> ();
            incomplete.Enqueue(new List<string> {"start"});
            int cPaths = 0;
            while(incomplete.Count > 0) {
                IList<string> path = incomplete.Dequeue();
                string endpoint = path.Last();
                foreach((string, string) candidate in connections.Where(c => c.Item1 == endpoint || c.Item2 == endpoint).ToList()) {
                    if(candidate.Item1 == "end" || candidate.Item2 == "end") {
                        if(path.Skip(1).Any(c => c.ToLower() == c)) // needs to visit a small cave 
                            ++cPaths;
                        continue;
                    }
                    if(candidate.Item1 == endpoint) {
                        if(candidate.Item2.ToUpper() == candidate.Item2 || 
                            (candidate.Item2.ToLower() == candidate.Item2 && !path.Contains(candidate.Item2))) {
                            incomplete.Enqueue(path.Append(candidate.Item2).ToList());
                            }
                    }
                    if(candidate.Item2 == endpoint && candidate.Item1 != "start") {
                        if(candidate.Item1.ToUpper() == candidate.Item1 || 
                            (candidate.Item1.ToLower() == candidate.Item1 && !path.Contains(candidate.Item1))) {
                            incomplete.Enqueue(path.Append(candidate.Item1).ToList());
                        }
                    }
                }              
            }
            Console.WriteLine(cPaths);

            Queue<IList<string>> q = new Queue<IList<string>> ();
            q.Enqueue(new List<string> {"start"});
            cPaths = 0;
            while(q.Count > 0) {
                IList<string> path = q.Dequeue();
                string endpoint = path.Last();
                foreach((string, string) candidate in connections.Where(c => c.Item1 == endpoint || c.Item2 == endpoint).ToList()) {
                    if(candidate.Item1 == "end" || candidate.Item2 == "end") {
                        ++cPaths;
                        continue;
                    }
                    if(candidate.Item1 == endpoint) {
                        if(candidate.Item2.ToUpper() == candidate.Item2) 
                            q.Enqueue(path.Append(candidate.Item2).ToList());

                        if(candidate.Item2.ToLower() == candidate.Item2 && candidate.Item2 != "start") { //
                            if(!path.Contains(candidate.Item2) || path.Where(p => p.ToLower() == p).Select(p => path.Count(x => p == x)).Count(c => c > 1) < 1)
                                q.Enqueue(path.Append(candidate.Item2).ToList());
                            }
                    }
                    if(candidate.Item2 == endpoint && candidate.Item1 != "start") {
                        if(candidate.Item1.ToUpper() == candidate.Item1)  
                            q.Enqueue(path.Append(candidate.Item1).ToList());
                        if(candidate.Item1.ToLower() == candidate.Item1  && candidate.Item1 != "start") { 
                            if(!path.Contains(candidate.Item1) || path.Where(p => p.ToLower() == p).Select(p => path.Count(x => p == x)).Count(c => c > 1) < 1)                                 
                                q.Enqueue(path.Append(candidate.Item1).ToList());
                        }
                    }
                }              
            }
            Console.WriteLine(cPaths);
        }
    }
}