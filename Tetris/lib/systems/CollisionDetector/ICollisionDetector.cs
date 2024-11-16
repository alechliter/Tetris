using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Models;
using Lechliter.Tetris.Lib.Objects;

namespace Lechliter.Tetris.Lib.Systems
{
    public interface ICollisionDetector<TPieceType, TDirection, TMoveType> where TPieceType : System.Enum
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
        public bool DetectCollisions(ITetromino<TPieceType, TDirection, TMoveType> piece, TMoveType moveType);

        /// <summary>
        /// Determines whether the given list of moves for a piece on the grid is possible without causing any further collisions. 
        /// </summary>
        /// <param name="piece">Piece to check moves on.</param>
        /// <param name="grid_pieces">Grid that contains the piece and locked pieces.</param>
        /// <param name="moves">The list of movements to test on the piece.</param>
        /// <returns>True if the list of movements is possible.</returns>
        public bool TryMove(ITetromino<ePieceType, eDirection, eMoveType> piece, ePieceType[,] grid_pieces, params Movement[] moves);


        public void StopAndResetStationaryTimer();
    }
}
