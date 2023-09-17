using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    /// <summary>
    /// ITracker records where the tetromino is with relation to the stationary blocks and the boundaries of the
    /// grid.
    /// </summary>
    /// <typeparam name="TPieceType">Enum: type of tetromino piece</typeparam>
    /// <typeparam name="TPieceDirection">Enum: direction of movement</typeparam>
    public interface ITracker<TPieceType, TPieceDirection, TMoveType> where TPieceType : System.Enum
                                                    where TPieceDirection : System.Enum
                                                    where TMoveType : System.Enum

    {
        /// <summary>
        /// The current tetromino piece being followed by the tracker.
        /// </summary>
        /// <value>Reference to the tetromino. </value>
        public ITetromino<TPieceType, TPieceDirection, TMoveType> CurrentPiece { get; set; }

        public IPreview<TPieceType, TPieceDirection, TMoveType> NextPiece { get; }

        public IPreview<TPieceType, TPieceDirection, TMoveType> HeldPiece { get; }

        /// <summary>
        /// Matrix of stationary blocks and boundaries, with the origin (LockedPieces[0, 0]) in the 
        /// top left corner of the grid.
        /// </summary>
        /// <value>Matrix of TPieceType values.</value>
        public TPieceType[,] LockedPieces { get; }

        /// <summary>
        /// Matrix of stationary blocks, boundaries, and the current tetromino blocks, with the origin (LockedPieces[0, 0]) in the 
        /// top left corner of the grid.
        /// </summary>
        /// <value>Matrix of TPieceType values.</value>
        public TPieceType[,] AllPieces { get; }

        public IntDimensions GridDim { get; }

        public IntDimensions BoundsDim { get; }

        public StaticComponent Grid { get; }

        /// <summary>
        /// Subscriber event for when the grid updates. Alert all subscribers when the grid updates.
        /// </summary>
        public event Action GridUpdate;

        /// <summary>
        /// Subscriber event for when the game ends. Alerts all subscribers when a tetromino locks at the very top.
        /// </summary>
        public event Action GameOver;

        /// <summary>
        /// Subscriber event for when at least one line is cleared. Sends all subscribers the number of lines cleared.
        /// </summary>
        public event Action<int> LinesCleared;

        /// <summary>
        /// Subscriber event for when a tetromino piece locks.
        /// </summary>
        public event Action PieceLocked;

        /// <summary>
        /// Convert tetromino piece to locked pieces
        /// </summary>
        public void LockPiece();

        /// <summary>
        /// Rounds the x and y values of a block's position to the upper level and then converts the coordinate 
        /// to the corresponding position in the xy-matrix.
        /// </summary>
        /// <param name="point">The point to find the xy-matrix indices for. </param>
        /// <param name="X">The x index of the block.</param>
        /// <param name="Y">The y index of the block.</param>
        public void GridPosition(Point point, out int X, out int Y);

        public void UpdateGrid(eMoveType moveType);

        public void HoldPiece();

        public bool isCollision(eMoveType moveType);

        public void ResetStationaryTimer();

        public void NextFrame();

        public ComponentContent[,] DisplayTimer();
    }
}
