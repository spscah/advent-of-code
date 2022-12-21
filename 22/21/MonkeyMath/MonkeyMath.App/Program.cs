using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace MonkeyMath.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 21;
            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test) == (152, 0));

            IList<string> real = TODAY.AsListOfStrings(false);
            (ulong partone, int parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (ulong partone, int parttwo) Result(IList<string> real)
        {
            Dictionary<string, Node> nodes = new();
            Dictionary<string, (string l, char op, string r)> todo = new();
            Node root = null;

            foreach (string line in real)
            {
                var bits = line.Split(' ').ToList();
                if (bits.Count == 2)
                {
                    // leaf node 
                    nodes.Add(bits[0].Substring(0, 4), new Node(ulong.Parse(bits[1])));
                }
                else
                {
                    // non-leaf node 
                    todo.Add(bits[0].Substring(0, 4), (bits[1], bits[2][0], bits[3]));
                }

            }

            while (todo.Keys.Count > 0)
            {
                foreach (KeyValuePair<string, (string l, char op, string r)> kvp in todo.ToList())
                {
                    if (nodes.ContainsKey(kvp.Value.l) && nodes.ContainsKey(kvp.Value.r))
                    {
                        nodes.Add(kvp.Key, new Node(nodes[kvp.Value.l], kvp.Value.op, nodes[kvp.Value.r]));
                        todo.Remove(kvp.Key);
                    }
                }
            }

            return (nodes["root"].Value.Value, 0);
        }
    }

    class Node
    {
        Node? _left;
        Node? _right;
        ulong? _value;
        readonly char? _op;
        readonly bool root = false;
        readonly bool human = false;

        public ulong? Value
        {
            get
            {
                if (Calculate()) return _value;
                else return null;
            }
        }

        public Node(ulong value)
        {
            _op = null;
            _value = value;
        }

        public Node(Node left, char op, Node right)
        {
            _op = op;
            _left = left;
            _right = right;
            _value = null;
        }

        public bool Calculate()
        {
            if (_value.HasValue) return true;
            if (!_left.Calculate()) return false;
            if (!_right.Calculate()) return false;



            switch (_op)
            {
                case '+':
                    _value = _left.Value + _right.Value;
                    break;
                case '-':
                    _value = _left.Value - _right.Value;
                    break;
                case '*':
                    _value = _left.Value * _right.Value;
                    break;
                case '/':
                    _value = _left.Value / _right.Value;
                    break;

            }
            return true;
        }
    }
}