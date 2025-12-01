using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace AdventOfCode.Lib
{
    public static class CommonFunctions
    {
        static string GetTodaysData(int day, bool test, string touse = null)
        {
            string filename = test ? "test.txt" : "today.txt";
            if (test && touse != null)
            {
                filename = touse;
            }
            return File.ReadAllText(filename);
        }

        public static IList<string> AsListOfStrings(this int day, bool test = false, bool notrim = false)
        {
            return GetTodaysData(day, test).TrimEnd().Split('\n').Select(l => notrim ? l : l.Trim()).ToList();
        }

        public static IList<int> AsListOfIntegers(this int day, bool test = false)
        {
            return AsListOfStrings(day, test).Select(i => Convert.ToInt32(i)).ToList();
        }

        public static IList<ValueTuple<string, int>> AsStringIntegerPairs(this int day, bool test = false)
        {
            return AsListOfStrings(day, test).Select(i => i.Split(' ')).Select(p => (p[0], Convert.ToInt32(p[1]))).ToList();
        }

        public static IList<(int x, int y, int z)> AsIntegerTriplets(this int day, bool test = false)
        {
            return AsListOfStrings(day, test).Select(i => i.Split(',')).Select(p => (Convert.ToInt32(p[0]), Convert.ToInt32(p[1]), Convert.ToInt32(p[2]))).ToList();
        }

        public static IList<int> CsvToIntegers(this int day, bool test = false)
        {
            return GetTodaysData(day, test).TrimEnd().Split(',').Select(i => Convert.ToInt32(i)).ToList();
        }

        public static IEnumerable<(int, int)> Generate(int a, int b)
        {
            for (int i = 0; i < a; ++i)
                for (int j = 0; j < b; ++j)
                    yield return (i, j);

        }

        public static IEnumerable<(int, int, int)> Generate(int a, int b, int c, int offset = 0)
        {
            for (int i = 0; i < a; ++i)
                for (int j = 0; j < b; ++j)
                    for (int k = 0; k < c; ++k)
                        yield return (i + offset, j + offset, k + offset);

        }


    }
}
