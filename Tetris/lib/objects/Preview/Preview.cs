using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Types;
using Tetris.lib.Design.Helpers;

namespace Lechliter.Tetris.Lib.Objects
{
    public class Preview : IPreview<ePieceType>
    {
        public ePieceType PieceType
        {
            get
            {
                return _piece;
            }
            set
            {
                _piece = value;
                AddPieceToGrid(_piece);
                PieceUpdated?.Invoke();
            }
        }

        public ePieceType[,] Grid { get; private set; }

        public event Action? PieceUpdated;

        private ePieceType _piece = ePieceType.NotSet;

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
            Grid = CreateGrid();
        }

        private void AddPieceToGrid(ePieceType pieceType)
        {
            Grid = CreateGrid();

            if (pieceType == ePieceType.NotSet)
            {
                return;
            }

            ITetromino<ePieceType, eDirection, eMoveType> tetromino = new Tetromino(DefaultOrigin, pieceType);
            foreach (IBlock block in tetromino.Blocks)
            {
                IntPoint point = GridPosition(block.Position);
                Grid[point.X, point.Y] = pieceType;
            }
        }

        private ePieceType[,] CreateGrid()
        {
            return new ePieceType[Dim.X, Dim.Y];
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
