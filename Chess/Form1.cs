using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FilipsChess
{
    public partial class Board : Form
    {
        public Board()
        {
            InitializeComponent();
        }

        string playerColor = "white";
        bool gameOver = false;
        State currentChessState;

        Label lblWhitePlayer = new Label();
        Label lblBlackPlayer = new Label();

        Bitmap empty;
        Bitmap[] pawn = new Bitmap[2]; Bitmap[] rook = new Bitmap[2];
        Bitmap[] knight = new Bitmap[2]; Bitmap[] bishop = new Bitmap[2];
        Bitmap[] queen = new Bitmap[2]; Bitmap[] king = new Bitmap[2];

        Color lightColor = Color.NavajoWhite;
        Color darkColor = Color.FromArgb(64, 0, 0);
        Color selectC = Color.DarkViolet;
        Color moveC = Color.SkyBlue;
        Color enemyC = Color.DarkRed;
        Color checkC = Color.DarkOrange;
        Color checkmateC = Color.Gold;

        // Computer info
        List<ChessPiece> blackPieces;
        Point bestMove;
        ChessPiece bestPiece;

        private delegate void ArrayElementAction(int y, int x);

        private void ExecuteAcrossGrid(ArrayElementAction action, int startY = 0, int endY = 8, int startX=0, int endX=8)
        {
            for (int y = startY; y < endY; y++)
                for (int x = startX; x < endX; x++)
                    action(y,x);
        }


        private void Board_Load(object sender, EventArgs e)
        {
            CreatePlayerLabel(lblWhitePlayer, new Point(0, 0), "White Turn", Color.White, Color.Black);
            CreatePlayerLabel(lblBlackPlayer, new Point(316, 0), "Black Turn", Color.Black, Color.Black);

            //MessageBox.Show("on Load: " + Convert.ToString(kingMoves[0].Length));
            Left = 350; Top = 50;

            empty = new Bitmap(btn17.Image);
            Global.emptyImage = empty;

            pawn[0] = new Bitmap(btn9.Image); pawn[1] = new Bitmap(btn50.Image);
            rook[0] = new Bitmap(btn1.Image); rook[1] = new Bitmap(btn64.Image);
            knight[0] = new Bitmap(btn2.Image); knight[1] = new Bitmap(btn63.Image);
            bishop[0] = new Bitmap(btn3.Image); bishop[1] = new Bitmap(btn62.Image);
            queen[0] = new Bitmap(btn4.Image); queen[1] = new Bitmap(btn60.Image);
            king[0] = new Bitmap(btn5.Image); king[1] = new Bitmap(btn61.Image);

            Global.board[0, 0] = btn1; Global.board[0, 1] = btn2; Global.board[0, 2] = btn3; Global.board[0, 3] = btn4;
            Global.board[0, 4] = btn5; Global.board[0, 5] = btn6; Global.board[0, 6] = btn7; Global.board[0, 7] = btn8;

            Global.board[1, 0] = btn9; Global.board[1, 1] = btn10; Global.board[1, 2] = btn11; Global.board[1, 3] = btn12;
            Global.board[1, 4] = btn13; Global.board[1, 5] = btn14; Global.board[1, 6] = btn15; Global.board[1, 7] = btn16;

            Global.board[2, 0] = btn17; Global.board[2, 1] = btn18; Global.board[2, 2] = btn19; Global.board[2, 3] = btn20;
            Global.board[2, 4] = btn21; Global.board[2, 5] = btn22; Global.board[2, 6] = btn23; Global.board[2, 7] = btn24;

            Global.board[3, 0] = btn25; Global.board[3, 1] = btn26; Global.board[3, 2] = btn27; Global.board[3, 3] = btn28;
            Global.board[3, 4] = btn29; Global.board[3, 5] = btn30; Global.board[3, 6] = btn31; Global.board[3, 7] = btn32;

            Global.board[4, 0] = btn33; Global.board[4, 1] = btn34; Global.board[4, 2] = btn35; Global.board[4, 3] = btn36;
            Global.board[4, 4] = btn37; Global.board[4, 5] = btn38; Global.board[4, 6] = btn39; Global.board[4, 7] = btn40;

            Global.board[5, 0] = btn41; Global.board[5, 1] = btn42; Global.board[5, 2] = btn43; Global.board[5, 3] = btn44;
            Global.board[5, 4] = btn45; Global.board[5, 5] = btn46; Global.board[5, 6] = btn47; Global.board[5, 7] = btn48;

            Global.board[6, 0] = btn49; Global.board[6, 1] = btn50; Global.board[6, 2] = btn51; Global.board[6, 3] = btn52;
            Global.board[6, 4] = btn53; Global.board[6, 5] = btn54; Global.board[6, 6] = btn55; Global.board[6, 7] = btn56;

            Global.board[7, 0] = btn57; Global.board[7, 1] = btn58; Global.board[7, 2] = btn59; Global.board[7, 3] = btn60;
            Global.board[7, 4] = btn61; Global.board[7, 5] = btn62; Global.board[7, 6] = btn63; Global.board[7, 7] = btn64;

            BoardSetup();
        }

        private void CreatePlayerLabel(Label lbl, Point labelPos, string labelText, Color labelBackColor, Color labelFontColor)
        {
            lbl.Size = new Size(317, 50);
            lbl.Location = labelPos;
            lbl.Text = labelText;
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Font = new Font(FontFamily.GenericSansSerif, 20, FontStyle.Bold);
            lbl.BorderStyle = BorderStyle.FixedSingle;
            lbl.BackColor = labelBackColor;
            lbl.ForeColor = labelFontColor;

            Controls.Add(lbl);
        }

        

        private void BoardSetup()
        {
            currentChessState = new State("white");
            ResetStatus();
            ResetColours();
            ExecuteAcrossGrid((y, x) => Global.chessPieces[y, x] = null);
            ExecuteAcrossGrid((y, x) => Global.chessPieces[y, x] = new WhitePawn(y, x, pawn[0], "white"), startY: 1, endY: 2);
            ExecuteAcrossGrid((y, x) => Global.chessPieces[y, x] = new BlackPawn(y, x, pawn[1], "black"), startY: 6, endY: 7);

            //rooks
            Global.chessPieces[0, 0] = new Rook(0, 0, rook[0], "white");
            Global.chessPieces[0, 7] = new Rook(0, 7, rook[0], "white");
            Global.chessPieces[7, 0] = new Rook(7, 0, rook[1], "black");
            Global.chessPieces[7, 7] = new Rook(7, 7, rook[1], "black");

            //knights
            Global.chessPieces[0, 1] = new Knight(0, 1, knight[0], "white");
            Global.chessPieces[0, 6] = new Knight(0, 6, knight[0], "white");
            Global.chessPieces[7, 1] = new Knight(7, 1, knight[1], "black");
            Global.chessPieces[7, 6] = new Knight(7, 6, knight[1], "black");

            //bishops
            Global.chessPieces[0, 2] = new Bishop(0, 2, bishop[0], "white");
            Global.chessPieces[0, 5] = new Bishop(0, 5, bishop[0], "white");
            Global.chessPieces[7, 2] = new Bishop(7, 2, bishop[1], "black");
            Global.chessPieces[7, 5] = new Bishop(7, 5, bishop[1], "black");

            //queens
            Global.chessPieces[0, 3] = new Queen(0, 3, queen[0], "white");
            Global.chessPieces[7, 3] = new Queen(7, 3, queen[1], "black");

            //kings
            Global.chessPieces[0, 4] = new King(0, 4, king[0], "white");
            Global.chessPieces[7, 4] = new King(7, 4, king[1], "black");

            PopulateState();
            LoadState();
        }

        private void PopulateState() => ExecuteAcrossGrid((y, x) => currentChessState.ChessPieces[y, x] = Global.chessPieces[y, x]);

        private void LoadState()
        {
            ExecuteAcrossGrid((y, x) =>
            {
                Global.chessPieces[y, x] = currentChessState.ChessPieces[y, x];
                if (Global.chessPieces[y, x] == null)
                    return;
                Global.chessPieces[y, x].Y = y;
                Global.chessPieces[y, x].X = x;
            });
            playerColor = currentChessState.PlayerColor;
            gameOver = currentChessState.GameOver;

            blackPieces = new List<ChessPiece>();
            ExecuteAcrossGrid((y, x) =>
            {
                if (Global.chessPieces[y, x] == null)
                    Global.board[y, x].Image = Global.emptyImage;
                else
                {
                    if (Global.chessPieces[y, x].Color == "black")
                        blackPieces.Add(Global.chessPieces[y, x]);
                    Global.board[y, x].Image = Global.chessPieces[y, x].Image;
                }
            });

            UpdateTurnLabels();
            ResetStatus();
            ResetColours();
            AnalyseBoard();
            UpdateStateButtons();
        }

        private void UpdateStateButtons()
        {
            btnPrev.Enabled = true;
            btnPrev.BackColor = Color.Tomato;
            btnNext.Enabled = true;
            btnNext.BackColor = Color.PaleGreen;

            if (currentChessState.Prev == null)
            {
                btnPrev.Enabled = false;
                btnPrev.BackColor = Color.White;
            }
                
            if (currentChessState.Next == null)
            {
                btnNext.Enabled = false;
                btnNext.BackColor = Color.White;
            }
                
        }

        void ResetColours()
        {
            ExecuteAcrossGrid((y, x) =>
            {
                if (Global.kingInCheck != null && Global.chessPieces[y, x] == Global.kingInCheck)
                {
                    Global.board[y, x].BackColor = Global.checkColor;
                }
                else if (Global.chessPieces[y, x] != null && Global.chessPieces[y, x].PinningPiece != null)
                {
                    Global.board[y, x].BackColor = Global.pinColor;
                }
                else
                {
                    // shift = { 0 if even, 1 if odd }
                    int shift = y % 2;
                    // shifts by 1 if row is odd
                    if ((x + shift) % 2 == 0)
                        Global.board[y, x].BackColor = lightColor;
                    else
                        Global.board[y, x].BackColor = darkColor;
                }
            });

        }

        private void AddUniquePoint(List<Point> positionList, Point pos)
        {
            if (!positionList.Contains(pos))
            {
                positionList.Add(pos);
            }
        }

        void AnalyseBoard()
        {
            void FindKingAroundPiece(List<Point> moves, string pieceColor)
            {
                int[,] directions = { { 1, 0 }, { -1, 0 }, { 0, 1 }, { 0, -1 }, { 1, 1 }, { -1, 1 }, { 1, -1 }, { -1, -1 } };
                foreach (Point pos in moves)
                    for (int i = 0; i < 8; i++)
                    {
                        int newX = pos.X + directions[i, 0];
                        int newY = pos.Y + directions[i, 1];
                        if (newX < 0 || newX > 7 || newY < 0 || newY > 7)
                            continue;

                        ChessPiece opponentPiece = Global.chessPieces[newY, newX];
                        if (opponentPiece != null && opponentPiece.Color != pieceColor && opponentPiece is King)
                        {
                            //MessageBox.Show(string.Format("Blocked king move at: {0},{1}", pos.Y, pos.X));
                            AddUniquePoint((List<Point>)Global.blockedKingMoves[opponentPiece.Color], pos);
                            break;
                        }
                    }
            }

            List<King> kingList = new List<King>();
            bool movesAvailable = false;
            void Analyze(ChessPiece piece)
            {
                if (piece == null)
                    return;

                piece.CalcMoves(analysisMode: true);
                if (piece is King)
                    kingList.Add((King)piece);

                if (piece.Color != playerColor && !FindCheck(piece))
                {
                    UpdateCastlingOptions(CheckCastleClearance, piece);
                    UpdateCastlingOptions(CheckCastleBlock, piece);
                }

                if (piece is Pawn)
                    FindKingAroundPiece(((Pawn)piece).PotentialCaptures, piece.Color);
                else
                    FindKingAroundPiece(piece.Moves, piece.Color);

                if (piece.Color == playerColor && piece.TotalMoves.Count > 0)
                    movesAvailable = true;

                piece.ClearMoves();
            }




            ExecuteAcrossGrid((y, x) => Analyze(Global.chessPieces[y, x]));
            FindCheckMate(kingList);
            FindStaleMate(movesAvailable);
        }

        private void FindStaleMate(bool movesAvailable)
        {
            if (!movesAvailable && !gameOver)
                Stalemate();
        }

        private void FindCheckMate(List<King> kingList)
        {
            if (Global.checkingPieces.Count == 0)
                return;

            foreach (King king in kingList)
            {
                if (!king.InCheck)
                    continue;

                king.CalcMoves();

                if ((king.Moves.Count + king.Captures.Count) == 0 && NoHelp(king.Color))
                {
                    Checkmate(king.Color);
                    break;
                }
            }
        }

        private void Stalemate()
        {
            gameOver = true;
            MessageBox.Show("Stalemate. Unfortunate Draw :(");
        }

        private delegate void BlockCastleCheck(ChessPiece piece, int rangeIndex, int y);
        private void UpdateCastlingOptions(BlockCastleCheck updateOptions, ChessPiece piece)
        {
            for (int i = 0; i < 2; i++)
            {
                if (!Castle.Availability[i])
                    continue;

                int opponentStartY = 0;
                if (piece.Color == "white")
                    opponentStartY = 7;

                updateOptions(piece, i, opponentStartY);
            }
        }

        private void CheckCastleBlock(ChessPiece piece, int rangeIndex, int y)
        {
            for (int x = Castle.RangeList[rangeIndex][0]; x <= Castle.RangeList[rangeIndex][1]; x++)
            {
                if (piece.TotalMoves.Contains(new Point(x, y)))
                {
                    Castle.Availability[rangeIndex] = false;
                    break;
                }
            }
        }

        private void CheckCastleClearance(ChessPiece piece, int rangeIndex, int y)
        {
            for (int x = Castle.RangeList[rangeIndex][0] + 1; x <= Castle.RangeList[rangeIndex][1] - 1; x++)
            {
                if (Global.chessPieces[y, x] != null)
                {
                    Castle.Availability[rangeIndex] = false;
                    break;
                }
            }
        }


        private void Checkmate(string checkmatedKingColor)
        {
            string winningColor;
            switch (checkmatedKingColor)
            {
                case "black": winningColor = "white"; break;
                default: winningColor = "black"; break;
            }

            MessageBox.Show("Checkmate! " + winningColor + " Wins!");
            gameOver = true;
        }

        private bool NoHelp(string kingColor)
        {
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                {
                    ChessPiece piece = Global.chessPieces[y, x];
                    if (piece != null && piece.Color == kingColor)
                    {
                        piece.CalcMoves();
                        piece.MeetMoveRequirements();
                        if (Global.canResolveCheck)
                            return false;
                        piece.ClearMoves();
                    }
                }
            return true;
        }

        private bool FindCheck(ChessPiece piece)
        {
            foreach(Point pos in piece.Captures)
            {
                King king = Global.chessPieces[pos.Y, pos.X] as King;
                if (king != null)
                {
                    Global.checkingPieces.Add(piece);
                    Global.kingInCheck = king;
                    king.InCheck = true;
                    Global.board[pos.Y, pos.X].BackColor = Global.checkColor;
                    return true; ;
                }
            }
            return false;
        }

        void SwitchPlayer()
        {
            switch (playerColor)
            {
                case "white":  playerColor = "black"; break;
                case "black":  playerColor = "white"; break;
            }
            UpdateTurnLabels();
        }

        private void UpdateTurnLabels()
        {
            switch (playerColor)
            {
                case "white":
                    lblWhitePlayer.ForeColor = Color.Black;
                    lblBlackPlayer.ForeColor = Color.Black;
                    break;
                case "black":
                    lblWhitePlayer.ForeColor = Color.White;
                    lblBlackPlayer.ForeColor = Color.White;
                    break;
            }
        }

        private void ResetStatus()
        {
            ((List<Point>)Global.blockedKingMoves["white"]).Clear();
            ((List<Point>)Global.blockedKingMoves["black"]).Clear();

            Global.checkingPieces.Clear();
            Global.checkingPath.Clear();
            Global.canResolveCheck = false;
            if(Global.kingInCheck != null)
            {
                Global.kingInCheck.InCheck = false;
                Global.kingInCheck = null;
            }
            Castle.Availability[0] = true;
            Castle.Availability[1] = true;

            Global.passingPawn = null;
            Global.enPassantCapture = Point.Empty;

            bestPiece = null;
            
            ExecuteAcrossGrid((y, x) =>
            {
                if (Global.chessPieces[y, x] != null)
                {
                    Global.chessPieces[y, x].Defenders.Clear();
                    Global.chessPieces[y, x].PinningPiece = null;
                    Global.chessPieces[y, x].PinnedPath.Clear();
                }
            });
        }

        private bool CheckPawnPromotion()
        {
            Pawn pawn = Global.selectedPiece as Pawn;
            if (pawn == null)
                return false;

            if(pawn.Y == pawn.EndRow)
            {
                Button btn = Global.board[Global.selectedPiece.Y, Global.selectedPiece.X];
                Point pos = new Point(btn.Left + Left, btn.Top + Top + 30);

                menuPromotion.Show(pos);
                menuPromotion.AutoClose = false;
                return true;
            }
            return false;
        }

        private void EndTurn()
        {
            Global.selectedPiece = null;
            SwitchPlayer();
            CreateNewState();
            
            AnalyseBoard();

            StartComputerTurn();
        }

        private void CreateNewState()
        {
            State nextState = new State(playerColor);

            currentChessState.Next = nextState;
            nextState.Prev = currentChessState;
            currentChessState = nextState;

            PopulateState();
            UpdateStateButtons();
        }

        


        private void NextState(object sender, EventArgs e)
        {
            currentChessState = currentChessState.Next;
            LoadState();
        }

        private void PreviousState(object sender, EventArgs e)
        {
            currentChessState = currentChessState.Prev;
            LoadState();
        }

        private void Chess(object sender, EventArgs e)
        {
            
            string tag = ((Button)sender).Tag.ToString();
            int y = int.Parse(tag[0].ToString());
            int x = int.Parse(tag[1].ToString());
            ChessPiece chessPiece = Global.chessPieces[y, x];
            Button square = Global.board[y, x];

            ActivateSquare(y, x);
        }

        private void ActivateSquare(int y, int x)
        {
            if (gameOver)
                return;
            // deselect piece
            if (Global.selectedPiece != null && Global.selectedPiece.X == x && Global.selectedPiece.Y == y)
            {
                Global.selectedPiece.ClearMoves();
                Global.selectedPiece = null;
                ResetColours();
            }
            // select if it is player piece
            else if (Global.chessPieces[y, x] != null && Global.chessPieces[y, x].Color == playerColor)
            {
                if (Global.selectedPiece != null)
                    Global.selectedPiece.ClearMoves();
                ResetColours();
                Global.selectedPiece = Global.chessPieces[y, x];
                Global.board[y, x].BackColor = Global.selectColor;
                Global.selectedPiece.ShowMoves();
            }
            // move piece
            else if (Global.board[y, x].BackColor == Global.moveColor || Global.board[y, x].BackColor == Global.enemyColor)
            {
                CheckCastleRequirements();
                UpdateBlackPieces(y, x);
                CheckForEnPassant(y, x);

                Global.chessPieces[Global.selectedPiece.Y, Global.selectedPiece.X] = null;
                Global.chessPieces[y, x] = Global.selectedPiece;
                Global.board[Global.selectedPiece.Y, Global.selectedPiece.X].Image = Global.emptyImage;
                Global.board[y, x].Image = Global.selectedPiece.Image;

                if (Global.selectedPiece is King)
                    CheckCastling(x);

                ResetStatus();
                ResetColours();

                CheckForPassingPawn(y);

                Global.selectedPiece.X = x;
                Global.selectedPiece.Y = y;

                if (!CheckPawnPromotion())
                    EndTurn();
            }
        }

        private void UpdateBlackPieces(int y, int x)
        {
            if (playerColor == "white" && Global.chessPieces[y, x] != null)
                blackPieces.Remove(Global.chessPieces[y, x]);
        }

        private void CheckCastleRequirements()
        {
            if (Global.selectedPiece is CastlePiece castlePiece)
                castlePiece.Moved = true;
        }

        private void CheckForPassingPawn(int y)
        {
            if (Global.selectedPiece is Pawn pawn && ((pawn.Y - 2) == y || (pawn.Y + 2) == y))
            {
                Global.passingPawn = pawn;
            }
        }

        private void CheckForEnPassant(int y, int x)
        {
            if (Global.selectedPiece is Pawn && Global.enPassantCapture.X == x && Global.enPassantCapture.Y == y)
            {
                Global.chessPieces[Global.passingPawn.Y, Global.passingPawn.X] = null;
                Global.board[Global.passingPawn.Y, Global.passingPawn.X].Image = Global.emptyImage;
            }
        }

        private void AnalyzeBlackPieces()
        {
            int maxPoints = 0;
            blackPieces.ForEach(piece => maxPoints = FindBestCapture(piece, maxPoints));
            if(bestPiece != null)
            {
                Global.selectedPiece = bestPiece;
                bestPiece.ShowMoves();
            }
                
        }

        private int FindBestCapture(ChessPiece piece, int maxPoints)
        {
            void UpdateMaxPoints(int points, Point capturePos)
            {
                if (points > maxPoints)
                {
                    maxPoints = points;
                    bestMove = capturePos;
                    bestPiece = piece;
                }
            }

            piece.CalcMoves();
            piece.MeetMoveRequirements();

            foreach (Point pos in piece.Captures)
            {
                ChessPiece whitePiece = Global.chessPieces[pos.Y, pos.X];
                int points = whitePiece.Value;

                if (whitePiece.Defenders.Count == 0 || (points -= piece.Value) > 0)
                    UpdateMaxPoints(points, pos);
            }

            return maxPoints;
        }

        private void StartComputerTurn()
        {
            if (!chkComputer.Checked || playerColor == "white" || gameOver)
                return;

            int hierarchy = 0;
            while(bestPiece == null)
            {
                switch(hierarchy)
                {
                    case 0: AnalyzeBlackPieces(); break;
                    case 1: FindRandomMove(piece => piece.Moves); break;
                    default: FindRandomMove(piece => piece.Captures); break;
                }
                hierarchy++;
            }
            ActivateSquare(bestMove.Y, bestMove.X);
        }

        private delegate List<Point> AccessList(ChessPiece piece);

        private void FindRandomMove(AccessList accessList)
        {
            List<int> indexes = new List<int>();
            for (int i = 0; i < blackPieces.Count; i++)
                indexes.Add(i);

            Random rand = new Random();
            do
            {
                if (indexes.Count == 0)
                    return;
                int index = indexes[rand.Next(indexes.Count)];
                Global.selectedPiece = blackPieces[index];
                Global.selectedPiece.CalcMoves();
                Global.selectedPiece.MeetMoveRequirements();
                indexes.Remove(index);
            }
            while (accessList(Global.selectedPiece).Count == 0);

            Global.selectedPiece.ShowMoves();
            bestPiece = Global.selectedPiece;
            bestMove = accessList(Global.selectedPiece)[rand.Next(Global.selectedPiece.TotalMoves.Count)];
        }

        private void CheckCastling(int newX)
        {
            King king = (King)Global.selectedPiece;
            for (int i = 0; i < 2; i++)
                if (newX == king.CastleDirections[i])
                    MoveRook(i, king);

        }

        private void MoveRook(int dirIndex, King king)
        {
            int[] newX = { king.X - 1, king.X + 1 };
            int edgeX = dirIndex * 7;
            Rook rook = (Rook)Global.chessPieces[king.Y, edgeX];
            Global.chessPieces[king.Y, edgeX] = null;

            Global.chessPieces[king.Y, newX[dirIndex]] = rook;
            rook.X = newX[dirIndex];
            rook.Moved = true;

            Global.board[king.Y, edgeX].Image = Global.emptyImage;
            Global.board[king.Y, newX[dirIndex]].Image = rook.Image;
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            
            
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            int y = Global.selectedPiece.Y;
            int x = Global.selectedPiece.X;

            int colorIndex = 0;
            if (playerColor == "black")
                colorIndex = 1;

            switch (item.Text)
            {
                case "Knight": Global.selectedPiece = new Knight(y, x, knight[colorIndex], playerColor); break;
                case "Bishop": Global.selectedPiece = new Bishop(y, x, bishop[colorIndex], playerColor); break;
                case "Rook": Global.selectedPiece = new Rook(y, x, rook[colorIndex], playerColor); break;
                case "Queen": Global.selectedPiece = new Queen(y, x, queen[colorIndex], playerColor); break;
            }

            Global.chessPieces[y, x] = Global.selectedPiece;
            Global.board[y, x].Image = Global.selectedPiece.Image;

            EndTurn();

            menuPromotion.AutoClose = true;
            menuPromotion.Hide();


        }

        private void Board_LocationChanged(object sender, EventArgs e)
        {
            if (Global.selectedPiece == null)
                return;
            Button btn = Global.board[Global.selectedPiece.Y, Global.selectedPiece.X];
            Point pos = new Point(btn.Left + Left, btn.Top + Top + 30);

            menuPromotion.Left = pos.X;
            menuPromotion.Top = pos.Y;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            BoardSetup();
        }

        private void chkComputer_CheckedChanged(object sender, EventArgs e)
        {
            StartComputerTurn();

        }

    }
}
