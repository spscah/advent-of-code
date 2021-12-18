using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;

namespace SnailfishNumbers.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 18;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST);


            Pair sum = new Pair(data[0], null);
            foreach(string d in data.Skip(1)) {
                Pair p2 = new Pair(d, null);
                Pair p = new Pair(sum, p2);
                sum = Process(p);
            }
            Console.WriteLine($"{sum.Magnitude}");

            IList<int> scores = new List<int>();
            for(int i = 0 ; i < data.Count; ++i) { 
                for(int j = 0 ; j < data.Count; ++j) { 
                    Pair a = new Pair(data[i], null);
                    Pair b = new Pair(data[j], null);
                    Pair c = new Pair(a,b);
                    scores.Add(Process(c).Magnitude);

                }

            }
            Console.WriteLine(scores.Max());

        }
        static Pair Process(Pair p) {

                bool goagain;
                do {
                    goagain = false;
                    IList<Number> iot = p.InOrderTraversal().ToList();
                    if(p.Explode(iot)) 
                        goagain = true;
                    if(!goagain) {
                        // if there was a number to split, return true - there might be another one 
                        Pair e = p.Split(out goagain);
                        // if e is null, it means the split needs priority exploding 
                        if(e != null) {
                            // Console.WriteLine(p);
                            iot = p.InOrderTraversal().ToList();
                            e.Explode(iot, true);
                        }
                    }
                    // Console.WriteLine(p);
                } while(goagain);
                return p;

        }
    }
    

    abstract class Node {         
        protected Pair _parent;
        protected int Level => _parent == null ? 1 : _parent.Level+1;

        public abstract int Magnitude {get;}
        protected Node(Pair p) {
            _parent = p; 
        }

        public abstract bool Explode(IList<Number> iot, bool force = false);

        protected void NewParent(Pair p) {
            _parent = p;
        }

    }

    class Number : Node, IEquatable<Number> { 
        readonly int _value;

        public int Value => _value;

        public override int Magnitude => _value;

        public Pair Parent => (Pair)_parent;

        public Number (string v, Pair p) : base(p) {
            _value = Int32.Parse(v);
        }

        public Number (int v, Pair p) : base(p) {
            _value = v;
        }
       
        public override bool Explode(IList<Number> iot, bool force = false) {
            return false;
        }

        public override string ToString()
        {
            return _value.ToString();
        }

        public bool Equals(Number other)
        {
            return (other == this);
        }
    }
    enum Direction { Left, Right};

    class Pair : Node { 
        Node _left; 
        Node _right;        

        public override int Magnitude => 3 * _left.Magnitude + 2 * _right.Magnitude;

        public Pair(string s, Pair parent) : base(parent) {
            (string, string) p = MakePair(s);             
            _left = p.Item1[0] == '[' ? new Pair(p.Item1, this) : new Number(p.Item1, this);
            _right = p.Item2[0] == '[' ? new Pair(p.Item2, this) : new Number(p.Item2, this);           
            
        }

        public Pair(Pair p1, Pair p2) : base(null) {
            _left = p1;
            p1.NewParent(this);
            _right = p2;
            p2.NewParent(this);

        }

        public IEnumerable<Number> InOrderTraversal() {
            foreach(Node child in new List<Node> {_left, _right}) {
                if(child != null) {
                    if(child.GetType() == typeof(Number))
                        yield return (Number)child;
                    else
                        foreach(Number n in ((Pair)child).InOrderTraversal())
                            yield return n;
                }
            }
        }

        public Pair Split(out bool result)
        {
            Number splittable = InOrderTraversal().FirstOrDefault(n => n.Value >=10);
            result = false;
            if(splittable == null)
                return null;
            result = true;
            int sv = splittable.Value;
            Pair np = new Pair($"[{sv/2},{sv-(sv/2)}]", splittable.Parent);
            splittable.Parent.Replace(splittable, np);
            if(np.Level > 4)
                return np;
            return null;
        }

        public void Replace(Number oldChild, Pair newChild) {
            if(_left == oldChild)
                _left = newChild;
            if(_right == oldChild)
                _right = newChild;
        }

        public void Replace(Number oldChild, Number newChild) {
            if(_left == oldChild)
                _left = newChild;
            if(_right == oldChild)
                _right = newChild;
        }

        public override bool Explode(IList<Number> iot, bool force = false)
        {
            if(Level < 4) {
                if(_left.Explode(iot) || _right.Explode(iot))
                    return true;
                return false;
            }
            // 5 is the Number, 4 is the Pair - none should be deeper 
                        

            IList<Node> nodes = force ? new List<Node> {this._parent._left, this._parent._right} : new List<Node> {_left, _right};
            foreach(Node child in nodes) {
                if(child.GetType() == typeof(Pair)) {
                    if(force && child != this)
                        continue;
                    Pair ch = (Pair)child;
                    Number c1 = ((Number)ch._left);
                    Number c2 = ((Number)ch._right);

                    int loc1 = iot.IndexOf(c1)-1;
                    if(loc1 >= 0) {
                        int nv = iot[loc1].Value + c1.Value;
                        iot[loc1].Parent.Replace(iot[loc1], new Number(nv, iot[loc1].Parent));
                    }
                    int loc2 = iot.IndexOf(c2)+1;
                    if(loc2 < iot.Count) {
                        int nv = iot[loc2].Value + c2.Value;
                        iot[loc2].Parent.Replace(iot[loc2], new Number(nv, iot[loc2].Parent));
                    }
                    if(!force) {
                        if(child == _left)
                            _left = new Number(0, this);
                        else 
                            _right = new Number(0, this);
                    } else {
                        if(child == this._parent._left)
                            this._parent._left = new Number(0, this._parent);            Console.WriteLine($"{sum.Magnitude}");

                        else 
                            this._parent._right = new Number(0, this._parent);
                    }
                    return true;
                }
            }
            return false;
        }

        (string, string) MakePair(string s) {
            string twohalves = UntilMatchingClosing(s);
            if(twohalves[0] == '[') {
                string first = $"[{UntilMatchingClosing(twohalves)}]";
                string second = twohalves.Substring(first.Length+1);
                return (first,second);
            }
            int i = s.IndexOf(',');
            string a = s.Substring(1,i-1);
            string b = s.Substring(i+1, s.Length-i-2);
            return(a,b);

        }

        string UntilMatchingClosing(string s) {
            // s[0] will be an opening [ 
            int unmatched = 1;
            for(int i = 1; i < s.Length; ++i) {
                switch(s[i]) {
                    case '[':
                        ++unmatched;
                        break;
                    case ']':
                        --unmatched;
                        if(unmatched == 0)
                            return s.Substring(1,i-1);
                        break;
                }
            }
            throw new Exception("that wasn't expected");
        }

        public override string ToString()
        {
            string a = Level > 4 ? "*" : "";
            return $"[{a}{_left.ToString()},{_right.ToString()}{a}]"; // ({Level})
        }
    }
}