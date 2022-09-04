using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastChessEngine
{
     partial class ThinkerPro
    {
        //let's beegin with the squares
        const int total_Squarefeatures=23;
        //checked_white,checked_black,occupied_white,occupied_black,piece,rowCheckrightw,rowcheckleftw,rowCheckrightb,rowcheckleftb
        //colcheckupw,colcheckdownw,colcheckupb,colcheckdownb,diagonalcheckrightw,diagonalcheckleftw,diagonalcheckupw,diagonalcheckdownw
        //diagonalcheckrightb,diagonalcheckleftb,diagonalcheckupb,diagonalcheckdownb,wpoint,bbpoint
        int[] squares;//1000,[100 000,8,8,27]
        int[] wcontrolers;
        int[] bcontrolers;
        public ThinkerPro()
        {
            squares = new int[totalnumberassign * 8 * 8 * total_Squarefeatures];
            wcontrolers = new int[totalnumberassign* 8* 8* 15];
            bcontrolers = new int[totalnumberassign * 8 * 8 * 15];
            setboards();
            setchecks();
            setmoves();
            setpieces();
            setsituations();

        }
        ///<summary>
        ///features
        ///0=checked_white,1=checked_black,2=occupied_white,3=occupied_black,4=piece,5=rowCheckrightw,6=rowcheckleftw,7=rowCheckrightb,8=rowcheckleftb
        ///9=colcheckupw,10=colcheckdownw,11=colcheckupb,12=colcheckdownb,13=diagonalcheckrightupw,14=diagonalcheckrightdownw
        ///15=diagonalcheckrightupb,16=diagonalcheckrightdownb,17=diagonalcheckleftupw,18=diagonalcheckleftdownw
        ///19=diagonalcheckleftupb,20=diagonalcheckleftdownb,21=wpoint,22=bbpoint
        ///</summary>
        void square_setsquare_feature(int board,int col,int row,int featureid,int value)
        {
            squares[board * 8 * 8 * total_Squarefeatures + (col * 8 + row * 1) * total_Squarefeatures + featureid] = value;
        }
        ///<summary>
        ///features
        ///0=checked_white,1=checked_black,2=occupied_white,3=occupied_black,4=piece,5=rowCheckrightw,6=rowcheckleftw,7=rowCheckrightb,8=rowcheckleftb
        ///9=colcheckupw,10=colcheckdownw,11=colcheckupb,12=colcheckdownb,13=diagonalcheckrightupw,14=diagonalcheckrightdownw
        ///15=diagonalcheckrightupb,16=diagonalcheckrightdownb,17=diagonalcheckleftupw,18=diagonalcheckleftdownw
        ///19=diagonalcheckleftupb,20=diagonalcheckleftdownb,21=wpoint,22=bbpoint
        ///</summary>
        public  int square_getsquare_feature(int board, int col, int row, int featureid)
        {
            return  squares[board * 8 * 8 * total_Squarefeatures + (col * 8 + row * 1) * total_Squarefeatures + featureid];
        }
        ///<summary>
        ///features
        ///0=checked_white,1=checked_black,2=occupied_white,3=occupied_black,4=piece,5=rowCheckrightw,6=rowcheckleftw,7=rowCheckrightb,8=rowcheckleftb
        ///9=colcheckupw,10=colcheckdownw,11=colcheckupb,12=colcheckdownb,13=diagonalcheckrightupw,14=diagonalcheckrightdownw
        ///15=diagonalcheckrightupb,16=diagonalcheckrightdownb,17=diagonalcheckleftupw,18=diagonalcheckleftdownw
        ///19=diagonalcheckleftupb,20=diagonalcheckleftdownb,21=wpoint,22=bbpoint
        ///</summary>
        int square_litefeature_extractor(int part,int featureid)
        {
            return squares[part + featureid];
        }
        ///<summary>
        ///features
        ///0=checked_white,1=checked_black,2=occupied_white,3=occupied_black,4=piece,5=rowCheckrightw,6=rowcheckleftw,7=rowCheckrightb,8=rowcheckleftb
        ///9=colcheckupw,10=colcheckdownw,11=colcheckupb,12=colcheckdownb,13=diagonalcheckrightupw,14=diagonalcheckrightdownw
        ///15=diagonalcheckrightupb,16=diagonalcheckrightdownb,17=diagonalcheckleftupw,18=diagonalcheckleftdownw
        ///19=diagonalcheckleftupb,20=diagonalcheckleftdownb,21=wpoint,22=bbpoint
        ///</summary>
        void square_litefeature_assign(int part,int featureid,int value)
        {
             squares[part + featureid]=value;
        }
        int square_getpart(int board, int col, int row)
        {
            return board * 8 * 8 * total_Squarefeatures + (col * 8 + row * 1) * total_Squarefeatures;
        }

        void squaresetoccupy(int board,int piece,int col,int row)
        {
            int part = square_getpart(board, col, row);
            
            var oldpiece = square_litefeature_extractor(part, 4);
            if (oldpiece!= -1)
            {
                piece_setpiece_feature(board, oldpiece, 3, -1);
            }
            square_litefeature_assign(part, 4, piece);
            if ( piece_getpiece_feature(board, piece, 4)==0){
                square_litefeature_assign(part, 2, 1);
                square_litefeature_assign(part, 3, 0);
            }
            else
            {
                square_litefeature_assign(part, 2, 0);
                square_litefeature_assign(part, 3, 1);
            }

        }
        void square_freesquare(int board,int col,int row)
        {
            int part = square_getpart(board, col, row);
            square_litefeature_assign(part, 2, 0);
            square_litefeature_assign(part, 4, -1);
            square_litefeature_assign(part, 3, 0);
        }
        string square_to_string(int board,int col,int row)
        {
            string ans = "";


                    var piece = square_getsquare_feature(board, col, row, 4);

                    if (piece == -1)
                    {
                        ans += "   ";
                    }
                    else
                    {
                        ans +=""+ piece_to_string(board, piece)+"";
                    }
                  


            return ans;
        }
        void square_init_square(int board,int col,int row,int piece)
        {
            square_reset_square(board, col, row);
            int side = piece_getpiece_feature(board, piece, 4);
            square_setsquare_feature(board, col, row, 2 + side, 1);
            square_setsquare_feature(board, col, row, 4, piece);
        }
        
        void square_reset_square(int board,int col,int row)
        {
            var l = board * 8 * 8 * total_Squarefeatures + (col * 8 + row * 1) * total_Squarefeatures;
           // for(int u = 0; u < total_Squarefeatures; u++)
           // {
                squares[l + 0] = 0;
                squares[l + 1] = 0;
                squares[l + 2] = 0;
                squares[l + 3] = 0;
                squares[l + 4] = -1;
                squares[l + 5] = 0;
                squares[l + 6] = 0;
                squares[l + 7] = 0;
                squares[l + 8] = 0;
                squares[l + 9] = 0;
                squares[l + 10] = 0;
                squares[l + 11] = 0;
                squares[l + 12] = 0;
                squares[l + 13] = 0;
                squares[l + 14] = 0;
                squares[l + 15] = 0;
                squares[l + 16] = 0;
                squares[l + 17] = 0;
                squares[l + 18] = 0;
                squares[l + 19] = 0;
                squares[l + 20] = 0;
                squares[l + 21] = 0;
                squares[l + 22] = 0;
            //increaselock();
          //  }
          //  squares[l + 4] = -1;
        }
        public void assign()
        {
            var x = 10000 * 8 * 8 * 27;
            int[] ax = new int[8*8*27*100];
            // int u = 0;
            byte r = 1;
            void doassign( int rr, int r2, int r3, int r4, int val)
            {
                ax[r * r2 * r3 * r4] = val;
            }
            for (int i = 0; i <10000*8*8*27 ; i++)
            {

                //ax[0] = r;
                doassign( 1, 1, 1, 1, 1);
            }
        }
        
    }
}
