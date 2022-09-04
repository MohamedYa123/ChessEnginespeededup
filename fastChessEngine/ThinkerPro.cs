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
        List<string> sequencemoves = new List<string>();
        public double search(int side,int sidetomove,int depth,int board, bool withalphabeta, double alpha = -100000000, double beta = 10000000)
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
                branchesfound = 0;
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
            int[] movesindices = new int[nummoves];
            int[] capturevalues = new int[nummoves];
            for(int i = 0; i < nummoves; i++)
            {
                movesindices[i] = i;
                capturevalues[i] = -1*move_getmove_feature(board, i, 3);
            }
            Array.Sort(capturevalues, movesindices);
            double besteval = -1000000000;
            //  move bestmove;
            if (sidetomove != side)
            {
                besteval = 1000000000;
            }
            int topmove = 0;
            for (int t = 0; t < nummoves; t++)
            {
                int br = movesindices[t];
                if (depth == depthM)
                { sequencemoves.Clear(); }
                //h
                int newboard = freeboard.Pop();
                board_copyboard(board, newboard);
                move_aplymove(newboard, br,board);
                var eval= search(side, 1 - sidetomove, depth - 1, newboard, withalphabeta, alpha,beta);
                if (sidetomove == side)
                {
                    if (eval > besteval)
                    {
                        besteval = eval;
                        // stmain.bestmove = fmove;
                        // bestmove = br;
                        topmove = br;
                    }
                }
                else
                {
                    if (eval < besteval)
                    {
                        besteval = eval;
                        //bestmove = br;
                        topmove = br;
                    }
                }
                //
                if (withalphabeta)
                {
                    if (sidetomove == side)
                    {
                        //it means white to move
                        if (beta <= eval)
                        {
                            //stmain.eval = st.eval;
                            freeboard.Push(board);
                            setbestmove(topmove, depth);
                            return eval;
                        }
                        else
                        {
                            // beta2 = st.eval;
                        }
                        if (eval >= alpha)
                        {
                            alpha = eval;
                        }
                    }
                    else
                    {
                        if (alpha >= eval)
                        {
                            //stmeval = st.eval;
                            freeboard.Push(board);
                            setbestmove(topmove, depth);
                            return eval;
                        }
                        else
                        {
                            //   alpha2 = st.eval;
                        }
                        if (eval <= beta)
                        {
                            beta = eval;
                        }
                    }
                }
            }
            if (nummoves == 0)
            {
                branchesfound++;
            }
            freeboard.Push(board);
            setbestmove(topmove, depth);
            return besteval;
        }
        void setbestmove(int top,int depth)
        {
            if (depth == depthM)
            {
                bestmove = top;
            }
        }
    }

}
