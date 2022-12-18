using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace PyroclasticFlow.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const int TODAY = 17;
            IList<string> test = TODAY.AsListOfStrings(true);
            Debug.Assert(Result(test) == (3068, 1514285714288UL));

            IList<string> real = TODAY.AsListOfStrings(false);
            (uint partone, ulong parttwo) result = Result(real);
            Console.WriteLine($"Part 1: {result.partone}");
            Console.WriteLine($"Part 2: {result.parttwo}");
        }

        static (uint partone, ulong parttwo) Result(IList<string> real)
        {
            string pattern = real[0];
            int puff = 0;

            List<int> tower = new();
            List<IList<int>> blocks = new();
            blocks.Add(new List<int> { 30 });
            blocks.Add(new List<int> { 8, 28, 8 });
            blocks.Add(new List<int> { 4, 4, 28 });
            blocks.Add(new List<int> { 16, 16, 16, 16 });
            blocks.Add(new List<int> { 24, 24 });

            ulong c = 0;
            uint partone = 0u;
            ulong parttwo = 0ul;

            Dictionary<(string, int), IList<(int, ulong)>> logs = new();
            ulong target = 1000000000000UL;



            while (c < target)
            {
                // grab the next block 
                IList<int> block = blocks[(int)(c % 5)].Select(b => b).ToList();

                // for each rock falling add empty rows to the end of the list, and one for each of the block rows
                for (int empty = 0; empty < 3 + block.Count; ++empty)
                    tower.Add(0);

                string combo = string.Empty;
                while (true)
                {
                    // can it move down? with the appropriate left or right
                    char lr = pattern[(puff) % pattern.Length];
                    combo += lr;
                    (bool result, IList<int> b) = LeftOrRightAndDown(lr, tower, block);
                    block = b;
                    //Dump(tower, block, result, lr);
                    ++puff;
                    if (!result)
                    {
                        // if not lock it in then break
                        LockItIn(tower, block);
                        break;
                    }
                }

                // trim the end of the list to remove zeros 
                TrimTower(tower);

                (string, int) key = (combo, (int)(c % 5));
                (int, ulong) value = (tower.Count, c);

                if (logs.ContainsKey(key))
                {
                    if (logs[key].Count < 5 || c < 2023)
                        logs[key].Add(value);
                    else
                    {
                        var previous = logs[key];
                        if ((value.Item1 - previous[^1].Item1 == previous[^1].Item1 - previous[^2].Item1)
                        && (previous[^2].Item1 - previous[^3].Item1 == previous[^1].Item1 - previous[^2].Item1)
                        && (value.Item2 - previous[^1].Item2 == previous[^1].Item2 - previous[^2].Item2)
                        && (previous[^2].Item2 - previous[^3].Item2 == previous[^1].Item2 - previous[^2].Item2))
                        {
                            Console.WriteLine($"{key.Item1}, {key.Item2}");
                            Console.WriteLine("yes");
                            int delta = value.Item1 - previous[^1].Item1;
                            ulong period = value.Item2 - previous[^1].Item2;
                            ulong divisions = (target - c) / period;
                            parttwo = (ulong)tower.Count + divisions * (ulong)delta;
                            c = c + divisions * period;
                            ulong remaining = target - c;
                            ulong offset = (ulong)remaining - 1L + previous[^1].Item2;

                            int extra = logs.Values.Where(v => v.Any(p => p.Item2 == offset)).First().Last(p => p.Item2 == offset).Item1 - previous[^1].Item1;

                            parttwo += (ulong)extra;

                            return (partone, parttwo);


                        }
                        else
                            logs[key].Add(value);
                    }
                }
                else
                    logs.Add(key, new List<(int, ulong)> { value });



                ++c;
                if (c == 2022)
                {
                    partone = (uint)tower.Count;
                }
            }


            return (partone, parttwo + (ulong)tower.Count);
        }


        static int GetHashForTower(List<int> tower, int cHashes)
        {
            unchecked
            {
                int hash = 19;
                foreach (int t in tower.TakeLast(cHashes))
                    hash = hash * 31 + t;
                return hash;
            }
        }

        static void Dump(List<int> tower, IList<int> block, bool result, char lr)
        {
            Console.WriteLine($"{lr} {result}");
            for (int i = 0; i < tower.Count; ++i)
            {
                int row = tower.Count - 1 - i;
                for (int col = 6; col >= 0; --col)
                {
                    if (i < block.Count)
                    {
                        char ch = ((block[i] & (1 << col)) != 0) ? '@' : '.';
                        Console.Write(ch);
                    }
                    else
                    {
                        char ch = ((tower[row] & (1 << col)) != 0) ? '#' : '.';
                        Console.Write(ch);

                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void LockItIn(IList<int> tower, IList<int> block)
        {
            for (int i = 0; i < block.Count; ++i)
                tower[^(i + 1)] |= block[i];
        }

        static (bool, IList<int>) LeftOrRightAndDown(char lr, IList<int> tower, IList<int> block)
        {
            if (lr == '<')
                block = Left(tower, block);
            else
                block = Right(tower, block);
            return Down(tower, block);
        }
        /*
                static IList<int> LeftOrRight(char lr, IList<int> tower, IList<int> block)
                {
                    if (lr == '<')
                        block = Left(tower, block);
                    else
                        block = Right(tower, block);
                    return block;
                }
        */
        static IList<int> Left(IList<int> tower, IList<int> block)
        {
            if (block.Any(b => (b & (1 << 6)) > 0))
                return block;
            IList<int> updated = block.Select(b => b << 1).ToList();
            block = IsClear(tower, updated) ? updated : block;
            return block;
        }

        static IList<int> Right(IList<int> tower, IList<int> block)
        {
            if (block.Any(b => ((b & 1) > 0)))
                return block;
            IList<int> updated = block.Select(b => b >> 1).ToList();
            block = IsClear(tower, updated) ? updated : block;
            return block;
        }

        static (bool, IList<int>) Down(IList<int> tower, IList<int> block)
        {
            IList<int> updated = block.Select(b => b).ToList();
            updated.Insert(0, 0);
            bool able = IsClear(tower, updated);
            block = able ? updated : block;
            return (able, block);
        }

        static bool IsClear(IList<int> tower, IList<int> block)
        {
            for (int i = 0; i < block.Count; ++i)
            {
                if (((i + 1) > tower.Count) || (block[i] & tower[^(i + 1)]) != 0)
                    return false;
            }
            return true;
        }

        static void TrimTower(IList<int> tower)
        {
            while (tower.Last() == 0)
                tower.RemoveAt(tower.Count - 1);

        }
    }
}