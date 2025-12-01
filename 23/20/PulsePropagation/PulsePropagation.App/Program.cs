using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace PulsePropagation.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 20;
            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test) == (0, 0));

            IList<string> real = TODAY.AsListOfStrings(false);
            (int partone, int parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (int partone, int parttwo) Result(IList<string> real)
        {
            throw new NotImplementedException();
        }
    }

    class Module
    {
        bool _iIsFlipFlop;
        bool _isInverter;
        string _name;
        IList<string> _outputs;
        bool _state;
        int _low;
        int _high;

        public Module(bool isFlipFlip, bool isInverter, string name, IList<string> outputs)
        {
            _iIsFlipFlop = isFlipFlip;
            _isInverter = isInverter;
            _name = name;
            _outputs = outputs;
            _state = false;
            _low = 0;
            _high = 0;
        }
    }
}