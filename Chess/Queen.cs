using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilipsChess
{
    internal class Queen : ChessPiece
    {
        public Queen(int y, int x, string pieceColor) : base(y, x, pieceColor)
        {
            value = 9;

            if (pieceColor == "white")
                Image = Asset.QueenImage[0];
            else
                Image = Asset.QueenImage[1];
        }
        public override void CalcMoves(bool analysisMode = false)
        {
            int[,] directions = { { 1, 1 }, { 1, -1 }, { -1, 1 }, { -1, -1 }, { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
            Global.SearchPaths(this, directions, analysisMode);
        }
    }
}
