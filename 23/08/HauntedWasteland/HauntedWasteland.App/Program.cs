using AdventOfCode.Lib;
using System.Diagnostics;

namespace HauntedWasteland.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 8;
            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test) == (6, 6));


            IList<string> real = TODAY.AsListOfStrings(false);
            (int partone, ulong parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (int partone, ulong parttwo) Result(IList<string> real)
        {
            var nodes = new Dictionary<string, Node>();
            string patten = real[0];
            foreach (string line in real.Skip(2))
            {
                string name = line.Split(' ')[0];
                nodes.Add(name, new Node(name));
            }
            foreach (string line in real.Skip(2))
            {
                int bracket = line.IndexOf('(');
                string name = line.Split(' ')[0];
                IList<string> children = line.Substring(bracket + 1, line.IndexOf(')') - bracket - 1).Split(", ").ToList();
                string l = children[0];
                string r = children[1];
                nodes[name].AddLeft(nodes[l]);
                nodes[name].AddRight(nodes[r]);
            }
            Node root = nodes["AAA"];
            int partone = (int)Loop(root, nodes, patten, "ZZZ");

            IList<Node> ghosts = nodes.Values.Where(n => n.Name.EndsWith("A")).ToList();
            IList<ulong> loops = ghosts.Select(g => Loop(g, nodes, patten, "Z")).ToList();

            ulong parttwo = lcm(loops);

            return (partone, parttwo);
        }

        static ulong Loop(Node root, Dictionary<string, Node> nodes, string pattern, string ends)
        {
            ulong result = 0;
            while (!root.Name.EndsWith(ends))
            {
                ulong i = result % (ulong)pattern.Count();
                root = pattern[(int)i] == 'L' ? root.Left : root.Right;
                ++result;
            }
            return result;
        }

        static ulong gcd(ulong n1, ulong n2)
        {
            if (n2 == 0)
            {
                return n1;
            }
            else
            {
                return gcd(n2, n1 % n2);
            }
        }

        public static ulong lcm(IList<ulong> numbers)
        {
            return numbers.Aggregate((S, val) => S * val / gcd(S, val));
        }

    }

    class Node
    {
        string _name;
        Node _right;
        Node _left;

        public string Name => _name;
        public Node Left => _left;
        public Node Right => _right;

        public Node(string name)
        {
            _name = name;
            _left = null;
            _right = null;
        }

        public void AddLeft(Node left)
        {
            _left = left;
        }

        public void AddRight(Node right)
        {
            _right = right;
        }

        public override string ToString()
        {
            string l = _left == null ? "null" : _left.Name;
            string r = _right == null ? "null" : _right.Name;
            return $"{_name} -> {l} {r}";
        }
    }
}