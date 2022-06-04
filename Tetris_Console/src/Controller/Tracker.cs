using System;

namespace Lechliter.Tetris_Console
{
    public struct IntDimensions
    {
        public int X, Y;
        public IntDimensions(int width, int height)
        {
            this.X = width;
            this.Y = height;
        }
    }
    public class Tracker : ITracker<PieceType, Direction, MoveType>
    {
        /* Private Members */
        private ICollisionDetector<PieceType, Direction, MoveType> collisionDetector;

        /* Public Members */
        public static readonly IntDimensions GRID_DIM;

        public ITetromino<PieceType, Direction, MoveType> CurrentPiece { get; set; }

        public PieceType[,] LockedPieces { get; protected set; }

        public PieceType[,] AllPieces { get; protected set; }

        public event Action GridUpdate;

        /* Constructor */
        static Tracker()
        {
            const int WIDTH = 12, HEIGHT = 22;
            GRID_DIM = new IntDimensions(WIDTH, HEIGHT);
        }

        public Tracker(ITetromino<PieceType, Direction, MoveType> newPiece)
        {
            CurrentPiece = newPiece;

            LockedPieces = NewGrid();
            AllPieces = AddPieceToGrid(LockedPieces, CurrentPiece);
            collisionDetector = new CollisionDetector();

            // Subscribes to changes in the position
            CurrentPiece.UpdatePosition += DetectCollisions;
            CurrentPiece.UpdatePosition += UpdateGrid;
            CurrentPiece.UpdatePosition += (MoveType moveType) => { 
                if (moveType == MoveType.Translation || moveType == MoveType.Rotation)
                {
                    (collisionDetector as CollisionDetector).RestartStationaryTimer(); 
                }
            } ;
            // Subscribe to timer events
            collisionDetector.LockPiece += LockPiece;
        }

        /* Private Methods --------------------------------------------------------*/
        private static PieceType[,] AddPieceToGrid(PieceType[,] pieces, ITetromino<PieceType, Direction, MoveType> piece)
        {
            PieceType[,] newGrid = (PieceType[,])pieces.Clone();

            foreach(IBlock block in piece.Blocks){
                int x, y;
                GridPosition(block.Position, out x, out y);
                if(x < GRID_DIM.X - 1 && x > 0 && y < GRID_DIM.Y - 1 && y > 0){
                    if (newGrid[x, y] == PieceType.Empty)
                    {
                        newGrid[x, y] = piece.Type;
                    }
                    else
                    {
                        Console.Error.WriteLine("ERROR: Piece Collision Detected But Not Handled.");
                    }
                }
            }
            return newGrid;
        }
        private static PieceType[,] NewGrid()
        {
            const int X_LENGTH = 12, Y_LENGTH = 22;

            PieceType[,] grid = new PieceType[X_LENGTH, Y_LENGTH];

            // Fills the buffer area (perimeter) of the grid with locked markers
            for(int x = 0; x < GRID_DIM.X; x++)
            {
                grid[x, 0] = PieceType.Empty;
                grid[x, GRID_DIM.Y - 1] = PieceType.Locked;
            }
            for(int y = 1; y < GRID_DIM.Y - 1; y++)
            {
                grid[0, y] = PieceType.Locked;
                // Fills the remaining grid area with empty markers
                for(int x = 1; x < GRID_DIM.X - 1; x++)
                {
                    grid[x, y] = PieceType.Empty;
                }
                grid[GRID_DIM.X - 1, y] = PieceType.Locked;
            }

            return grid;
        }

        private void DetectCollisions(MoveType moveType)
        {
            collisionDetector.DetectCollisions(CurrentPiece, LockedPieces, moveType);
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
            X = (int)Math.Ceiling(point.x) % GRID_DIM.X;
            Y = (int)Math.Ceiling(point.y) % GRID_DIM.Y;
        }

        public void UpdateGrid(MoveType moveType)
        {
            this.AllPieces = AddPieceToGrid(this.LockedPieces, this.CurrentPiece);
            GridUpdate?.Invoke();
        }

        public void LockPiece()
        {
            LockedPieces = AllPieces;
            CurrentPiece.NewPiece();
            (collisionDetector.LockTimerStationary as LockTimer).Stop();
            //Console.WriteLine("Pieces Locked!");
        }

        public bool isCollision(MoveType moveType)
        {
            return collisionDetector.DetectCollisions(CurrentPiece, LockedPieces, moveType);
        }

        public void NextFrame()
        {
            collisionDetector.LockTimerFalling.CountDown();
            collisionDetector.LockTimerStationary.CountDown();

            Console.WriteLine($"Falling Timer: {collisionDetector.LockTimerFalling.FramesRemaining}");
            Console.WriteLine($"Stationary Timer: {collisionDetector.LockTimerStationary.FramesRemaining}");
        }
    }
}
