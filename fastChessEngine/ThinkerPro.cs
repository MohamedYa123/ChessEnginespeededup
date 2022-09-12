 using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace fastChessEngine
{
    public class pathway {
        public int move;
        public pathway child;
    }
    public partial class ThinkerPro
    {
       const int totalnumberassign = 10000;
        // int Blocks=70;
        // int blocksize = 10000;
        public int branchesfound = 0;
        ConcurrentStack<int> freeboardreadyforreset = new ConcurrentStack<int>();
        Stack<int> freeboard=new Stack<int>();
        public int depthM;
        public int bestmove;
        int depthhere = 0;
        int boardhere = 0;
        List<string> sequencemoves = new List<string>();
        zoobristhasher zb = new zoobristhasher();
        Dictionary<long, int> repeatedpositions = new Dictionary<long, int>();
        Dictionary<long, int> prevpathway = new Dictionary<long, int>();
        List<string> listpath = new List<string>();
        public void putrepeatedposition(long keyg)
        {
            repeatedpositions.Add(keyg, 0);
        }
        void boardresetter()
        {
            while (true)
            {
                if (freeboardreadyforreset.Count > 0)
                {
                    int f;
                    if (freeboardreadyforreset.TryPop(out f))
                    {
                        board_resetallsquares(f);
                        check_resetme(f);
                        freeboard.Push(f);
                    }
                }
                Thread.Sleep(1);
            }
        }
        object lockobj = new object();
        void pushfreeboard(int board)
        {
            freeboardreadyforreset.Push(board);
        }
        int getfreeboard()
        {
            lock (lockobj)
            {
                int res=0;
                //while(! freeboard.TryPop(out res))
                //{

                //}
                return res;
            }
        }
        public  Dictionary<long, double[]> positionslist = new Dictionary<long, double[]>();//after evaluating a position we save it here
        Dictionary<long, pathway> positionspaths = new Dictionary<long, pathway>();//after evaluating a position we save it here
    //    string lastmove = "";
        public double search(int side,int sidetomove,int depth,int board, bool withalphabeta, double alpha = -100000000, double beta = 10000000)
        {
            
            double getevalorpush(double evalfull,long keyfull,int depthfull)
            {
                //return evalfull;
                double[] evalanddepth = { evalfull, depthfull };
                if (!positionslist.ContainsKey(keyfull))
                {
                    positionslist.Add(keyfull, evalanddepth);
                  //  positionspaths.Add(keyfull, pth);
                }
                else 
                {
                    if (positionslist[keyfull][1] < depthfull)
                    {
                        positionslist[keyfull] = evalanddepth;
                     //   positionspaths[keyfull] = pthc;
                    }
                    else
                    {
                        return positionslist[keyfull][0];
                    }
                }
                return evalfull;
            }
            double getevalorpushlite(int boardfull,int sidefull, long keyfull, int depthfull,bool aretheremoves)
            {
                //return board_eval(boardfull, sidefull, depthfull, aretheremoves, sidetomove);
                if (!aretheremoves)
                {
                    return board_eval(boardfull, sidefull, depthfull, aretheremoves, sidetomove);
                }
                if (!positionslist.ContainsKey(keyfull))
                {
                    double[] evalanddepth = { board_eval(boardfull,sidefull,depthfull, aretheremoves, sidetomove), depthfull };
                    positionslist.Add(keyfull, evalanddepth);
                    return evalanddepth[0];
                 //   positionspaths.Add(keyfull, pthc);
                }
                else
                {
                    if (positionslist[keyfull][1] < depthfull)
                    {
                        double[] evalanddepth = { board_eval(boardfull, sidefull,depthfull, aretheremoves, sidetomove), depthfull };
                        positionslist[keyfull] = evalanddepth;
                    }
                    else
                    {
                        return positionslist[keyfull][0];
                    }
                }
                return board_eval(boardfull, sidefull, depthfull, aretheremoves, sidetomove);
            }
            if (depth == depthM)
            {
                branchesfound = 0;
                zb.generate();
                positionslist.Clear();
                prevpathway.Clear();
                positionspaths.Clear();
                for (int i = 0; i < 1000 - 1; i++)
                {
                    if (i != board)
                    {
                        board_resetallsquares(i);
                        check_resetme(i);
                        freeboard.Push(i);
                    }
                }
                Thread th = new Thread(() => boardresetter());
             //   th.Start();
            }
            depthhere = depth;
            boardhere = board;
            var key = board_getkey(board, sidetomove, side);
           
            if (repeatedpositions.ContainsKey(key) || prevpathway.ContainsKey(key))
            {
                freeboard.Push(board);
                branchesfound++;
                return 0;
            }
            prevpathway.Add(key, 0);
            if (positionslist.ContainsKey(key) && positionslist[key][1] >= depth)
            {
                //  var cdepth = positionslist[key][1];
                //if (depth > 3)
                //{
                //    var fh = positionslist[key][1];
                //}

                freeboard.Push(board);
                branchesfound++;
                prevpathway.Remove(key);
              //  pth.child = positionspaths[key];
                return positionslist[key][0];
            }
            if (depth == 1)
            {
                freeboard.Push(board);
                branchesfound++;
                //board_getallmoves(board, sidetomove);
                //var eval = board_eval(board, side);
                board_getallchecks(board, -1);
                prevpathway.Remove(key);
               // return 0;
                return getevalorpushlite(board,side,key,depth, board_arethermoves(board,sidetomove));

            }
            
            board_getallchecks(board,1-sidetomove);
            board_getallmoves(board, sidetomove);
            int nummoves = board_getboardfeature(board, 5);
            int[] movesindices = new int[nummoves];
            int[] capturevalues = new int[nummoves];
            string[] sad = new string[nummoves];
            for(int i = 0; i < nummoves; i++)
            {
                movesindices[i] = i;
                capturevalues[i] = -1*move_getmove_feature(board, i, 3);
                sad[i] = move_to_string(board, i);
            }
            Array.Sort(capturevalues, movesindices);
            //  Array.Sort(capturevalues, sad);

            double besteval = -1000000000;
            //  move bestmove;
            if (sidetomove != side)
            {
                besteval = 1000000000;
            }
            int topmove = 0;
            //pathway bestpth = null;
            double alpha2 = alpha;
            double beta2 = beta;
            if (sidetomove != side)
            {
                alpha2 = alpha;
                beta2 = 10000000;
            }
            else
            {
                alpha2 = -10000000;
                beta2 = beta;
            }
            for (int t = 0; t < nummoves; t++)
            {
                int br = movesindices[t];
                // var move = move_to_string(board, br);
                // bool yes = false;
                //  var ls = lastmove;
                //  //var ev = 0.0;
                //if (move == "Kb7" && lastmove== "Kd3")
                //{
                //    ev = board_eval(board, side, depth, true, sidetomove);
                //    yes = true;
                //}
                //if (depth == depthM)
                //{ sequencemoves.Clear(); }
                //h
                int newboard = freeboard.Pop();// getfreeboard();
                board_copyboard(board, newboard);
                move_aplymove(newboard, br,board);
                //pathway pth3 = new pathway();
                //pth3.move = br;
                //pth.child = pth3;
               // lastmove = br;
                var eval= search(side, 1 - sidetomove, depth - 1, newboard, withalphabeta, alpha2,beta2);
                if (eval == -12345)
                {
                    if (depth == 3)
                    {
                        board_tostring(board, richt1);
                    }
                    return eval;
                }
                //if (move.Contains("x") )
                //{

                //}
                //if ( yes && sidetomove==1&&ev<2)
                //{
                    
                    
                //}
                
                    if (sidetomove == side)
                {
                    if (eval > besteval)
                    {
                        besteval = eval;
                        // stmain.bestmove = fmove;
                        // bestmove = br;
                        //bestpth = pth3;
                        topmove = br;
                    }
                }
                else
                {
                    if (eval < besteval)
                    {
                        besteval = eval;
                        //bestmove = br;
                        //bestpth = pth3;
                        topmove = br;
                    }
                }
                //
                if (withalphabeta)
                {
                    if (sidetomove == side)
                    {
                        //it means white to move
                        if (beta2 <= eval)
                        {
                            //stmain.eval = st.eval;
                            freeboard.Push(board);
                            setbestmove(topmove, depth);
                            branchesfound++;
                            // pathway pth2 = new pathway();
                            // pth2.move = move;
                            //  pth.child = bestpth;
                            prevpathway.Remove(key);
                            //if (beta == eval)
                            //{
                            //    return besteval;
                            //}
                            return  getevalorpush(besteval, key, depth);
                        }
                        else
                        {
                            // beta2 = st.eval;
                        }
                        if (eval > alpha2)
                        {
                            alpha2 = eval;
                        }
                    }
                    else
                    {
                        if (alpha2 >= eval)
                        {
                            //stmeval = st.eval;
                            freeboard.Push(board);
                            setbestmove(topmove, depth);
                            branchesfound++;
                            // pathway pth2 = new pathway();
                            // pth2.move = move;
                            //     pth.child = bestpth;
                            prevpathway.Remove(key);
                            //if (alpha == eval)
                            //{
                            //    return besteval;
                            //}
                            return getevalorpush(besteval, key,depth);
                        }
                        else
                        {
                            //   alpha2 = st.eval;
                        }
                        if (eval < beta2)
                        {
                            beta2 = eval;
                        }
                    }
                }
            }
            
            if (nummoves == 0)
            {
                branchesfound++;
                freeboard.Push(board);
                prevpathway.Remove(key);
                return getevalorpushlite(board, side, key, depth,false); //board_eval(board, side, depth);
            }
            freeboard.Push(board);
            setbestmove(topmove, depth);
            if (depth == depthM)
            {

            }
            prevpathway.Remove(key);
            //pathway pth22 = new pathway();
            //pth22.move = move_to_string(board,bestmove);
           // pth.child = bestpth;
            return getevalorpush(besteval, key, depth);
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
