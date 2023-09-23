using FilipsChess.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FilipsChess
{
    internal abstract class ChessPiece
    {
        Bitmap image;
        private Point position;
        protected List<Point> moves;
        protected List<Point> captures;
        private List<ChessPiece> defenders;
        private ChessPiece pinningPiece;
        private List<Point> pinnedPath;
        protected int opponentIndex;
        protected int value;
        private string pieceColor;

        public ChessPiece(int y, int x, string pieceColor)
        {
            position = new Point(x, y);
            moves = new List<Point>();
            captures = new List<Point>();
            defenders = new List<ChessPiece>();
            pinnedPath = new List<Point>();
            this.pieceColor = pieceColor;
        }

        public int X { get => position.X; set => position.X = value; }
        public int Y { get => position.Y; set => position.Y = value; }
        public Bitmap Image { get => image; set => image = value; }
        public List<Point> Moves { get => moves; set => moves = value; }
        public List<Point> Captures { get => captures; set => captures = value; }

        public List<Point> TotalMoves { 
            get
            {
                List<Point> totalMoves = moves.GetRange(0, moves.Count);
                totalMoves.AddRange(captures);
                return totalMoves;
            }
        }
        public string Color { get => pieceColor; set => pieceColor = value; }
        internal List<ChessPiece> Defenders { get => defenders; set => defenders = value; }
        public Point Position { get => position; set => position = value; }
        public ChessPiece PinningPiece { get => pinningPiece; set => pinningPiece = value; }
        public List<Point> PinnedPath { get => pinnedPath; set => pinnedPath = value; }
        public int Value { get => value; }

        public void ShowMoves()
        {
            ClearMoves();
            CalcMoves();

            if (MeetMoveRequirements())
            {
                moves.ForEach(m => Global.board[m.Y, m.X].BackColor = Global.moveColor);
                captures.ForEach(c => Global.board[c.Y, c.X].BackColor = Global.enemyColor);
            }
            

        }

        public bool MeetMoveRequirements()
        {
            // King's functionality already handles movement restriction
            if (this is King)
                return true;

            List<Point> capturesCopy = captures.GetRange(0, captures.Count);
            List<Point> movesCopy = Moves.GetRange(0, moves.Count);

            // impossible to move
            if (Global.checkingPieces.Count == 2 || (Global.checkingPieces.Count == 1 && pinningPiece != null))
                ClearMoves();
            // restricting movement only to eliminating check
            else if (Global.checkingPieces.Count == 1)
            {
                ClearMoves();
                FindCheckingPiece(capturesCopy);
                BlockCheck(movesCopy);
            }
            // restricting pinned piece to only allowing eliminating the pinning piece
            else if (pinningPiece != null)
                HandlePinnedMovement(capturesCopy);
            return true;
        }

        private void BlockCheck(List<Point> movesCopy)
        {
            foreach (var pos in movesCopy)
            {
                if (Global.checkingPath.Contains(pos))
                {
                    moves.Add(pos);
                    Global.canResolveCheck = true;
                }
            }
        }

        private void FindCheckingPiece(List<Point> capturesCopy)
        {
            foreach (var pos in capturesCopy)
            {
                if (Global.checkingPieces[0] == Global.chessPieces[pos.Y, pos.X])
                {
                    captures.Add(pos);
                    Global.canResolveCheck = true;
                }
            }
        }

        private void HandlePinnedMovement(List<Point> capturesCopy)
        {
            foreach (Point pos in Moves.GetRange(0, Moves.Count))
                if (!PinnedPath.Contains(pos))
                    Moves.Remove(pos);

            captures.Clear();
            if (capturesCopy.Contains(pinningPiece.Position))
                captures.Add(pinningPiece.Position);
        }

        public virtual void ClearMoves()
        {
            moves.Clear();
            captures.Clear();
        }

        public abstract void CalcMoves(bool analysisMode = false);


    }
}
