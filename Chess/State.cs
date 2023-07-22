using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FilipsChess
{
    internal class State
    {
        private State prev;
        private State next;
        private string playerColor;
        private bool gameOver;
        private Hashtable castlePieces;

        private ChessPiece[,] chessPieces; 

        public State(string playerColor, bool gameOver=false)
        {
            chessPieces = new ChessPiece[8, 8];
            castlePieces = new Hashtable();
            this.playerColor = playerColor;
            this.gameOver = gameOver;
        }

        public string PlayerColor { get => playerColor; set => playerColor = value; }
        public bool GameOver { get => gameOver; set => gameOver = value; }
        public Hashtable CastlePieces { get => castlePieces; set => castlePieces = value; }
        internal State Prev { get => prev; set => prev = value; }
        internal State Next { get => next; set => next = value; }
        internal ChessPiece[,] ChessPieces { get => chessPieces; set => chessPieces = value; }

        public void SaveCastlingConditions()
        {
            for (int y = 0; y < 8; y++)
                for (int x = 0; x < 8; x++)
                    if (chessPieces[y, x] != null && chessPieces[y, x] is CastlePiece)
                    {
                        CastlePiece castlePiece = (CastlePiece)chessPieces[y, x];
                        castlePieces.Add(castlePiece.Position, castlePiece.Moved);
                    }
                        
        }
    }
}
