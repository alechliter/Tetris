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
    interface ITracker<TPieceType, TPieceDirection> where TPieceType : System.Enum 
                                                    where TPieceDirection : System.Enum
                                                                         
    {
        /// <summary>
        /// The current tetromino piece being followed by the tracker.
        /// </summary>
        /// <value>Reference to the tetromino. </value>
        ITetromino<TPieceType, TPieceDirection> CurrentPiece { get; set; }

        /// <summary>
        /// Matrix of stationary blocks and boundaries, with the origin (LockedPieces[0, 0]) in the 
        /// top left corner of the grid.
        /// </summary>
        /// <value>Matrix of TPieceType values.</value>
        TPieceType[,] LockedPieces { get; }

        /// <summary>
        /// Matrix of stationary blocks, boundaries, and the current tetromino blocks, with the origin (LockedPieces[0, 0]) in the 
        /// top left corner of the grid.
        /// </summary>
        /// <value>Matrix of TPieceType values.</value>
        TPieceType[,] AllPieces { get; }

        /// <summary>
        /// Subscriber event for when the grid updates. Alert all subscribers when the grid updates.
        /// </summary>
        public event Action GridUpdate;
        
        /// <summary>
        /// Convert tetromino piece to locked pieces
        /// </summary>
        void LockPiece();

        /// <summary>
        /// Checks if the current tetromino piece in contact with the boundaries or locked pieces.
        /// </summary>
        /// <returns>True if the piece is in contact with other objects.</returns>
        bool DetectCollision();
    }
}
