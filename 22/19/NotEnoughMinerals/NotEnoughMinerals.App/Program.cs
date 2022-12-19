using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace NotEnoughMinerals.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 19;
            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test) == (33, 56 * 62));

            IList<string> real = TODAY.AsListOfStrings(false);
            (int partone, int parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (int partone, int parttwo) Result(IList<string> data)
        {
            List<Quad> stash = new();
            List<Quad> bots = new();
            List<Blueprint> blueprints = new();

            string pattern = @"Blueprint (\d+):.*(\d+) ore\..*(\d+) ore\..*(\d+) ore and (\d+) clay\..* (\d+) ore and (\d+) obsidian\.";
            foreach (string line in data)
            {
                var matches = Regex.Matches(line, pattern);
                Debug.Assert(matches.Count == 1);
                Debug.Assert(matches[0].Groups.Count == 8);
                int bp = int.Parse(matches[0].Groups[1].Value);
                Quad ore = new Quad(int.Parse(matches[0].Groups[2].Value), 0, 0, 0);
                Quad clay = new Quad(int.Parse(matches[0].Groups[3].Value), 0, 0, 0);
                Quad obsidian = new Quad(int.Parse(matches[0].Groups[4].Value), int.Parse(matches[0].Groups[5].Value), 0, 0);
                Quad geode = new Quad(int.Parse(matches[0].Groups[6].Value), 0, int.Parse(matches[0].Groups[7].Value), 0);

                blueprints.Add(new Blueprint(bp, (ore, clay, obsidian, geode)));
            }

            int partone = 0;
            State.Limit = 24;
            foreach (Blueprint bp in blueprints)
            {
                int best = bp.Id * bp.GeodeHunter();
                partone += best;
            }

            int parttwo = 1;
            State.Limit = 32;
            int counter = 0;
            foreach (Blueprint bp in blueprints)
            {
                int best = bp.GeodeHunter();
                partone *= best;
                ++counter;
                if (counter == 3)
                {
                    parttwo *= best;
                    break;
                }
            }

            return (partone, parttwo);
        }

    }

    class Quad : ICloneable
    {
        readonly int _ore;
        readonly int _clay;
        readonly int _obsidian;
        readonly int _geode;

        public int Ore => _ore;
        public int Clay => _clay;
        public int Obsidian => _obsidian;
        public int Geode => _geode;

        public Quad(int ore, int clay, int obsidian, int geode)
        {
            _ore = ore;
            _clay = clay;
            _obsidian = obsidian;
            _geode = geode;
        }

        public static Quad operator +(Quad a, Quad b) => new Quad(a.Ore + b.Ore, a.Clay + b.Clay, a.Obsidian + b.Obsidian, a.Geode + b.Geode);
        public static Quad operator -(Quad a, Quad b) => new Quad(a.Ore - b.Ore, a.Clay - b.Clay, a.Obsidian - b.Obsidian, a.Geode - b.Geode);
        public static Quad operator *(Quad a, int b) => new Quad(a.Ore * b, a.Clay * b, a.Obsidian * b, a.Geode * b);
        public static bool operator >=(Quad a, Quad b) => a.Ore >= b.Ore && a.Clay >= b.Clay && a.Obsidian >= b.Obsidian && a.Geode >= b.Geode;
        public static bool operator <=(Quad a, Quad b) => a.Ore <= b.Ore && a.Clay <= b.Clay && a.Obsidian <= b.Obsidian && a.Geode <= b.Geode;
        public static bool operator >(Quad a, Quad b) => !(a <= b);
        public static bool operator <(Quad a, Quad b) => a.Ore < b.Ore && a.Clay < b.Clay && a.Obsidian < b.Obsidian && a.Geode < b.Geode;
        public static bool operator ==(Quad a, Quad b) => a.Ore == b.Ore && a.Clay == b.Clay && a.Obsidian == b.Obsidian && a.Geode == b.Geode;
        public static bool operator !=(Quad a, Quad b) => !(a == b);

        public override string ToString() => $"({Ore}, {Clay}, {Obsidian}, {Geode})";

        public object Clone()
        {
            return new Quad(Ore, Clay, Obsidian, Geode);
        }
    }

    class Blueprint
    {
        readonly int _id;
        readonly Quad _oreCost;
        readonly Quad _clayCost;
        readonly Quad _obsidianCost;
        readonly Quad _geodeCost;
        readonly int _orelimit;

        public int Id => _id;
        readonly Quad _limits;
        public int OreBotLimit => _limits.Ore;
        public int OreStashLimit => 7;

        public Queue<State> queue = new();
        HashSet<State> visited = new();


        public Blueprint(int id, (Quad, Quad, Quad, Quad) cost)
        {
            _id = id;
            (_oreCost, _clayCost, _obsidianCost, _geodeCost) = cost;
            _limits = new Quad(
                new List<int> { _oreCost.Ore, _clayCost.Ore, _obsidianCost.Ore, _geodeCost.Ore }.OrderByDescending(c => c).Take(2).Sum() + 1,
                new List<int> { _oreCost.Clay, _clayCost.Clay, _obsidianCost.Clay, _geodeCost.Clay }.OrderByDescending(c => c).Take(2).Sum() + 1,
                new List<int> { _oreCost.Obsidian, _clayCost.Clay, _obsidianCost.Obsidian, _geodeCost.Obsidian }.OrderByDescending(c => c).Take(2).Sum() + 1,
                0);

            queue.Enqueue(new State(this, 0, new Quad(0, 0, 0, 0), new Quad(1, 0, 0, 0)));

        }

        public IEnumerable<(Quad cost, Quad benefit)> Options(Quad stash, Quad bots)
        {
            if (stash.Ore >= _oreCost.Ore) // && bots.Ore <= _limits.Ore && stash.Ore <= _limits.Ore)
            {
                int b = HowManyBots(stash.Ore, _oreCost.Ore);
                yield return ((_oreCost * b), new Quad(b, 0, 0, 0));
            }
            if (stash.Ore >= _clayCost.Ore) // && bots.Clay <= _limits.Clay && stash.Clay <= _limits.Clay)
            {
                int b = HowManyBots(stash.Ore, _clayCost.Ore);
                yield return ((_clayCost * b), new Quad(0, b, 0, 0));
            }
            if (stash.Ore >= _obsidianCost.Ore) // && bots.Obsidian <= _limits.Obsidian && stash.Obsidian <= _limits.Obsidian)
            {
                int ore = HowManyBots(stash.Ore, _obsidianCost.Ore);
                int clay = HowManyBots(stash.Clay, _obsidianCost.Clay);
                int b = Math.Min(ore, clay);
                yield return (_obsidianCost * b, new Quad(0, 0, b, 0));
            }
            if (stash.Ore >= _geodeCost.Ore) // && stash.Obsidian >= _geodeCost.Obsidian)
            {
                int ore = HowManyBots(stash.Ore, _geodeCost.Ore);
                int obsidian = HowManyBots(stash.Obsidian, _geodeCost.Obsidian);
                int b = Math.Min(ore, obsidian);
                yield return (_geodeCost * b, new Quad(0, 0, 0, b));
            }
        }

        static int HowManyBots(int stash, int cost)
        {
            return stash / cost;
        }


        public int GeodeHunter()
        {
            int best = 0;
            HashSet<uint> topScores = new();
            topScores.Add(0);
            while (queue.Count > 0)
            {
                State state = queue.Dequeue();
                if (state.Minutes > State.Limit)
                    break;
                foreach (State next in state.NextState())
                {
                    if (queue.Any(p => next.IsStrictlyWorseThan(p)) || next.OreBots > OreBotLimit || next.StashedOre > 7)
                        continue;

                    if (state.Minutes == State.Limit)
                    {
                        Console.WriteLine($"{Id}: {next.Value}");

                        if (next.Value > best)
                            best = next.Value;
                    }
                    uint score = Score(next);
                    uint qualifer = topScores.Min();
                    if (score > qualifer)
                    {
                        topScores.Add(score);
                        if (topScores.Count > 1000)
                            topScores.Remove(qualifer);
                        queue.Enqueue(next);

                    }

                }
            }

            return best;
        }

        public uint Score(State state)
        {
            Quad stateScore = state.Score();
            return
                (uint)_geodeCost.Obsidian * (uint)_obsidianCost.Clay * (uint)_clayCost.Ore * (uint)stateScore.Geode
                + (uint)_obsidianCost.Clay * (uint)_clayCost.Ore * (uint)stateScore.Obsidian
                + (uint)_clayCost.Ore * (uint)stateScore.Clay
                + (uint)stateScore.Ore;
        }

    }


    class State
    {
        public static int Limit = 24;

        readonly Quad _stash;
        readonly Quad _bots;
        readonly Blueprint _bp;
        readonly int _minutes;
        readonly int _limit;

        public int Minutes => _minutes;
        public int Value => _stash.Geode;

        public int OreBots => _bots.Ore;
        public int ClayBots => _bots.Clay;
        public int StashedOre => _stash.Ore;
        public State(Blueprint bp, int minutes, Quad stash, Quad bots)
        {
            _stash = stash;
            _bots = bots;
            _bp = bp;
            _minutes = minutes;
        }

        public Quad Score()
        {
            return new Quad(
                _bots.Ore * Limit - Minutes + _stash.Ore,
                _bots.Clay * Limit - Minutes + _stash.Clay,
                _bots.Obsidian * Limit - Minutes + _stash.Obsidian,
                _bots.Geode * Limit - Minutes + _stash.Geode
                );

        }

        public IEnumerable<State> NextState()
        {
            if (_minutes >= Limit)
            {
                yield break;
            }
            Quad bots = (Quad)_bots.Clone();
            foreach ((Quad cost, Quad benefit) in _bp.Options(_stash, _bots))
            {
                yield return new State(_bp, _minutes + 1, bots + _stash - cost, _bots + benefit);
            }
            // don't make any new, just accrue - also the base initial case 
            yield return new State(_bp, _minutes + 1, bots + _stash, _bots);
        }

        public override string ToString() => $"{_bp.Id}: [{_minutes}] Stash: {_stash} Bots: {_bots}";

        public bool IsStrictlyWorseThan(State other)
        {
            bool b = _bots <= other._bots;
            bool s = _stash <= other._stash;
            return b && s;

        }
        public static bool operator <(State a, State b) => a._bots == b._bots && a._stash <= b._stash;
        public static bool operator >(State a, State b) => !(a < b);
    }
}