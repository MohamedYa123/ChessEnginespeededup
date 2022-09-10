using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace fastChessEngine
{
    partial class ThinkerPro
    {
        //now board
        const int totalboardfeatures = 6;
        int[] boards;
        //type 0 pawn , 1 rook,2 knight,3 bishop , 4 queen , 5 king
        // x is col number
         void setboards()
        {
            //assign pieces
            boards = new int[totalnumberassign * 1 * totalboardfeatures];
            // 0=key,1=o_owhite , 2=o_o_owhite , 3=o_oblack , 4=o_o_oblack , 5=pointer(num of moves)
        }
        int kingcol = -1;
        int kingrow = -1;
        int queencol = -1;
        int queenrow = -1;
        int rook1col = -1;
        int rook1row = -1;
        int rook2col = -1;
        int rook2row = -1;
        void board_setfeatureton1()
        {
             kingcol = -1;
             kingrow = -1;
             queencol = -1;
             queenrow = -1;
             rook1col = -1;
             rook1row = -1;
             rook2col = -1;
             rook2row = -1;
        }
        /// <summary>
        /// 0=key,1=o_owhite , 2=o_o_owhite , 3=o_oblack , 4=o_o_oblack , 5=pointer(num of moves)
        /// </summary>
        public int board_getboardfeature(int board,int feature)
        {
            return boards[board  * totalboardfeatures + feature];
        }
        /// <summary>
        /// 0=key,1=o_owhite , 2=o_o_owhite , 3=o_oblack , 4=o_o_oblack , 5=pointer(num of moves)
        /// </summary>
        public void board_setboardfeature(int board,int feature,int value)
        {
            boards[board  * totalboardfeatures + feature] = value;
        }
        public void board_increasepointer(int board)
        {
            boards[board * totalboardfeatures + 5]++;
        }
        public void board_copyboard(int board,int newboard)
        {
            //copy board features ##
            board_setfeatureton1();
            var p = board * totalboardfeatures;
            var np = newboard * totalboardfeatures;
            for(int i = 0; i < totalboardfeatures; i++)
            {
                boards[np + i] = boards[p + i];
            }
            boards[np + 5] = 0;
            check_resetme(newboard);
            //reset squares
            board_resetallsquares(newboard);
            //copypieces
            for(int i = 0; i < 32; i++)
            {
                piece_copytonewpiece(board, newboard, i);
            }
        }
        public string board_tostring(int board,RichTextBox rtc=null)
        {
            string ans = "";
            Color c=Color.White;
            for(int i = 7; i >-1; i--)
            {
                if (i != 7)
                {
                    ans += "\n";
                    rtc.SelectedText = Environment.NewLine;
                }
                for (int i2 = 0; i2 <8; i2++)
                {
                    var a= square_to_string(board, i2, i);
                    if (a == " P ")
                    {

                    }
                    var pp = square_getsquare_feature(board, i2, i, 4);
                    if (pp != -1) {
                        if (piece_getpiece_feature(board,pp , 4) == 1)
                        {
                            c = Color.Black;
                        }
                        else
                        {
                            c = Color.White;
                        } }
                    rtc.SelectionColor = c;
                    if ((i + i2) % 2 == 1)
                    {
                        rtc.SelectionBackColor = Color.Wheat;
                    }
                    else
                    {
                        rtc.SelectionBackColor = Color.Orange;
                    }
                    rtc.SelectedText = a;
                    ans += a;
                }
            }
            return ans;
        }
        public void board_putpiecesinsquares(int board)
        {
            for (int i = 0; i < 32; i++)
            {
                if (piece_getpiece_feature(board, i, 3) != -1)
                {
                    piece_putpieceinsquare(board, i);
                }
            }
        }
        int alldone = 0;
        void increaselock()
        {
            Interlocked.Increment(ref alldone);
        }
        public void board_resetallsquares(int board)
        {
            alldone = 0;
            for(int i = 0; i < 8; i++)
            {
                for(int i2 = 0; i2 < 8; i2++)
                {
                     square_reset_square(board, i, i2);
                   // Task ts = new Task(() => square_reset_square(board, i, i2));
                  //  ts.Start();
                }
            }
            //while (alldone < 64)
            //{

            //}
        }

        /* slightly faster code
         square_reset_square(board,0, 0);
square_reset_square(board,0, 1);square_reset_square(board,0, 2);square_reset_square(board,0, 3);square_reset_square(board,0, 4);square_reset_square(board,0, 5);
square_reset_square(board,0, 6);square_reset_square(board,0, 7);square_reset_square(board,1, 0);square_reset_square(board,1, 1);square_reset_square(board,1, 2);square_reset_square(board,1, 3);square_reset_square(board,1, 4);
square_reset_square(board,1, 5);square_reset_square(board,1, 6);square_reset_square(board,1, 7);square_reset_square(board,2, 0);square_reset_square(board,2, 1);square_reset_square(board,2, 2);square_reset_square(board,2, 3);
square_reset_square(board,2, 4);square_reset_square(board,2, 5);square_reset_square(board,2, 6);square_reset_square(board,2, 7);square_reset_square(board,3, 0);square_reset_square(board,3, 1);square_reset_square(board,3, 2);
square_reset_square(board,3, 3);square_reset_square(board,3, 4);square_reset_square(board,3, 5);square_reset_square(board,3, 6);square_reset_square(board,3, 7);
square_reset_square(board,4, 0);square_reset_square(board,4, 1);
square_reset_square(board,4, 2);square_reset_square(board,4, 3);square_reset_square(board,4, 4);square_reset_square(board,4, 5);square_reset_square(board,4, 6);
square_reset_square(board,4, 7);square_reset_square(board,5, 0);
square_reset_square(board,5, 1);square_reset_square(board,5, 2);square_reset_square(board,5, 3);square_reset_square(board,5, 4);square_reset_square(board,5, 5);
square_reset_square(board,5, 6);square_reset_square(board,5, 7);square_reset_square(board,6, 0);square_reset_square(board,6, 1);square_reset_square(board,6, 2);square_reset_square(board,6, 3);square_reset_square(board,6, 4);
square_reset_square(board,6, 5);square_reset_square(board,6, 6);square_reset_square(board,6, 7);square_reset_square(board,7, 0);square_reset_square(board,7, 1);square_reset_square(board,7, 2);square_reset_square(board,7, 3);
square_reset_square(board,7, 4);square_reset_square(board,7, 5);square_reset_square(board,7, 6);square_reset_square(board,7, 7);
         */
        void board_resetallmoves(int board)
        {
            for(int i = 0; i < 200; i++)
            {
                move_setmove_feature(board, i, 8, -1);
            }
        }
        public string board_listallmoves(int board)
        {
            string ans = "";
            var max = board_getboardfeature(board, 5);
            for (int i = 0; i < max; i++)
            {
                var s = move_to_string(board, i);
                if (s == "")
                {
                    break;
                }
                ans +=(i+1)+" - "+s +"\r\n";
            }
            return ans;
         }
        public void board_getallmoves(int board,int side)
        {
            for(int i = 0; i < 32; i++)
            {
                if (piece_getpiece_feature(board, i, 3) != -1 &&( piece_getpiece_feature(board, i, 4)==side||side==-1))
                {
                    piece_getmove(board, i);
                }
            }
        }
        public bool board_arethermoves(int board, int side)
        {
            board_setboardfeature(board, 5,0);
            for (int i = 0; i < 32; i++)
            {
                if (piece_getpiece_feature(board, i, 3) != -1 && (piece_getpiece_feature(board, i, 4) == side || side == -1))
                {
                    piece_getmove(board, i);
                    if (board_getboardfeature(board, 5) != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public void board_resetlongmovefirstpieces(int board)
        {
            for (int i = 0; i < 32; i++)
            {
                
                {
                    piece_setpiece_feature(board, i,6,0);
                }
            }
        }
        public void board_getallchecks(int board, int side)
        {
            for (int i = 0; i < 32; i++)
            {
                if (piece_getpiece_feature(board, i, 3) != -1 && (piece_getpiece_feature(board, i, 4)==side||side==-1))
                {
                    piece_getchecks(board, i);
                }
            }
        }
        int infinity = 1000000;
        public double board_eval(int board,int side,int depth,bool aretheremoves,int sidetomove)
        {
            double ans = 0;
            var d = 0;
            if (!aretheremoves)
            {
                var numpieces = check_getcheckfeature(board, sidetomove, 0);
                if (numpieces == 0)
                {
                    return 0;
                }
                else
                {
                    if (sidetomove == side)
                    {
                        return -infinity - depth;
                    }
                    else
                    {
                        return infinity + depth;
                    }
                }
            }
            for (int i = 0; i < 32; i++)
            {
                if (piece_getpiece_feature(board, i, 3) != -1)
                {
                    var a= piece_getvalue(board, i);
                    if (a == 40)
                    {
                        d++;
                    }
                    if (piece_getpiece_feature(board, i, 4) == side)
                    {
                        ans += a;
                    }
                    else
                    {
                        ans -= a;
                    }
                }
            }
            if (d != 2)
            {
                var al = piece_getpiece_feature(board, 12, 3);
                var aal = piece_getpiece_feature(board, 28, 3);
                var aalk = piece_getpiece_feature(board, 28, 0);
            }
            else if (d == 2)
            {

            }

            return ans/10;
        }
        long blackside = 5464478908796543;
        long blackside1 = 4846648782131846;
        long whiteside = -251452145153462;
        long whiteside1 = -56847452462624;

        public long board_getkey(int board, int sidetomove, int sideh)
        {//very slow !! consumed around 30% of the time!!

            long key = 0;
            var side0 = 0;
            var side1 = 1;
            //  for (int i = 0; i < 8; i++)
            var exis = 0;
            for (int piece = 0; piece < 32; piece++)
            {
                var exists = piece_getpiece_feature(board, piece, 3);
                if (exists != -1)
                {
                    long keyy2 = 0;
                    var side = piece_getpiece_feature(board, piece, 4);
                    var col = piece_getpiece_feature(board, piece, 1) + 1;
                    var row = piece_getpiece_feature(board, piece, 2) + 1;
                    exis++;
                    if (side == 0)
                    {
                        side0++;
                    }
                    else
                    {
                        side1++;
                    }
                    if (col <= 0 || row <= 0)
                    {

                    }
                    switch (piece_getpiece_feature(board, piece, 0))
                    {
                        case 0:
                            keyy2 = zb.squares_pawn[col, row, side];
                            break;
                        case 1:
                            keyy2 = zb.squares_Rook[col, row, side];
                            break;
                        case 2:
                            keyy2 = zb.squares_Knight[col, row, side];
                            break;
                        case 3:
                            keyy2 = zb.squares_Bishop[col, row, side];
                            break;
                        case 4:
                            keyy2 = zb.squares_Queen[col, row, side];
                            break;
                        case 5:
                            keyy2 = zb.squares_king[col, row, side];
                            break;
                        default:
                            break;
                    }
                    key ^= keyy2;
                }
            }
            if (exis < 3)
            {

            }
            switch (sidetomove)
            {
                case 0:
                    key ^= blackside;
                    break;
                case 1:
                    key ^= whiteside;
                    break;
            }
            switch (sideh)
            {
                case 0:
                    key ^= blackside1;
                    break;
                case 1:
                    key ^= whiteside1;
                    break;
            }
            return key;
        }
        public void Board_init_board(int board)
        {
            board_resetallsquares(board);//this part is slow consumes around 40% of the time ##
            board_resetallmoves(board);
            board_setboardfeature(board, 1, 1);
            board_setboardfeature(board, 2, 1);
            board_setboardfeature(board, 3, 1);
            board_setboardfeature(board, 4, 1);
            board_setboardfeature(board, 5, 0);
            // return;
            for (int i = 0; i < 32; i++)
            {
                if (i < 8)
                {
                    piece_create_piece(board, i, 0, i, 1, 0);
                }
                else if (i < 16)
                {
                    if (i == 8)
                    {
                        piece_create_piece(board, i, 1, i - 8, 0, 0);//rook
                    }
                    else if (i == 9)
                    {
                        piece_create_piece(board, i, 2, i - 8, 0, 0);//knight
                    }
                    else if (i == 10)
                    {
                        piece_create_piece(board, i, 3, i - 8, 0, 0);//bishop
                    }
                    else if (i == 11)
                    {
                        piece_create_piece(board, i, 4, i - 8, 0, 0);//queen
                    }
                    else if (i == 12)
                    {
                        piece_create_piece(board, i, 5, i - 8, 0, 0);//king
                        check_setking(board, 0, i - 8, 0);
                    }
                    else if (i == 13)
                    {
                        piece_create_piece(board, i, 3, i - 8, 0, 0);//bishop
                    }
                    else if (i == 14)
                    {
                        piece_create_piece(board, i, 2, i - 8, 0, 0);//knight
                    }
                    else if (i == 15)
                    {
                        piece_create_piece(board, i, 1, i - 8, 0, 0);//rook
                    }
                }
                else if (i < 24)
                {
                    piece_create_piece(board, i, 0, i - 16, 6, 1);
                }
                else if (i < 32)
                {
                    if (i == 24)
                    {
                        piece_create_piece(board, i, 1, i - 24, 7, 1);//rook
                    }
                    else if (i == 25)
                    {
                        piece_create_piece(board, i, 2, i - 24, 7, 1);//knight
                    }
                    else if (i == 26)
                    {
                        piece_create_piece(board, i, 3, i - 24, 7, 1);//bishop
                    }
                    else if (i == 27)
                    {
                        piece_create_piece(board, i, 4, i - 24, 7, 1);//queen
                    }
                    else if (i == 28)
                    {
                        piece_create_piece(board, i, 5, i - 24, 7, 1);//king
                        check_setking(board, 1, i -24, 7);
                    }
                    else if (i == 29)
                    {
                        piece_create_piece(board, i, 3, i - 24, 7, 1);//bishop
                    }
                    else if (i == 30)
                    {
                        piece_create_piece(board, i, 2, i - 24, 7, 1);//knight
                    }
                    else if (i == 31)
                    {
                        piece_create_piece(board, i, 1, i - 24, 7, 1);//rook
                    }
                }
            }
        }
    }
}
