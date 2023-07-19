using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilipsChess
{
    internal static class Global
    {
        public static Button[,] board = new Button[8,8];
        public static ChessPiece[,] chessPieces = new ChessPiece[8, 8];
        public static Bitmap emptyImage;
        //public static ChessPiece selectedPiece;
        public static King kingInCheck;
        public static Pawn passingPawn;
        public static Point enPassantCapture = Point.Empty;

        public static List<ChessPiece> checkingPieces = new List<ChessPiece>();
        public static List<Point> checkingPath = new List<Point>();

        public static bool canResolveCheck = false;

        public static Hashtable blockedKingMoves = new Hashtable()
        {
            { "white", new List<Point>() },
            { "black", new List<Point>() }
        };

        public static Color lightColor = Color.NavajoWhite;
        public static Color darkColor = Color.FromArgb(64, 0, 0);
        public static Color selectColor = Color.DarkViolet;
        public static Color moveColor = Color.SkyBlue;
        public static Color enemyColor = Color.DarkRed;
        public static Color checkColor = Color.DarkOrange;
        public static Color checkmateColor = Color.Gold;
        public static Color pinColor = Color.LimeGreen;

        public static void SearchPaths(ChessPiece piece, int[,] directions, bool analysisMode)
        {
            piece.ClearMoves();
            for (int i = 0; i < directions.Length / 2; i++)
            {
                
                int newX = piece.X;
                int newY = piece.Y;
                ChessPiece potentialPinnedPiece = null;
                List<Point> pinnedPath = new List<Point>();
                while ((newX += directions[i, 0]) >= 0 && newX <= 7 && (newY += directions[i, 1]) >= 0 && newY <= 7)
                    if (chessPieces[newY, newX] == null)
                    {
                        Point newMove = new Point(newX, newY);
                        if(potentialPinnedPiece == null)
                            piece.Moves.Add(newMove);
                        pinnedPath.Add(newMove);
                    }
                    else if (chessPieces[newY, newX].Color != piece.Color)
                    {
                        if (potentialPinnedPiece == null)
                        {
                            piece.Captures.Add(new Point(newX, newY));
                            if(chessPieces[newY, newX] is King)
                                checkingPath.AddRange(pinnedPath.GetRange(0, pinnedPath.Count));
                            else if (analysisMode)
                                potentialPinnedPiece = chessPieces[newY, newX];
                            else
                                break;
                        }
                        else if (chessPieces[newY, newX] is King)
                        {
                            potentialPinnedPiece.PinningPiece = piece;
                            potentialPinnedPiece.PinnedPath = pinnedPath.GetRange(0, pinnedPath.Count);
                            board[potentialPinnedPiece.Y, potentialPinnedPiece.X].BackColor = pinColor;
                            break;
                        }
                        else
                            break;
                    }
                    else if (chessPieces[newY, newX].Color == piece.Color)
                    {
                        chessPieces[newY, newX].Defenders.Add(piece);
                        break;
                    }
                        
            }
        }

        private static bool CalculatePin(int y, int x, ChessPiece potentialPinnedPiece, List<Point> pinnedPath, ChessPiece piece, bool analysisMode)
        {
            if (potentialPinnedPiece == null && chessPieces[y, x] is King)
            {
                checkingPath.AddRange(pinnedPath.GetRange(0, pinnedPath.Count));
                piece.Captures.Add(new Point(x, y));
            }
            else if (potentialPinnedPiece == null)
            {
                piece.Captures.Add(new Point(x, y));
                if (analysisMode)
                {
                    potentialPinnedPiece = chessPieces[y, x];
                    return false;
                }
                    
            }
            else if (chessPieces[y, x] is King)
            {
                potentialPinnedPiece.PinningPiece = piece;
                potentialPinnedPiece.PinnedPath = pinnedPath.GetRange(0, pinnedPath.Count);
                board[potentialPinnedPiece.Y, potentialPinnedPiece.X].BackColor = checkmateColor;
            }
            return true;
        }

        public delegate bool ExitPoint(int x, int y);

        public static void SearchAround(ChessPiece piece, int[,] directions, ExitPoint exitPoint)
        {
            piece.ClearMoves();
            for (int i = 0; i < directions.Length / 2; i++)
            {
                int newX = piece.X + directions[i, 0];
                int newY = piece.Y + directions[i, 1];
                if (exitPoint(newX, newY) == true)
                    continue;

                if (chessPieces[newY, newX] == null)
                    piece.Moves.Add(new Point(newX, newY));
                else if (chessPieces[newY, newX].Color != piece.Color && (chessPieces[newY, newX].Defenders.Count == 0 || !(piece is King)))
                    piece.Captures.Add(new Point(newX, newY));
                else if (chessPieces[newY, newX].Color == piece.Color)
                    chessPieces[newY, newX].Defenders.Add(piece);
            }
        }
    }
}
