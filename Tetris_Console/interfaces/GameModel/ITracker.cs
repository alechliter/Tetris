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

        /// <summary>
        /// Subscriber event for when the grid updates. Alert all subscribers when the grid updates.
        /// </summary>
        public event Action GridUpdate;
        
        /// <summary>
        /// Convert tetromino piece to locked pieces
        /// </summary>
        public void LockPiece();
    }
}
