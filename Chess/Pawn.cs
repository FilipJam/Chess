using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilipsChess
{
    internal class Pawn : ChessPiece
    {
        protected int moveDirection;
        protected int startRow;
        protected int endRow;
        private List<Point> potentialCaptures;

        public List<Point> PotentialCaptures { get => potentialCaptures; set => potentialCaptures = value; }
        public int EndRow { get => endRow; set => endRow = value; }

        public Pawn(int y, int x, Bitmap image, string pieceColor) : base(y, x, image, pieceColor)
        {
            value = 1;
            potentialCaptures= new List<Point>();
        }

        public override void CalcMoves(bool analysisMode = false)
        {
            ClearMoves();
            Point move1 = new Point(X, Y + moveDirection);
            Point move2 = new Point(X, Y + moveDirection*2);

            if (Global.chessPieces[move1.Y, move1.X] == null)
            {
                moves.Add(move1);
                if (Y == startRow && Global.chessPieces[move2.Y, move2.X] == null)
                    moves.Add(move2);
            }

            Point[] captureDirections = DefineCaptureDirections();
            FindEnemies(captureDirections);
        }

        private void FindEnemies(Point[] captureDirections)
        {
            for (int i = 0; i < captureDirections.Length; i++)
            {
                Point capturePos = new Point(X + captureDirections[i].X, Y + captureDirections[i].Y);
                potentialCaptures.Add(capturePos);
                ChessPiece potentialEnemy = Global.chessPieces[capturePos.Y, capturePos.X];

                if (potentialEnemy != null && potentialEnemy.Color != Color)
                {
                    captures.Add(capturePos);
                }
                else if (potentialEnemy != null && potentialEnemy.Color == Color)
                    potentialEnemy.Defenders.Add(this);

                FindEnPassant(capturePos);
            }
        }

        private void FindEnPassant(Point capturePos)
        {
            if (Global.passingPawn == null)
                return;
            Point pawnPassingPos = new Point(capturePos.X, Y);
            if (pawnPassingPos.X == Global.passingPawn.X && pawnPassingPos.Y == Global.passingPawn.Y)
            {
                Global.enPassantCapture = new Point(capturePos.X, capturePos.Y);
                captures.Add(Global.enPassantCapture);
            }


        }

        protected virtual Point[] DefineCaptureDirections()
        {
            switch (X)
            {
                case 0: return new Point[] { new Point(1, moveDirection) };
                case 7: return new Point[] { new Point(-1, moveDirection) };
                default: return new Point[] { new Point(1, moveDirection), new Point(-1, moveDirection) };
            }
        }

        public override void ClearMoves()
        {
            base.ClearMoves();
            potentialCaptures.Clear();
        }
    }
}
