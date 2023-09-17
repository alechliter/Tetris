using System;

namespace Lechliter.Tetris_Console
{
    public class Tracker : ITracker<ePieceType, eDirection, eMoveType>
    {
        public ITetromino<ePieceType, eDirection, eMoveType> CurrentPiece { get; set; }

        public IPreview<ePieceType, eDirection, eMoveType> NextPiece { get; }

        public IPreview<ePieceType, eDirection, eMoveType> HeldPiece { get; }

        public ePieceType[,] LockedPieces { get; protected set; }

        public ePieceType[,] AllPieces { get; protected set; }

        public IntDimensions GridDim { get; protected set; }

        public IntDimensions BoundsDim { get; protected set; }

        public StaticComponent Grid { get; protected set; }

        public event Action GridUpdate;

        public event Action GameOver;

        public event Action<int> LinesCleared;

        public event Action PieceLocked;

        private ICollisionDetector<ePieceType, eDirection, eMoveType> _CollisionDetector;

        private bool CanHoldPiece = true;

        public Tracker(ITetromino<ePieceType, eDirection, eMoveType> newPiece, IntDimensions boundsDim, IntDimensions gridDim)
        {
            CurrentPiece = newPiece;
            NextPiece = new Preview();
            HeldPiece = new Preview(ePieceType.NotSet);

            BoundsDim = boundsDim;
            GridDim = gridDim;

            LockedPieces = NewGrid();
            AllPieces = AddPieceToGrid(LockedPieces, CurrentPiece);
            _CollisionDetector = new CollisionDetector(this, BoundsDim, GridDim);

            // Subscribes to changes in the position
            CurrentPiece.UpdatePosition += DetectCollisions;
            CurrentPiece.UpdatePosition += UpdateGrid;

            // Subscribe to timer events
            _CollisionDetector.LockPiece += LockPiece;
        }

        #region Public Methods

        public void GridPosition(Point point, out int X, out int Y)
        {
            X = (int)Math.Ceiling(point.x) % BoundsDim.X;
            Y = (int)Math.Ceiling(point.y) % BoundsDim.Y;
        }

        public void UpdateGrid(eMoveType moveType)
        {
            this.AllPieces = AddPieceToGrid(this.LockedPieces, this.CurrentPiece);
            GridUpdate?.Invoke();
        }

        public void LockPiece()
        {
            LockedPieces = AllPieces;
            PieceLocked?.Invoke();
            ClearLines();
            ResetStationaryTimer();
            if (!IsGameOver())
            {
                CurrentPiece.NewPiece(NextPiece.Piece.Type);
                NextPiece.NewPiece();
                CanHoldPiece = true;
            }
        }

        public void HoldPiece()
        {
            if (CanHoldPiece)
            {
                if (HeldPiece.Piece.Type != ePieceType.NotSet)
                {
                    Tetromino temp = CurrentPiece.Copy() as Tetromino;
                    CurrentPiece.NewPiece(HeldPiece.Piece.Type);
                    HeldPiece.NewPiece(temp.Type);
                }
                else
                {
                    HeldPiece.NewPiece(CurrentPiece.Type);
                    CurrentPiece.NewPiece(NextPiece.Piece.Type);
                    NextPiece.NewPiece();
                }
                CanHoldPiece = false;
            }
        }

        public bool isCollision(eMoveType moveType)
        {
            return _CollisionDetector.DetectCollisions(CurrentPiece, LockedPieces, moveType);
        }

        public void ResetStationaryTimer()
        {
            _CollisionDetector.StopAndResetStationaryTimer();
        }

        public void NextFrame()
        {
            _CollisionDetector.LockTimerFalling.CountDown();
            _CollisionDetector.LockTimerStationary.CountDown();
        }

        public ComponentContent[,] DisplayTimer()
        {
            string fallingTimer = $"Falling Timer: {_CollisionDetector.LockTimerFalling.FramesRemaining}";
            string stationaryTimer = $"Stationary Timer: {_CollisionDetector.LockTimerStationary.FramesRemaining}";

            ComponentContent[,] timers = new ComponentContent[Math.Max(fallingTimer.Length, stationaryTimer.Length), 2];

            for (int i = 0; i < fallingTimer.Length; i++)
            {
                timers[i, 0] = new ComponentContent(fallingTimer[i], eTextColor.Yellow);
            }

            for (int i = 0; i < stationaryTimer.Length; i++)
            {
                timers[i, 1] = new ComponentContent(stationaryTimer[i], eTextColor.Yellow);
            }

            return timers;
        }
        #endregion

        #region Private Methods
        private ePieceType[,] AddPieceToGrid(ePieceType[,] pieces, ITetromino<ePieceType, eDirection, eMoveType> piece)
        {
            ePieceType[,] newGrid = (ePieceType[,])pieces.Clone();

            foreach (IBlock block in piece.Blocks)
            {
                int x, y;
                GridPosition(block.Position, out x, out y);
                if (x < BoundsDim.X - 1 && x > 0 && y < BoundsDim.Y - 1 && y >= 0)
                {
                    if (pieces[x, y] == ePieceType.Empty)
                    {
                        newGrid[x, y] = piece.Type;
                    }
                    else
                    {
                        ErrorMessageHandler.DisplayMessage($"ERROR: Piece Collision Detected But Not Handled. Position - X:{x}, Y:{y}");
                    }
                }
            }
            return newGrid;
        }
        private ePieceType[,] NewGrid()
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

        private void DetectCollisions(eMoveType moveType)
        {
            _CollisionDetector.DetectCollisions(CurrentPiece, LockedPieces, moveType);
        }

        private bool isLineFull(int row_number)
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

        private bool isLineEmpty(int row_number)
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

                if (row_number - 1 > 0 && !isLineEmpty(row_number - 1))
                {
                    MoveLinesDown(row_number - 1);
                }
            }
        }

        private void ClearLines()
        {
            int numLines = 0;
            for (int row_number = BoundsDim.Y - 2; row_number > 0; row_number--)
            {
                while (isLineFull(row_number))
                {
                    MoveLinesDown(row_number - 1);
                    numLines++;
                }
                if (isLineEmpty(row_number - 1))
                {
                    break;
                }
            }
            if (numLines > 0)
            {
                LinesCleared?.Invoke(numLines);
            }
            AllPieces = LockedPieces;
        }

        /// <summary>
        /// Alerts all GameOver subscribers if a piece is out of bounds at the top of the board.
        /// </summary>
        /// <returns>True if the game is over.</returns>
        private bool IsGameOver()
        {
            bool isOver = !isLineEmpty(0);
            if (isOver)
            {
                GameOver?.Invoke();
            }
            return isOver;
        }

        private Block[] copyBlocks()
        {
            Block[] blocks = new Block[CurrentPiece.Blocks.Count];
            CurrentPiece.Blocks.CopyTo(blocks, 0);
            return blocks;
        }

        #endregion
    }
}
