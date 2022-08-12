using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public class Preview : IPreview<ePieceType, eDirection, eMoveType>
    {
        public ITetromino<ePieceType, eDirection, eMoveType> Piece { get; }

        public ePieceType[,] Grid { get; protected set; }

        public event Action PieceUpdated;

        private static IntDimensions Dim { get => new IntDimensions(3, 4); }

        private static readonly Point DefaultOrigin = new Point(1, 2);

        public Preview()
        {
            Piece = new Tetromino(DefaultOrigin);
            NewPiece();
        }

        public Preview(ePieceType type)
        {
            Piece = new Tetromino(DefaultOrigin);
            NewPiece(type);
        }

        public void NewPiece()
        {
            Piece.NewPiece();
            AddPieceToGrid();
            PieceUpdated?.Invoke();
        }

        public void NewPiece(ePieceType type)
        {
            (Piece as Tetromino).NewPiece(type);
            AddPieceToGrid();
            PieceUpdated?.Invoke();
        }


        private void AddPieceToGrid()
        {
            this.Grid = new ePieceType[Dim.X, Dim.Y];
            foreach (IBlock block in this.Piece.Blocks)
            {
                IntPoint point = GridPosition(block.Position);
                Grid[point.X, point.Y] = Piece.Type;
            }
        }

        private static IntPoint GridPosition(Point point)
        {
            return new IntPoint()
            {
                X = (int)Math.Ceiling(point.x) % Dim.X,
                Y = (int)Math.Ceiling(point.y) % Dim.Y
            };
        }
    }
}
