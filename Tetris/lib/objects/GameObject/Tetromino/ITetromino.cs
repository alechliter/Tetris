using Lechliter.Tetris.Lib.Definitions;
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

        /// <summary>
        /// Creates a new tetromino piece and picks a random type for the tetromino piece
        /// </summary>
        void NewPiece();

        /// <summary>
        /// Creates a new tetromino piece with the given type.
        /// </summary>
        /// <param name="type">Tetromino type to create</param>
        void NewPiece(ePieceType type);

        /// <summary>
        /// Creates a new instance of a tetromino with the same initial position.
        /// </summary>
        /// <returns> A new instances of Tetromino at initialPos. </returns>
        ITetromino<TPieceType, TDirection, TMoveType> CreatePiece();
    }
}
