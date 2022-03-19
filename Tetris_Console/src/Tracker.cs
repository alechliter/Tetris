using System;
using System.Collections.Generic;
using System.Text;

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
    public class Tracker : ITracker<PieceType, Direction>
    {
        /* Public Members */
        public static readonly IntDimensions GRID_DIM;
        public ITetromino<PieceType, Direction> CurrentPiece { get; set; }

        public PieceType[,] LockedPieces { get; protected set; }
        public PieceType[,] AllPieces { get; protected set; }
        public event Action GridUpdate;

        /* Constructor */
        static Tracker()
        {
            const int WIDTH = 12, HEIGHT = 22;
            GRID_DIM = new IntDimensions(WIDTH, HEIGHT);
        }
        public Tracker(ITetromino<PieceType, Direction> newPiece)
        {
            CurrentPiece = newPiece;

            LockedPieces = NewGrid();
            AllPieces = AddPieceToGrid(LockedPieces, CurrentPiece);

            CurrentPiece.UpdatePosition += this.UpdateGrid; //subscribes to changes in the position
        }

        /* Private Methods --------------------------------------------------------*/
        private static PieceType[,] AddPieceToGrid(PieceType[,] pieces, ITetromino<PieceType, Direction> piece)
        {
            PieceType[,] newGrid = (PieceType[,])pieces.Clone();

            foreach(IBlock block in piece.Blocks){
                // Rounds the x and y values to the upper level and then converts the coordinate
                // to the corresponding position in the xy-matrix.
                int x = (int) Math.Ceiling(block.Position.x) % GRID_DIM.X;
                int y = (int) Math.Ceiling(block.Position.y) % GRID_DIM.Y;
                if(x < GRID_DIM.X - 1 && x > 0 && y < GRID_DIM.Y - 1 && y > 0){
                    // TODO: Handle collision with block
                    newGrid[x, y] = piece.Type;
                }else{
                    // TODO: Handle collision with wall
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
                grid[x, 0] = PieceType.X;
                grid[x, GRID_DIM.Y - 1] = PieceType.X;
            }
            for(int y = 1; y < GRID_DIM.Y - 1; y++)
            {
                grid[0, y] = PieceType.X;
                // Fills the remaining grid area with empty markers
                for(int x = 1; x < GRID_DIM.X - 1; x++)
                {
                    grid[x, y] = PieceType.E;
                }
                grid[GRID_DIM.X - 1, y] = PieceType.X;
            }

            return grid;
        }

        /* Public Methods -----------------------------------------------------------*/
        public void UpdateGrid()
        {
            this.AllPieces = AddPieceToGrid(this.LockedPieces, this.CurrentPiece);
            GridUpdate?.Invoke();
        }
        public bool DetectCollision()
        {
            // TODO: Finish
            return true;
        }

        public void LockPiece()
        {
            // TODO: Finish
        }
    }
}
