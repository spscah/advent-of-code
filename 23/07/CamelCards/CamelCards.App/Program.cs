using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CamelCards.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 7;
            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test) == (6440, 5905));

            IList<string> real = TODAY.AsListOfStrings(false);
            (int partone, int parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (int partone, int parttwo) Result(IList<string> real)
        {
            IList<CamelCard> cards = real.Select(s => new CamelCard(s, false)).OrderBy(s => s).ToList();
            int partone = 0;
            for (int i = 0; i < cards.Count; ++i)
            {
                partone += (i + 1) * cards[i].Bid;

            }

            IList<CamelCard> cards2 = real.Select(s => new CamelCard(s, true)).OrderBy(s => s).ToList();
            int parttwo = 0;
            for (int i = 0; i < cards2.Count; ++i)
            {
                parttwo += (i + 1) * cards2[i].Bid;

            }

            return (partone, parttwo);
        }
    }

    class CamelCard : IComparable<CamelCard>
    {
        string _hand;
        int _bid;
        int _rank;

        public int Bid => _bid;
        bool _joker;


        public CamelCard(string raw, bool joker)
        {
            IList<string> parts = raw.Split(' ').ToList();
            _hand = parts[0];
            _bid = int.Parse(parts[1]);
            _joker = joker;
            SetRank();
        }

        CamelCard(string hand, int bid, bool joker)
        {
            _hand = hand;
            _bid = bid;
            _joker = joker;
            SetRank();
        }

        public int CompareTo(CamelCard? other)
        {
            string order = _joker ? "J23456789TQKA" : "23456789TJQKA";
            if (other == null)
            {
                return 1;
            }
            if (_rank == other._rank)
            {
                for (int i = 0; i < _hand.Length; ++i)
                {
                    if (_hand[i] != other._hand[i])
                    {
                        return order.IndexOf(_hand[i]).CompareTo(order.IndexOf(other._hand[i]));
                    }
                }
            }
            else
            {
                return _rank.CompareTo(other._rank);
            }
            return 0;
        }

        public override string ToString()
        {
            return $"{_hand} {_bid}";
        }

        void SetRank()
        {
            IDictionary<char, int> summary = new Dictionary<char, int>();
            int jokers = 0;
            foreach (char c in _hand)
            {
                if (_joker && c == 'J')
                {
                    ++jokers;
                    continue;
                }

                if (summary.ContainsKey(c))
                {
                    summary[c]++;
                }
                else
                {
                    summary[c] = 1;
                }
            }
            if (_joker)
            {
                if (jokers == 5)
                {
                    _rank = 7;
                }
                else
                {

                    _rank =
                        summary
                            .Keys
                            .Select(k => new CamelCard(_hand.Replace('J', k), _bid, false)._rank)
                            .Max();
                }

            }
            else
            {
                switch (summary.Count)
                {
                    case 1:
                        _rank = 7;
                        break;
                    case 2:
                        _rank = summary.Values.Max() == 4 ? 6 : 5;
                        break;
                    case 3:
                        _rank = summary.Values.Max() == 3 ? 4 : 3;
                        break;
                    case 4:
                        _rank = 2;
                        break;
                    case 5:
                        _rank = 1;
                        break;
                    default:
                        throw new Exception("Invalid hand");
                }
            }

        }
    }
}