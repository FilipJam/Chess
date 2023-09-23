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
        public BlackPawn(int y, int x, string pieceColor) : base(y, x, pieceColor)
        {
            opponentIndex = 0;
            moveDirection = -1;
            startRow = 6;
            endRow = 0;

            Image = Asset.PawnImage[1];
        }
    }
}
