using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilipsChess
{
    internal class King : CastlePiece
    {
        private bool inCheck;

        public King(int y, int x, string color) : base(y, x, color) 
        {
            value = 5;
            inCheck = false;

            if (color == "white")
                Image = Asset.KingImage[0];
            else
                Image = Asset.KingImage[1];
        }

        public bool InCheck { get => inCheck; set => inCheck = value; }
        public int[] CastlePositions { get => new int[] { 2, 6 }; }

        public override void CalcMoves(bool analysisMode = false)
        {
            int[,] directions = { { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 }, { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
            Global.SearchAround(this, directions, KingMoveLimits);
            CalcCastleMoves();
        }

        private void CalcCastleMoves()
        {
            if (moved || InCheck)
                return;

            for (int i = 0; i < 2; i++)
            {
                Rook rook = Global.chessPieces[Y, i * 7] as Rook;
                if (rook != null && !rook.Moved && Castle.Availability[i])
                    moves.Add(new Point(CastlePositions[i], Y));
            }
        }

        private bool KingMoveLimits(int x, int y)
        {
            return x < 0 || x > 7 || y < 0 || y > 7 || ((List<Point>)Global.blockedKingMoves[Color]).Contains(new Point(x, y));
        }
    }

    
}
