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

        private ChessPiece[,] chessPieces; 

        public State(ChessPiece[,] chessPieces)
        {
            this.chessPieces = chessPieces;
        }

        public State(string playerColor, bool gameOver=false)
        {
            chessPieces = new ChessPiece[8, 8];
            this.playerColor = playerColor;
            this.gameOver = gameOver;
        }

        public string PlayerColor { get => playerColor; set => playerColor = value; }
        public bool GameOver { get => gameOver; set => gameOver = value; }
        internal State Prev { get => prev; set => prev = value; }
        internal State Next { get => next; set => next = value; }
        internal ChessPiece[,] ChessPieces { get => chessPieces; set => chessPieces = value; }
    }
}
