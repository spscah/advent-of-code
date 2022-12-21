using AdventOfCode.Lib;
using System.Diagnostics;

namespace MonkeyMath.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 21;
            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test) == (152, 301));

            IList<string> real = TODAY.AsListOfStrings(false);
            (long partone, long parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (long partone, long parttwo) Result(IList<string> real)
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
                    nodes.Add(bits[0].Substring(0, 4), new Node(long.Parse(bits[1]), bits[0].StartsWith("humn")));
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

            return (nodes["root"].Value.Value, nodes["root"].RootCalculation);
        }
    }

    class Node
    {
        Node? _left = null;
        Node? _right = null;
        long? _value;
        readonly char? _op;
        readonly bool _human;

        public long? Value
        {
            get
            {
                if (Calculate()) return _value;
                else return null;
            }
        }

        public Node(long value, bool human)
        {
            _op = null;
            _value = value;
            _human = human;
        }

        public Node(Node left, char op, Node right)
        {
            _op = op;
            _left = left;
            _right = right;
            _value = null;
            _human = false;
        }

        public long RootCalculation
        {
            get
            {
                if (_left.HasHuman)
                    return _left.Calculation(_right.Value.Value);
                else
                    return _right.Calculation(_left.Value.Value);
            }

        }

        long Calculation(long target)
        {
            if (_left == null || _right == null)
                return target;

            if (_left.HasHuman)
            {
                switch (_op)
                {
                    case '+': return _left.Calculation(target - _right.Value.Value);
                    case '-': return _left.Calculation(_right.Value.Value + target);
                    case '*': return _left.Calculation(target / _right.Value.Value);
                    case '/': return _left.Calculation(_right.Value.Value * target);
                }
            }
            else
            {
                switch (_op)
                {
                    case '+': return _right.Calculation(target - _left.Value.Value);
                    case '-': return _right.Calculation(_left.Value.Value - target);
                    case '*': return _right.Calculation(target / _left.Value.Value);
                    case '/': return _right.Calculation(_left.Value.Value / target);
                }
            }
            throw new Exception("Shouldn't get here");
        }

        public bool HasHuman
        {
            get
            {
                if (_human) return true;
                if (_left != null && _left.HasHuman) return true;
                if (_right != null && _right.HasHuman) return true;
                return false;
            }
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

        public override string ToString()
        {
            if (_op.HasValue)
            {
                string left = _left.HasHuman ? "H" : _left.Value.ToString();
                string right = _right.HasHuman ? "H" : _right.Value.ToString();
                return $"({left} {_op} {right})";
            }
            else
                return _value.ToString();
        }
    }
}