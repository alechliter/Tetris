using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public interface ICollisionDetector <TPieceType, TDirection, TMoveType> where TPieceType : System.Enum
                                                                     where TDirection : System.Enum
                                                                     where TMoveType : System.Enum
    {
        /// <summary>
        /// Frame timer that counts down when a piece has stopped falling.
        /// </summary>
        public IFrameTimer LockTimerFalling { get; }

        /// <summary>
        /// Frame timer that counts down when a piece has stopped moving.
        /// </summary>
        public IFrameTimer LockTimerStationary { get; }

        /// <summary>
        /// Subscriber event that alerts all subscribers when a collision has been detected.
        /// </summary>
        public event Action CollisionDetected;

        /// <summary>
        /// Subscriber event that alerts all subscribers when a piece needs to be locked.
        /// </summary>
        public event Action LockPiece;

        /// <summary>
        /// Given a tetromino and a grid of stationary pieces, checks if the tetromino is overlapping with any stationary pieces.
        /// If so, it moves the tetromino to a free space and alerts all subscribers that a collision has been detected.
        /// </summary>
        /// <param name="piece"></param>
        /// <param name="grid_pieces"></param>
        /// <param name="moveType"></param>
        /// <returns></returns>
        public bool DetectCollisions(ITetromino<TPieceType, TDirection, TMoveType> piece, TPieceType[,] grid_pieces, TMoveType moveType);
    }
}
