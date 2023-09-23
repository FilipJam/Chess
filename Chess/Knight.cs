using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FilipsChess
{
    internal class Knight : ChessPiece
    {
        public Knight(int y, int x, string pieceColor) : base(y, x, pieceColor)
        {
            value = 3;

            if (pieceColor == "white")
                Image = Asset.KnightImage[0];
            else
                Image = Asset.KnightImage[1];
        }

        public override void CalcMoves(bool analysisMode = false)
        {
            int[,] directions = { { 2, 1}, { 2,-1 }, { -2, 1 }, { -2, -1 }, { 1, 2 }, { -1, 2 }, { 1, -2 }, { -1, -2 } };
            Global.SearchAround(this, directions, KnightMoveLimits);
        }

        private bool KnightMoveLimits(int x, int y)
        {
            return x < 0 || x > 7 || y < 0 || y > 7;
        }
    }
}

