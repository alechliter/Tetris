using Lechliter.Tetris.Lib.Types;

namespace Lechliter.Tetris.Lib.Objects
{
    public interface IGrid<TPieceType, TDirection, TMoveType>
        where TPieceType : System.Enum
        where TDirection : System.Enum
        where TMoveType : System.Enum

    {
        public IntDimensions GridDim { get; }

        public IntDimensions BoundsDim { get; }

        /// <summary>
        /// Matrix of stationary blocks, boundaries, and the current tetromino blocks, with the origin (Pieces[0, 0]) in the 
        /// top left corner of the grid.
        /// </summary>
        /// <value>Matrix of TPieceType values.</value>
        public TPieceType[,] Pieces { get; }


        /// <summary>
        /// Subscriber event for when the grid updates. Alert all subscribers when the grid updates.
        /// </summary>
        public event Action? Update;

        /// <summary>
        /// Rounds the x and y values of a block's position to the upper level and then converts the coordinate 
        /// to the corresponding position in the xy-matrix.
        /// </summary>
        /// <param name="point">The point to find the xy-matrix indices for. </param>
        /// <param name="X">The x index of the block.</param>
        /// <param name="Y">The y index of the block.</param>
        public void GridPosition(Point point, out int X, out int Y);

        /// <summary>
        /// Adds the tetromino's blocks to the current grid.
        /// </summary>
        /// <param name="tetromino"></param>
        public void AddTetromino(ITetromino<TPieceType, TDirection, TMoveType> tetromino);

        /// <summary>
        /// Saves the current state of the grid to the locked pieces matrix.
        /// </summary>
        public void LockPieces();

        /// <summary>
        /// Checks if the cell positioned at (x, y) is empty.
        /// </summary>
        /// <param name="x">The x-axis position of the cell.</param>
        /// <param name="y">The y-axis position of the cell.</param>
        /// <returns>True if the cell at (x, y) is empty.</returns>
        public bool IsCellEmpty(int x, int y);

        /// <summary>
        /// Checks if a line does not have any empty spaces.
        /// </summary>
        /// <param name="row_number">The row to check.</param>
        /// <returns>True if there are 0 empty spaces in the row.</returns>
        public bool IsLineFull(int row_number);

        /// <summary>
        /// Checks if a line does not have any non-empty pieces in it.
        /// </summary>
        /// <param name="row_number">The row to check.</param>
        /// <returns>True if there are 0 non-empty pieces in the row.</returns>
        public bool IsLineEmpty(int row_number);

        public bool IsInBounds(int x, int y);

        /// <summary>
        /// Removes all full lines from the grid and moves all pieces above empty lines down.
        /// </summary>
        /// <returns>The number of full lines cleared.</returns>
        public int ClearLines();
    }
}
