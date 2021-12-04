using System;
using System.Collections.Generic;
using AdventOfCode.Lib;
using System.Linq;
using System.Text;

namespace GiantSquid.App
{
    class Program
    {
        static void Main(string[] args)
        {
            const bool TEST = false;
            const int TODAY = 4;
            IList<string> data = TODAY.AsListOfStrings(TEST);

            IList<int> calls = data[0].Split(',').Select(x => Convert.ToInt32(x)).ToList();

            int from = 2;
            IList<Board> boards = new List<Board>();
            while(from < data.Count) {
                boards.Add(new Board(data, from));
                from += 6;
            }
            
            bool won = false;
            int play = 0;
            int call;
            while(!won) {
               call = calls[play];
               foreach(Board b in boards) {
                   b.Mark(call);
                   if(b.Win) {
                       Console.WriteLine(b.Score(call));
                       won = true;
                   }
                   //Console.WriteLine(b);
                } 
                ++play;
            }
            // reset the boards 
            foreach(Board b in boards)
                b.Reset();

            // part 2
            play = 0;
            call=-1;
            Board lastBoard = null;
            while(boards.Count > 0) {
               call = calls[play];
               IList<int> doomed = new List<int>();
               for(int b = 0; b < boards.Count; ++b) {
                   boards[b].Mark(call);
                   if(boards[b].Win) {
                        doomed.Add(b);     
                   }
                } 
                ++play;
                foreach(int d in doomed.OrderByDescending(d => d))
                    boards.RemoveAt(d);
                if(boards.Count == 1)
                    lastBoard = boards[0];
            }
            // 3640 is too high 
            Console.WriteLine(lastBoard.Score(call));

        }
    }

    class Board {
        IList<int> _board;

        public Board(IList<string> next, int from) {
            _board = new List<int>();
            for(int i = 0; i < 5; ++i){
                foreach(int n in Enumerable.Range(0,4).Select(j => Convert.ToInt32( next[i+from].Substring(j*3,3))))
                    _board.Add(n);
                _board.Add(Convert.ToInt32(next[i+from].Substring(12)));
            }

        }

        public void Reset(){
            for(int i = 0 ; i< _board.Count; ++i)
                if(_board[i] < 0)
                    _board[i] *= -1;
        }

        public void Mark(int call) {
            if (!_board.Contains(call))
                return;
            int location = _board.IndexOf(call);
            _board[location] = -call;
        }

        public bool Win { 
            get {   
                for(int x = 0; x < 5; ++x)
                {
                    // x is the column
                    bool all = true;
                    int offset;
                    for(offset = 0; offset < _board.Count; offset +=5) {
                        if(_board[offset+x] > 0)
                            all = false;
                    }
                    if(all)
                        return true;

                    // x is the row 
                    all = true;
                    offset = x*5;
                    for(int r = 0; r < 5; ++r) {
                        if(_board[offset+r] > 0)
                            all = false;
                    }                    
                    if(all)
                        return true;

                }                
                return false;                    
            }
        }

        public int Score(int last) {
            return _board.Where(n => n > 0).Sum() * last;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < _board.Count; ++i)
            {
                string s = _board[i] < 0 ? "   " : _board[i].ToString().PadLeft(3);
                sb.Append($"{s}");
                if( (i+1) % 5 ==0)
                    sb.AppendLine();

            }
            return sb.ToString();
        }
    }
}
