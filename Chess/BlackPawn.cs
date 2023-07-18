using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilipsChess
{
    internal class BlackPawn : Pawn
    {
        public BlackPawn(int y, int x, Bitmap image, string pieceColor) : base(y, x, image, pieceColor)
        {
            opponentIndex = 0;
            moveDirection = -1;
            startRow = 6;
            endRow = 0;
        }
    }
}
