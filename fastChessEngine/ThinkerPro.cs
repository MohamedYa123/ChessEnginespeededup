 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastChessEngine
{
    public partial class ThinkerPro
    {
       const int totalnumberassign = 1000;
        // int Blocks=70;
        // int blocksize = 10000;
        public int branchesfound = 0;
        Stack<int> freeboard=new Stack<int>();
        public int depthM;
        public int bestmove;
        int depthhere = 0;
        int boardhere = 0;
        public double search(int side,int sidetomove,int depth,int board)
        {
            depthhere = depth;
            boardhere = board;
            if (depth == 1)
            {
                freeboard.Push(board);
                branchesfound++;
                return board_eval(board, side);

            }
            if (depth == depthM)
            {
                for(int i = 0; i < totalnumberassign-1; i++)
                {
                    if (i != board) {
                        freeboard.Push(i);
                    }
                }
            }
            board_getallchecks(board, 1 - sidetomove);
            board_getallmoves(board, sidetomove);
            int nummoves = board_getboardfeature(board, 5);
            for(int br = 0; br < nummoves; br++)
            {
                int newboard = freeboard.Pop();
                board_copyboard(board, newboard);
                move_aplymove(newboard, br,board);
                search(side, 1 - sidetomove, depth - 1, newboard);
            }
            if (nummoves == 0)
            {
                branchesfound++;
            }
            freeboard.Push(board);
            return 0;
        }
    }
}
