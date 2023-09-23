using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FilipsChess
{
    internal static class Asset
    {
        private static string imagesPath = PathManager.GetMyDirectory() + "\\images\\";

        private static Bitmap[] rookImages = {
            (Bitmap)Image.FromFile(imagesPath + "rookW.png"),
            (Bitmap)Image.FromFile(imagesPath + "rookB.png")
        };
        private static Bitmap[] bishopImages = {
            (Bitmap)Image.FromFile(imagesPath + "bishopW.png"),
            (Bitmap)Image.FromFile(imagesPath + "bishopB.png")
        };
        private static Bitmap[] knightImages = {
            (Bitmap)Image.FromFile(imagesPath + "knightW.png"),
            (Bitmap)Image.FromFile(imagesPath + "knightB.png")
        };
        private static Bitmap[] queenImages = {
            (Bitmap)Image.FromFile(imagesPath + "queenW.png"),
            (Bitmap)Image.FromFile(imagesPath + "queenB.png")
        };
        private static Bitmap[] kingImages = {
            (Bitmap)Image.FromFile(imagesPath + "kingW.png"),
            (Bitmap)Image.FromFile(imagesPath + "kingB.png")
        };
        private static Bitmap[] pawnImages = {
            (Bitmap)Image.FromFile(imagesPath + "pawnW.png"),
            (Bitmap)Image.FromFile(imagesPath + "pawnB.png")
        };

        private static Bitmap blankImage = (Bitmap)Image.FromFile(imagesPath + "empty.png");



        public static Bitmap[] RookImage { get => rookImages; }
        public static Bitmap[] BishopImage { get => bishopImages; }
        public static Bitmap[] KnightImage { get => knightImages; }
        public static Bitmap[] QueenImage { get => queenImages; }
        public static Bitmap[] KingImage { get => kingImages; }
        public static Bitmap[] PawnImage { get => pawnImages; }
        public static Bitmap BlankImage { get => blankImage; }
    }
}
