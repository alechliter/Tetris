using System;
using System.Collections.Generic;

namespace Lechliter.Tetris_Console
{
    public interface ITetromino <TPieceType, TDirection> : IGameObject where TPieceType : System.Enum 
                                                                where TDirection : System.Enum
    {
        public ICollection<IBlock> Blocks { get; set; }
        public TPieceType Type { get; }
        
        /// <summary>
        /// Subsriber event: notifies subsribers that the position/orientation of the tetromino changed.
        /// </summary>
        public event Action UpdatePosition;

        /// <summary>
        /// Rotates the piece about its pivot point.
        /// </summary>
        /// <param name="direction">Direction of rotation.</param>
        void Rotate(TDirection direction);
        
        /// <summary>
        /// Moves the tetromino piece on block length in the specified direction.
        /// </summary>
        /// <param name="direction">Direction of travel</param>
        void Move(TDirection direction);

        void NewPiece();
    }
}
