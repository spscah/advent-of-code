using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;

namespace DiracDice.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 0;
            const bool TEST = false;
            IList<string> data = TODAY.AsListOfStrings(TEST);

            IList<int> playerPositions = data.Select(d => int.Parse(d.Split(' ').Last())).ToList();
            int player = 0;
            IList<int> playScores = new List<int> { 0,0};
            int turns = 0;

            while(playScores.All(s => s < 1000)) {
                int dieScore = ((turns*3)+2)*3;
                int t = (playerPositions[player]-1+dieScore)%10 + 1;
                playScores[player] += t;
                playerPositions[player] = t;

                player = (1-player);
                ++turns;
            }

            Console.WriteLine($"{playScores[player] * turns * 3}");

            IList<(int,int)> diracTurnScores = new List<(int, int)> { 
                (3,1), 
                (4,3),
                (5,6), 
                (6,7),
                (7,6),
                (8,3), 
                (9,1)};


            playerPositions = data.Select(d => int.Parse(d.Split(' ').Last())).ToList();

            Queue<Universe> universes = new Queue<Universe>( new List<Universe> {(0,playerPositions[0], 0, playerPositions[1], true, 1)});

            IList<ulong> winningUniverses = new List<ulong> { 0,0};
/*
            ulong threes = 1;
            for(int i = 0; i < 10; ++i) {
                threes *= 27;
                if(i >= 8)
                    Console.WriteLine($"{i.ToString().PadLeft(2)} : {threes}");
            }

            ulong a = 444356092776315;
            ulong b = 341960390180808; 
            Console.WriteLine($"{"win".PadLeft(4)} {a}");
            Console.WriteLine($"{"lose".PadLeft(4)} {b}");
            Console.WriteLine($"{"=".PadLeft(4)} {a+b}");
*/
            while(universes.Count > 0) {
                // take each universe and spawn off each of the dirac children 
                Universe universe = universes.Dequeue();

                foreach(Universe u in universe.TakeATurn(diracTurnScores)) {
                // any scores at least 21 get added to the counter, the rest are pushed back into the 
                    int wp =u.WinningPlayer;
                    if(wp >= 0)
                        winningUniverses[wp] += u.NumberOfUniverses;
                    else 
                        universes.Enqueue(u);

                }
            }
/*
            for(int i = 0; i < 2; ++i) {
                Console.WriteLine($"  p{i} {winningUniverses[i]}");
            }
*/
            Console.WriteLine($"{winningUniverses.Max()}");
    }

        class Universe { 
            readonly (int p1, int p2) _scores;
            readonly (int p1, int p2) _positions; 

            readonly bool _playerOne; 

            readonly ulong _numberOfUniverses;

            public ulong NumberOfUniverses => _numberOfUniverses;

            public int WinningPlayer {
                get {
                    if(_scores.p1 >= 21)
                        return 0;
                    if(_scores.p2 >= 21)
                        return 1;
                    return -1;
                }
            }

            public Universe(int p1s, int p1p, int p2s, int p2p, bool playerOne, ulong u) {
                _scores = (p1s, p2s);
                _positions = (p1p, p2p);
                _numberOfUniverses = u;
                _playerOne = playerOne;
            }

            public IEnumerable<Universe> TakeATurn(IList<(int,int)> diracTurns) {
                foreach((int s, int u) in diracTurns) {
                    int pos = _playerOne ? _positions.p1 : _positions.p2;
                    int t = (pos-1+s)%10 + 1;
                    if(_playerOne)                                      
                        yield return (_scores.p1 + t, t, _scores.p2, _positions.p2, !_playerOne, _numberOfUniverses * (ulong)u);
                    else
                        yield return (_scores.p1, _positions.p1, _scores.p2 + t, t, !_playerOne, _numberOfUniverses * (ulong)u);

                }
            }

            public static implicit operator Universe((int p1s, int p1p, int p2s, int p2p, bool p1, ulong u) value) => new Universe(value.p1s, value.p1p, value.p2s, value.p2p, value.p1, value.u);
           
        }
    }
}