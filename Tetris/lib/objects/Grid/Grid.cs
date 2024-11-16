using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Exceptions;
using Lechliter.Tetris.Lib.Types;

namespace Lechliter.Tetris.Lib.Objects
{
    public class Grid : IGrid<ePieceType, eDirection, eMoveType>
    {
        public IntDimensions GridDim { get; private set; }

        public IntDimensions BoundsDim { get; private set; }

        public ePieceType[,] Pieces { get; private set; }

        public ePieceType[,] LockedPieces { get; private set; }

        public event Action? Update;

        private static readonly IntDimensions DefaultBounds;

        private static readonly IntDimensions DefaultGrid;

        static Grid()
        {
            DefaultBounds = new IntDimensions(GRID_WIDTH, GRID_HEIGHT);
            DefaultGrid = new IntDimensions(GRID_WIDTH - 2, GRID_HEIGHT - 2);
        }

        public Grid()
        {
            GridDim = DefaultGrid;
            BoundsDim = DefaultBounds;
            Pieces = CreateEmptyGrid();
            LockedPieces = CopyGrid(Pieces);
        }

        public void GridPosition(Point point, out int x, out int y)
        {
            x = (int)Math.Ceiling(point.x) % BoundsDim.X;
            y = (int)Math.Ceiling(point.y) % BoundsDim.Y;
        }

        public void AddTetromino(ITetromino<ePieceType, eDirection, eMoveType> tetromino)
        {
            Pieces = AddPiecesToGrid(LockedPieces, tetromino);
            Update?.Invoke();
        }

        public void LockPieces()
        {
            LockedPieces = CopyGrid(Pieces);
        }

        public bool IsLineFull(int row_number)
        {
            bool isFull = true;
            int x = 1;
            while (isFull && x < BoundsDim.X - 1)
            {
                isFull = this.LockedPieces[x, row_number] != ePieceType.Empty;
                x++;
            }
            return isFull;
        }

        public bool IsLineEmpty(int row_number)
        {
            bool isEmpty = true;
            int x = 1;
            while (isEmpty && x < BoundsDim.X - 1)
            {
                isEmpty = this.LockedPieces[x, row_number] == ePieceType.Empty;
                x++;
            }
            return isEmpty;
        }

        public int ClearLines()
        {
            int numLines = 0;
            for (int row_number = BoundsDim.Y - 2; row_number > 0; row_number--)
            {
                while (IsLineFull(row_number))
                {
                    MoveLinesDown(row_number - 1);
                    numLines++;
                }
                if (IsLineEmpty(row_number - 1))
                {
                    break;
                }
            }

            Pieces = LockedPieces;
            return numLines;
        }

        private ePieceType[,] CreateEmptyGrid()
        {
            ePieceType[,] grid = new ePieceType[BoundsDim.X, BoundsDim.Y];

            // Fills the buffer area (perimeter) of the grid with locked markers
            for (int x = 0; x < BoundsDim.X; x++)
            {
                grid[x, 0] = ePieceType.Empty;
                grid[x, BoundsDim.Y - 1] = ePieceType.Locked;
            }

            for (int y = 0; y < BoundsDim.Y - 1; y++)
            {
                grid[0, y] = ePieceType.Locked;
                // Fills the remaining grid area with empty markers
                for (int x = 1; x < BoundsDim.X - 1; x++)
                {
                    grid[x, y] = ePieceType.Empty;
                }
                grid[BoundsDim.X - 1, y] = ePieceType.Locked;
            }

            return grid;
        }

        private ePieceType[,] AddPiecesToGrid(ePieceType[,] pieces, ITetromino<ePieceType, eDirection, eMoveType> piece)
        {
            ePieceType[,] newGrid = CopyGrid(pieces);

            foreach (IBlock block in piece.Blocks)
            {
                GridPosition(block.Position, out int x, out int y);
                if (x < BoundsDim.X - 1 && x > 0 && y < BoundsDim.Y - 1 && y >= 0)
                {
                    if (pieces[x, y] == ePieceType.Empty)
                    {
                        newGrid[x, y] = piece.Type;
                    }
                    else
                    {
                        throw new TetrisLibException($"Tracker: Piece Collision Detected But Not Handled. Position - X:{x}, Y:{y}");
                    }
                }
            }
            return newGrid;
        }

        private void EraseRow(int row_number)
        {
            for (int x = 1; x < BoundsDim.X - 1; x++)
            {
                this.LockedPieces[x, row_number] = ePieceType.Empty;
            }
        }

        private void CopyRowTo(int src_row, int dest_row)
        {
            for (int x = 1; x < BoundsDim.X - 1; x++)
            {
                this.LockedPieces[x, dest_row] = this.LockedPieces[x, src_row];
            }
        }


        private void MoveLinesDown(int row_number)
        {
            if (row_number < BoundsDim.Y - 1)
            {
                if (row_number < BoundsDim.Y - 2)
                {
                    CopyRowTo(row_number, row_number + 1);
                }

                EraseRow(row_number);

                if (row_number - 1 > 0 && !IsLineEmpty(row_number - 1))
                {
                    MoveLinesDown(row_number - 1);
                }
            }
        }

        private ePieceType[,] CopyGrid(ePieceType[,] pieces)
        {
            return (ePieceType[,])pieces.Clone();
        }

        #region Constants

        private const int GRID_WIDTH = 12;

        private const int GRID_HEIGHT = 22;

        #endregion
    }
}
