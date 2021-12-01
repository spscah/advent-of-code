using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;


namespace SonarSweep.App
{
    class Program
    {
        static string GetTodaysData(int day)
        {
            string filename = "today.txt";
            if (!File.Exists(filename))
            {
                Uri uri = new Uri($"https://adventofcode.com/2021/day/{day}/input");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.CookieContainer = new CookieContainer();
                var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("session-cookie.json"));
                string sessionCookie = json["session"];
                request.CookieContainer.Add(new Cookie("session", sessionCookie, "/", uri.Host));
                WebResponse response = request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream);
                string download = readStream.ReadToEnd();
                File.WriteAllText(filename, download);
                return download;
            }
            else
            {
                return File.ReadAllText(filename);
            }          

        }
        static void Main(string[] args)
        {
            string data = GetTodaysData(1).TrimEnd();
            IList<int> numbers = data.Split('\n').Select(i => Convert.ToInt32(i)).ToList();
            int count = 0;
            int previous = numbers[0] + 1;
            foreach (int n in numbers) {
                if (n > previous)
                    count += 1;
                previous = n;
            }
            Console.WriteLine(count);

            count = 0;
            previous = numbers.Take(3).Sum() + 1;
            int a = 0;
            int b = numbers[0];
            int c = numbers[1];
            int current = a + b;
            foreach (int n in numbers.Skip(2)) {
                current -= a;
                a = b;
                b = c;
                c = n;
                current += c;
                if (current > previous)
                    count += 1;
                previous = current;
            }
            Console.WriteLine(count);
        }
    }
}
