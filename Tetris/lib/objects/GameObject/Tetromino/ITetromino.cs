using Lechliter.Tetris.Lib.Systems;
using Lechliter.Tetris.Lib.Types;
using Tetris.Lib.Objects.GameObject;

namespace Lechliter.Tetris.Lib.Objects
{
    public interface ITetromino<TPieceType, TDirection, TMoveType> : IGameObject, ICopyObject<ITetromino<TPieceType, TDirection, TMoveType>>
        where TPieceType : System.Enum
        where TDirection : System.Enum
        where TMoveType : System.Enum
    {
        ICollection<IBlock> Blocks { get; }

        TPieceType Type { get; }

        Point Velocity { get; }

        TDirection Rotation { get; }

        /// <summary>
        /// Subscriber event: notifies subscribers that the position/orientation of the tetromino changed.
        /// </summary>
        event Action<TMoveType> UpdatePosition;

        /// <summary>
        /// Rotates the piece about its pivot point.
        /// </summary>
        /// <param name="direction">Direction of rotation.</param>
        void Rotate(TDirection direction);

        /// <summary>
        /// Moves the tetromino piece on block length in the specified direction.
        /// </summary>
        /// <param name="direction">Direction of travel</param>
        void Move(TDirection direction, bool emitEvent = true);

        void Drop(ITracker<TPieceType, TDirection, TMoveType> tracker);

        void UndoMove(TMoveType moveType);
    }
}
