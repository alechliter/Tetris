using System;
using System.Collections.Generic;

namespace Lechliter.Tetris_Console
{
    public class Tracker : ITracker<ePieceType, eDirection, eMoveType>
    {
        /* Private Members */
        private ICollisionDetector<ePieceType, eDirection, eMoveType> collisionDetector;

        private bool canHoldPiece = true;

        /* Public Members */
        public static readonly IntDimensions BOUNDS_DIM;

        public static readonly IntDimensions GRID_DIM;

        public ITetromino<ePieceType, eDirection, eMoveType> CurrentPiece { get; set; }

        public IPreview<ePieceType, eDirection, eMoveType> NextPiece { get; }

        public IPreview<ePieceType, eDirection, eMoveType> HeldPiece { get; }

        public ePieceType[,] LockedPieces { get; protected set; }

        public ePieceType[,] AllPieces { get; protected set; }

        public StaticComponent Grid { get; protected set; }

        public event Action GridUpdate;
         
        public event Action GameOver;

        public event Action<int> LinesCleared;

        /* Constructor */
        static Tracker()
        {
            const int WIDTH = 12, HEIGHT = 22;
            BOUNDS_DIM = new IntDimensions(WIDTH, HEIGHT);
            GRID_DIM = new IntDimensions(WIDTH - 2, HEIGHT - 2);
        }

        public Tracker(ITetromino<ePieceType, eDirection, eMoveType> newPiece)
        {
            CurrentPiece = newPiece;
            NextPiece = new Preview();
            HeldPiece = new Preview(ePieceType.NotSet);

            LockedPieces = NewGrid();
            AllPieces = AddPieceToGrid(LockedPieces, CurrentPiece);
            collisionDetector = new CollisionDetector();

            // Subscribes to changes in the position
            CurrentPiece.UpdatePosition += DetectCollisions;
            CurrentPiece.UpdatePosition += UpdateGrid;

            // Subscribe to timer events
            collisionDetector.LockPiece += LockPiece;
        }

