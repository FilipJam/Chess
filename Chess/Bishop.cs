using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FilipsChess
{
    internal class Bishop : ChessPiece
    {
        public Bishop(int y, int x, string pieceColor) : base(y, x, pieceColor)
        {
            value = 3;

            if (pieceColor == "white")
                Image = Asset.BishopImage[0];
            else
                Image = Asset.BishopImage[1];
        }
        public override void CalcMoves(bool analysisMode = false)
        {
            int[,] directions = { { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 } };
            Global.SearchPaths(this, directions, analysisMode);
        }
    }
}
