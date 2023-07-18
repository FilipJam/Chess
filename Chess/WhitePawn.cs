using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace FilipsChess
{
    internal class WhitePawn : Pawn
    {
        public WhitePawn(int y, int x, Bitmap image, string pieceColor) : base(y, x, image, pieceColor)
        {
            opponentIndex = 1;
            moveDirection = 1;
            startRow = 1;
            endRow = 7;
        }
    }
}