        /* Private Methods --------------------------------------------------------*/
        private static ePieceType[,] AddPieceToGrid(ePieceType[,] pieces, ITetromino<ePieceType, eDirection, eMoveType> piece)
        {
            ePieceType[,] newGrid = (ePieceType[,])pieces.Clone();

            foreach(IBlock block in piece.Blocks){
                int x, y;
                GridPosition(block.Position, out x, out y);
                if(x < BOUNDS_DIM.X - 1 && x > 0 && y < BOUNDS_DIM.Y - 1 && y >= 0){
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
        private static ePieceType[,] NewGrid()
        {
            ePieceType[,] grid = new ePieceType[BOUNDS_DIM.X, BOUNDS_DIM.Y];

            // Fills the buffer area (perimeter) of the grid with locked markers
            for(int x = 0; x < BOUNDS_DIM.X; x++)
            {
                grid[x, 0] = ePieceType.Empty;
                grid[x, BOUNDS_DIM.Y - 1] = ePieceType.Locked;
            }
            for(int y = 0; y < BOUNDS_DIM.Y - 1; y++)
            {
                grid[0, y] = ePieceType.Locked;
                // Fills the remaining grid area with empty markers
                for(int x = 1; x < BOUNDS_DIM.X - 1; x++)
                {
                    grid[x, y] = ePieceType.Empty;
                }
                grid[BOUNDS_DIM.X - 1, y] = ePieceType.Locked;
            }

            return grid;
        }

        private void DetectCollisions(eMoveType moveType)
        {
            collisionDetector.DetectCollisions(CurrentPiece, LockedPieces, moveType);
        }

        private bool isLineFull(int row_number)
        {
            bool isFull = true;
            int x = 1;
            while(isFull && x < BOUNDS_DIM.X - 1)
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
            while(isEmpty && x < BOUNDS_DIM.X - 1)
            {
                isEmpty = this.LockedPieces[x, row_number] == ePieceType.Empty;
                x++;
            }
            return isEmpty;
        }

        private void EraseRow(int row_number)
        {
            for (int x = 1; x < BOUNDS_DIM.X - 1; x++)
            {
                this.LockedPieces[x, row_number] = ePieceType.Empty;
            }
        }

        private void CopyRowTo(int src_row, int dest_row)
        {
            for (int x = 1; x < BOUNDS_DIM.X - 1; x++)
            {
                this.LockedPieces[x, dest_row] = this.LockedPieces[x, src_row];
            }
        }

        private void MoveLinesDown(int row_number)
        {
            if (row_number < BOUNDS_DIM.Y - 1)
            {
                if (row_number < BOUNDS_DIM.Y - 2)
                {
                    CopyRowTo(row_number, row_number + 1);
                }

                EraseRow(row_number);

                if(row_number - 1 > 0 && !isLineEmpty(row_number - 1)){
                    MoveLinesDown(row_number - 1);
                }
            }
        }
        
        private void ClearLines()
        {
            int numLines = 0;
            for(int row_number = BOUNDS_DIM.Y - 2; row_number > 0; row_number--)
            {
                int i = 0;
                while (isLineFull(row_number))
                {
                    MoveLinesDown(row_number - 1);
                    numLines++;
                    Console.Beep(400 + i, 115);
                    i += 135;
                }
                if (isLineEmpty(row_number - 1)){
                    break;
                }
            }
            if (numLines > 0)
            {
                this.LinesCleared?.Invoke(numLines);
            }
            this.AllPieces = this.LockedPieces;
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

        /* Public Methods -----------------------------------------------------------*/

        /// <summary>
        /// Rounds the x and y values of a block's position to the upper level and then converts the coordinate 
        /// to the corresponding position in the xy-matrix.
        /// </summary>
        /// <param name="point">The point to find the xy-matrix indicies for. </param>
        /// <param name="X">The x index of the block.</param>
        /// <param name="Y">The y index of the block.</param>
        public static void GridPosition(Point point, out int X, out int Y)
        {
            X = (int)Math.Ceiling(point.x) % BOUNDS_DIM.X;
            Y = (int)Math.Ceiling(point.y) % BOUNDS_DIM.Y;
        }

        public void UpdateGrid(eMoveType moveType)
        {
            this.AllPieces = AddPieceToGrid(this.LockedPieces, this.CurrentPiece);
            GridUpdate?.Invoke();
        }

        public void LockPiece()
        {
            LockedPieces = AllPieces;
            Console.Beep(600, 200);
            ClearLines();
            ResetStationaryTimer();
            if (!IsGameOver())
            {
                (CurrentPiece as Tetromino).NewPiece(NextPiece.Piece.Type);
                NextPiece.NewPiece();
                this.canHoldPiece = true;
            }
        }

        public void HoldPiece()
        {
            if (this.canHoldPiece)
            {
                if (HeldPiece.Piece.Type != ePieceType.NotSet)
                {
                    Tetromino temp = (CurrentPiece as Tetromino).Copy();
                    (CurrentPiece as Tetromino).NewPiece(HeldPiece.Piece.Type);
                    HeldPiece.NewPiece(temp.Type);
                }
                else
                {
                    HeldPiece.NewPiece(CurrentPiece.Type);
                    (CurrentPiece as Tetromino).NewPiece(NextPiece.Piece.Type);
                    NextPiece.NewPiece();
                }
                this.canHoldPiece = false;
            }
        }

        public bool isCollision(eMoveType moveType)
        {
            return collisionDetector.DetectCollisions(CurrentPiece, LockedPieces, moveType);
        }

        public void ResetStationaryTimer()
        {
            (collisionDetector as CollisionDetector).StopAndResetStationaryTimer();
        }

        public void NextFrame()
        {
            collisionDetector.LockTimerFalling.CountDown();
            collisionDetector.LockTimerStationary.CountDown();
        }

        public ComponentContent[,] Displaytimer()
        {
            string fallingTimer = $"Falling Timer: {collisionDetector.LockTimerFalling.FramesRemaining}";
            string stationaryTimer = $"Stationary Timer: {collisionDetector.LockTimerStationary.FramesRemaining}";

            ComponentContent[,] timers = new ComponentContent[Math.Max(fallingTimer.Length, stationaryTimer.Length), 2];

            for(int i = 0; i < fallingTimer.Length; i++)
            {
                timers[i, 0] = new ComponentContent(fallingTimer[i], eTextColor.Yellow);
            }

            for (int i = 0; i < stationaryTimer.Length; i++)
            {
                timers[i, 1] = new ComponentContent(stationaryTimer[i], eTextColor.Yellow);
            }

            return timers;
        }
    }
}
