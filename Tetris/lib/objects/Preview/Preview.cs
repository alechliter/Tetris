using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Types;
using Tetris.lib.Design.Helpers;

namespace Lechliter.Tetris.Lib.Objects
{
    public class Preview : IPreview<ePieceType, eDirection, eMoveType>
    {
        public ITetromino<ePieceType, eDirection, eMoveType> Piece { get; }

        public ePieceType[,] Grid { get; protected set; }

        public event Action PieceUpdated;

        private static readonly IntDimensions Dim;

        private static readonly Point DefaultOrigin;

        static Preview()
        {
            Dim = new IntDimensions(
                width: ConfigurationHelper.GetInt("PreviewWindowWidth"),
                height: ConfigurationHelper.GetInt("PreviewWindowHeight")
            );
            DefaultOrigin = new Point(
                x: ConfigurationHelper.GetFloat("PreviewWindowOriginX"),
                y: ConfigurationHelper.GetFloat("PreviewWindowOriginY")
            );
        }

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
            Piece.NewPiece(type);
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
