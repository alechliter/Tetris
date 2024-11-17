using Lechliter.Tetris.Lib.Definitions;

namespace Lechliter.Tetris.Lib.Objects
{
    /// <summary>
    /// IPreview displays the next tetromino piece to fall.
    /// </summary>
    /// <typeparam name="TPieceType">Enum: type of tetromino piece</typeparam>
    /// <typeparam name="TPieceDirection">Enum: type of direction</typeparam>
    /// <typeparam name="TMoveType">Enum: type of movement</typeparam>
    public interface IPreview<TPieceType, TPieceDirection, TMoveType> where TPieceType : System.Enum
        where TPieceDirection : System.Enum where TMoveType : System.Enum
    {
        /// <summary>
        /// The next piece to fall.
        /// </summary>
        ITetromino<TPieceType, TPieceDirection, TMoveType> Piece { get; }

        /// <summary>
        /// A grid containing the next piece.
        /// </summary>
        TPieceType[,] Grid { get; }

        /// <summary>
        /// Notifies subscribers whenever a the piece is updated.
        /// </summary>
        public event Action PieceUpdated;

        /// <summary>
        /// Gets a new random piece and updates the grid.
        /// </summary>
        void NewPiece();

        /// <summary>
        /// Gets a new piece with the given type and updates the grid.
        /// </summary>
        /// <param name="type">Type of piece to create</param>
        void NewPiece(ePieceType type);
    }
}
