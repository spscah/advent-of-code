using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace AdventOfCode.Lib
{
    public static class CommonFunctions
    {
        static string GetTodaysData(int day)
        {
            string filename = "today.txt";
            if (!File.Exists(filename))
            {
                Uri uri = new Uri($"https://adventofcode.com/2021/day/{day}/input");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.CookieContainer = new CookieContainer();
                Dictionary<string, string> json = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("session-cookie.json"));
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

        public static IList<string> AsListOfStrings(this int day)
        {
            return GetTodaysData(day).TrimEnd().Split('\n');
        }

        public static IList<int> AsListOfIntegers(this int day)
        {
            return AsListOfStrings(day).Select(i => Convert.ToInt32(i)).ToList();
        }

        public static IList<ValueTuple<string,int>> AsStringIntegerPairs(this int day)
        {
            return AsListOfStrings(day).Select(i => i.Split(' ')).Select(p => (p[0], Convert.ToInt32(p[1]))).ToList();
        }
    }
}
