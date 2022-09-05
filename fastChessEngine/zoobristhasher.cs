using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastChessEngine
{
    [Serializable]
    public  class zoobristhasher
    {
        public  long[,,] squares_pawn;
        public  long[,,] squares_Queen;
        public  long[,,] squares_Rook;
        public  long[,,] squares_Bishop;
        public  long[,,] squares_Knight;
        public  long[,,] squares_king;
        static long LongRandom(long min, long max, Random rand)
        {
            long result = rand.Next((Int32)(min >> 32), (Int32)(max >> 32));
            result = (result << 32);
            result = result | (long)rand.Next((Int32)min, (Int32)max);
            return result;
        }
        public  void generate()
        {
            squares_pawn = new long[9, 9, 2];
            squares_Queen = new long[9, 9, 2];
            squares_Rook = new long[9, 9, 2];
            squares_Bishop = new long[9, 9, 2];
            squares_Knight = new long[9, 9, 2];
            squares_king = new long[9, 9, 2];
            Random rd = new Random();
            for (int i = 0; i < 9; i++)
            {
                for (int i2 = 0; i2 < 9; i2++)
                {
                    for (int i3 = 0; i3 < 2; i3++)
                    {
                        squares_pawn[i, i2, i3] = LongRandom(int.MinValue, int.MaxValue, rd);
                        //  Thread.Sleep(1);
                        squares_Queen[i, i2, i3] = LongRandom(int.MinValue, int.MaxValue, rd);
                        //Thread.Sleep(1);
                        squares_Rook[i, i2, i3] = LongRandom(int.MinValue, int.MaxValue, rd);
                        //Thread.Sleep(1);
                        squares_Knight[i, i2, i3] = LongRandom(int.MinValue, int.MaxValue, rd);
                        //Thread.Sleep(1);
                        squares_Bishop[i, i2, i3] = LongRandom(int.MinValue, int.MaxValue, rd);
                        //Thread.Sleep(1);
                        squares_king[i, i2, i3] = LongRandom(int.MinValue, int.MaxValue, rd);
                        //Thread.Sleep(1);
                    }
                }

            }
        }

    }
}
