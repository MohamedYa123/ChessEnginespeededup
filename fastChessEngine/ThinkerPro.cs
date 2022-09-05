 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastChessEngine
{
    public partial class ThinkerPro
    {
       const int totalnumberassign = 10000;
        // int Blocks=70;
        // int blocksize = 10000;
        public int branchesfound = 0;
        Stack<int> freeboard=new Stack<int>();
        public int depthM;
        public int bestmove;
        int depthhere = 0;
        int boardhere = 0;
        List<string> sequencemoves = new List<string>();
        zoobristhasher zb = new zoobristhasher();
        Dictionary<long, double[]> positionslist = new Dictionary<long, double[]>();//after evaluating a position we save it here
        public double search(int side,int sidetomove,int depth,int board, bool withalphabeta, double alpha = -100000000, double beta = 10000000)
        {
            double getevalorpush(double evalfull,long keyfull,int depthfull)
            {
                double[] evalanddepth = { evalfull, depthfull };
                if (!positionslist.ContainsKey(keyfull))
                {
                    positionslist.Add(keyfull, evalanddepth);
                }
                else
                {
                    if (positionslist[keyfull][1] < depthfull)
                    {
                        positionslist[keyfull] = evalanddepth;
                    }
                    else
                    {
                        return positionslist[keyfull][0];
                    }
                }
                return evalfull;
            }
            double getevalorpushlite(int boardfull,int sidefull, long keyfull, int depthfull)
            {
                
                if (!positionslist.ContainsKey(keyfull))
                {
                    double[] evalanddepth = { board_eval(boardfull,sidefull,depthfull), depthfull };
                    positionslist.Add(keyfull, evalanddepth);
                }
                else
                {
                    if (positionslist[keyfull][1] < depthfull)
                    {
                        double[] evalanddepth = { board_eval(boardfull, sidefull,depthfull), depthfull };
                        positionslist[keyfull] = evalanddepth;
                    }
                    else
                    {
                        return positionslist[keyfull][0];
                    }
                }
                return board_eval(boardfull, sidefull, depthfull);
            }
            if (depth == depthM)
            {
                branchesfound = 0;
                zb.generate();
                positionslist.Clear();
                for (int i = 0; i < 1000 - 1; i++)
                {
                    if (i != board)
                    {
                        freeboard.Push(i);
                    }
                }

            }
            depthhere = depth;
            boardhere = board;
            var key =  board_getkey(board, sidetomove, side);
            //if(positionslist.ContainsKey(key)&& positionslist[key][1] >= depth)
            //{
            //    freeboard.Push(board);
            //    branchesfound++;
            //    return positionslist[key][0];
            //}
            if (depth == 1)
            {
                freeboard.Push(board);
                branchesfound++;
                //var eval = board_eval(board, side);
                
                return getevalorpushlite(board,side,key,depth);

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
               // var move = move_to_string(board, br);
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
                            branchesfound++;
                            return  getevalorpush(besteval, key, depth);
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
                            branchesfound++;
                            return getevalorpush(besteval, key,depth);
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
                freeboard.Push(board);
                return getevalorpushlite(board, side, key, depth); //board_eval(board, side, depth);
            }
            freeboard.Push(board);
            setbestmove(topmove, depth);
            if (depth == depthM)
            {

            }
            return getevalorpush(besteval, key, depth); ;
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
