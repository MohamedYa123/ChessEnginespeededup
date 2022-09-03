using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fastChessEngine
{
    partial class ThinkerPro
    {
        //now moves
        const int totalmovesFeatures = 10;
        int[] moves;
        public void setmoves()
        {
            //assign pieces
            moves = new int[totalnumberassign * 200 * totalmovesFeatures];
            //castle,longmove,promotion,capturevalue,x1,y1,x2,y2,piece,part2
        }
        public string move_to_string(int board,int move)
        {
            string ans = "";
            string mkv = "";
            //get piece id
            var piece = move_getmove_feature(board, move, 8);
            if (piece == -1)
            {
                return "";
            }
            var type = piece_getpiece_feature(board, piece, 0);
            var x1 = move_getmove_feature(board, move, 4);
            var y1 = move_getmove_feature(board, move, 5);
            var promotion = move_getmove_feature(board, move, 2);
            switch (type)
            {
                case 0:
                    ans= "P";
                    break;
                case 1:
                    ans= "R";
                    break;
                case 2:
                    ans= "N";
                    break;
                case 3:
                    ans= "B";
                    break;
                case 4:
                    ans= "Q";
                    break;
                case 5:
                    ans= "K";
                    break;
            }
            switch (x1)
            {
                case 0:
                    mkv = "a";
                    break;
                case 1:
                    mkv = "b";
                    break;
                case 2:
                    mkv = "c";
                    break;
                case 3:
                    mkv = "d";
                    break;
                case 4:
                    mkv = "e";
                    break;
                case 5:
                    mkv = "f";
                    break;
                case 6:
                    mkv = "g";
                    break;
                case 7:
                    mkv = "h";
                    break;
            }
            var oc = square_getsquare_feature(board, x1, y1, 3) == 1;
            if ((move_getmove_feature(board, move, 9) != -1 && move_getmove_feature(board, move, 0)==-1) ||square_getsquare_feature(board,x1,y1,2)==1||  oc)
            {
                ans += "x";
            }
            ans += mkv + (y1+1);
            if (promotion > 0)
            {
                ans += "=";
                switch (promotion)
                {
                    case 1:
                        ans += "R";
                        break;
                    case 2:
                        ans+= "N";
                        break;
                    case 3:
                        ans += "B";
                        break;
                    case 4:
                        ans += "Q";
                        break;
                    case 5:
                        ans += "K";
                        break;
                }
                
            }
            return ans;
        }
         void move_createmove(int board ,int move,int castle,int longmove,int promotion,int x1,int y1,int x2,int y2,int piece,int part2,bool kingmove,bool pawn,ref int pointer,int side, int pawnside = 0)
        {
            int numpieces = check_getcheckfeature(board, side, 0);
            if (!kingmove)
            {
                
                if (numpieces == 2)
                {
                    return;
                }
                else if (numpieces == 1)
                {
                    if (!check_matchmove(board, side, x1, y1))
                    {
                        return ;
                    }
                }
            }
            if(kingmove&&x2!=-1&& numpieces != 0)
            {
                return;
            }
            if (pawn && (pawnside == 0 && y1 == 7) || (pawnside == 1 && y1 == 0))
            {
                for(int i = 4; i >0; i--)
                {
                    int pointer11 = board * 200 * totalmovesFeatures + move * totalmovesFeatures;
                    moves[pointer11] = castle;
                    moves[pointer11 + 1] = longmove;
                    moves[pointer11 + 2] = i;
                    moves[pointer11 + 4] = x1;
                    moves[pointer11 + 5] = y1;
                    moves[pointer11 + 6] = x2;
                    moves[pointer11 + 7] = y2;
                    moves[pointer11 + 8] = piece;
                    moves[pointer11 + 9] = part2;
                    board_increasepointer(board);
                    pointer++;
                    move++;
                }
            }
            else
            {
                int pointer11 = board * 200 * totalmovesFeatures + move * totalmovesFeatures;
                moves[pointer11] = castle;
                moves[pointer11 + 1] = longmove;
                moves[pointer11 + 2] = promotion;
                moves[pointer11 + 4] = x1;
                moves[pointer11 + 5] = y1;
                moves[pointer11 + 6] = x2;
                moves[pointer11 + 7] = y2;
                moves[pointer11 + 8] = piece;
                moves[pointer11 + 9] = part2;
                board_increasepointer(board);
                pointer++;
            }
        }
        public void move_aplymove(int board,int move,int boardfrom=-1)
        {
            int pointer = 0;
            if (boardfrom == -1)
            {
                 pointer = board * 200 * totalmovesFeatures + move * totalmovesFeatures;
            }
            else
            {
                 pointer = boardfrom * 200 * totalmovesFeatures + move * totalmovesFeatures;
            }
            var piece = moves[pointer + 8];
            var part2 = moves[pointer + 9];
            var col = moves[pointer + 4];
            var row = moves[pointer + 5];
            var longmove = moves[pointer + 1];
            if (piece_getpiece_feature(0, piece, 0) == 5)
            {
                check_setking(board, piece_getpiece_feature(0, piece, 4), col, row);
            }
            var col2 = moves[pointer + 6];
            var promotion = moves[pointer + 2];
            if(part2!=-1 && col2 == -1)
            {
                piece_removepiece(board, part2);
            }
            else if(part2!=-1)
            {
                var row2 = moves[pointer + 7];
                pieceChangePosition(board, part2, col2, row2);
            }
            if (promotion > 0)
            {
                piece_setpiece_feature(board, piece, 0, promotion);
            }
            pieceChangePosition(board, piece, col, row);
            board_resetlongmovefirstpieces(board);
            piece_setpiece_feature(board, piece, 6, longmove);
        }
        /// <summary>
        /// 0=castle,1=longmove,2=promotion,3=capturevalue,4=x1,5=y1,6=x2,7=y2,8=piece,9=part2
        /// </summary>
         int move_getmove_feature(int board,int move,int feature)
        {
            return moves[board * 200 * totalmovesFeatures + move * totalmovesFeatures + feature];
        }
        /// <summary>
        /// 0=castle,1=longmove,2=promotion,3=capturevalue,4=x1,5=y1,6=x2,7=y2,8=piece,9=part2
        /// </summary>
         void move_setmove_feature(int board,int move,int feature,int value)
        {
             moves[board * 200 * totalmovesFeatures + move * totalmovesFeatures + feature]=value;
        }
    }
}
