using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FilipsChess
{
    internal class Rook : CastlePiece
    {
        public Rook(int y, int x, Bitmap image, string pieceColor) : base(y, x, image, pieceColor)
        {
            value = 5;
            moved = false;
        }

        public override void CalcMoves(bool analysisMode = false)
        {
            int[,] directions = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 } };
            Global.SearchPaths(this, directions, analysisMode);

        }
    }
}
