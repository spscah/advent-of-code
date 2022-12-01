using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Amphipod.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 23;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST, true);


            data.Insert(3, "  #D#C#B#A#");
            data.Insert(4, "  #D#B#A#C#");
            Day_23.Part_2(data, false);

       }
    }

}