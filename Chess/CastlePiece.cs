using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilipsChess
{
    abstract class CastlePiece : ChessPiece
    {
        protected bool moved;
        public CastlePiece(int y, int x, string pieceColor) : base(y, x, pieceColor)
        {
            moved = false;
        }

        public bool Moved { get => moved; set => moved = value; }

    }
}
