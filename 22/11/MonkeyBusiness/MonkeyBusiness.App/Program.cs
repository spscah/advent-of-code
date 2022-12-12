using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace MonkeyBusiness.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 11;
            const bool TEST = true;
            IList<string> data = TODAY.AsListOfStrings(TEST);

            (ulong partone, ulong parttwo) test = Results(data);
            Debug.Assert(test.partone == 10605);
            Debug.Assert(test.parttwo == 2713310158);


            data = TODAY.AsListOfStrings(false);

            (ulong partone, ulong parttwo) real = Results(data);
            Console.WriteLine($"Part one: {real.partone}");
            Console.WriteLine($"Part two: {real.parttwo}");

        }

        static (ulong, ulong) Results(IList<string> data)
        {
            return (Result(data, true), Result(data, false));
        }


        static ulong Result(IList<string> data, bool partone)
        {

            IList<Monkey> monkeys;
            if (partone)
            {
                monkeys = GiveMeMonkeys(data);
                ulong result = 0;
                for (int round = 0; round < 20; ++round)
                {
                    foreach (var monkey in monkeys)
                    {
                        IList<(ulong worry, int result)> updates = monkey.Operations().ToList();
                        foreach ((ulong worry, int m) in updates)
                        {
                            monkeys[m].Add(worry);
                        }
                    }


                    Console.WriteLine($"Round {round + 1}");
                    foreach (var monkey in monkeys)
                    {
                        Console.WriteLine(monkey);
                    }
                    Console.WriteLine();
                    result = (ulong)monkeys.Select(m => m.Inspected).OrderByDescending(i => i).Take(2).Aggregate((ulong)1, (x, y) => (ulong)x * (ulong)y);
                }

                return result;
            }
            // part two 
            Monkey._mitigation = 0;
            monkeys = GiveMeMonkeys(data);
            Monkey._parttwo = (ulong)monkeys.Aggregate((ulong)1, (x, y) => (ulong)x * (ulong)y.TestFactor);
            ++Monkey._mitigation;
            for (int round = 0; round < 10000; ++round)
            {
                foreach (var monkey in monkeys)
                {
                    IList<(ulong worry, int result)> updates = monkey.Operations().ToList();
                    foreach ((ulong worry, int m) in updates)
                    {
                        monkeys[m].Add(worry);
                    }
                }

                if ((round + 1) == 1 || (round + 1) == 20 || (round + 1) % 1000 == 0)
                {
                    Console.WriteLine($"Mitigation: {Monkey._mitigation}{Environment.NewLine}Round {round + 1}");
                    foreach (var monkey in monkeys)
                    {
                        Console.WriteLine(monkey);
                    }
                    Console.WriteLine();

                }
            }
            return (ulong)monkeys.Select(m => m.Inspected).OrderByDescending(i => i).Take(2).Aggregate((ulong)1, (x, y) => (ulong)x * (ulong)y);

        }
        static IList<Monkey> GiveMeMonkeys(IList<string> data)
        {
            int index = 0;
            List<Monkey> monkeys = new();
            while (index < data.Count)
            {
                //int monkey = data[index][7]-'0';      
                ++index;
                IEnumerable<int> items = data[index].Split(':')[1].Split(',').Select(i => int.Parse(i));
                ++index;
                IList<string> values = data[index].Split(' ').ToList();
                string operation = values[values.Count - 2];
                string value = values[values.Count - 1];
                ulong operand = value == "old" ? 0 : ulong.Parse(value);
                ++index;
                ulong test = ulong.Parse(data[index].Split(' ').Last());
                ++index;
                int ifTrue = int.Parse(data[index].Split(' ').Last());
                ++index;
                int ifFalse = int.Parse(data[index].Split(' ').Last());
                ++index;

                ++index; // for the blank line 

                Monkey monkey = new Monkey(items, operation, operand, test, ifTrue, ifFalse);
                monkeys.Add(monkey);
            }
            return monkeys;
        }
    }

    class Monkey
    {
        static public ulong _mitigation = 3;
        static public ulong _parttwo = 0;
        readonly IList<ulong> _items = new List<ulong>();
        readonly ulong _worryFactor;
        readonly ulong _test; 
        public ulong TestFactor => _test;
        readonly string _operation;
        int _inspected = 0;
        readonly int _ifTrue, _ifFalse;

        internal int Inspected => _inspected;

        internal Monkey(IEnumerable<int> items, string operation, ulong worryFactor, ulong test, int t, int f)
        {
            _items = items.Select(i => (ulong)i).ToList();
            _worryFactor = worryFactor;
            _test = test;
            _ifFalse = f;
            _ifTrue = t;
            _operation = operation;
        }

        internal IList<(ulong, int)> Operations()
        {
            IList<(ulong, int)>
                result = _items.Select(i => Operate(i))
                        .Select(worry => worry % _test == 0 ? (worry, _ifTrue) : (worry, _ifFalse)).ToList();
            _inspected += _items.Count;
            _items.Clear();
            return result;
        }

        private ulong Operate(ulong i)
        {
            ulong result = 0;
            switch (_operation[0])
            {
                case ('*'):
                    result = (i * (_worryFactor == 0 ? i : _worryFactor) / _mitigation);
                    break;
                case '+':
                    result = ((i + (_worryFactor == 0 ? i : _worryFactor)) / _mitigation);
                    break;
                default:
                    throw new NotImplementedException();
            }
            if (_parttwo > 0)
                return result % _parttwo;
            else
                return result;
        }

        internal void Add(ulong worry)
        {
            _items.Add(worry);
        }

        public override string ToString()
        {
            return $"Monkey [{_inspected}] {string.Join(',', _items)}";
        }
    }
}