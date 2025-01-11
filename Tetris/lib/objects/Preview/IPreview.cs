namespace Lechliter.Tetris.Lib.Objects
{
    /// <summary>
    /// IPreview displays the next tetromino piece to fall.
    /// </summary>
    /// <typeparam name="TPieceType">Enum: type of tetromino piece</typeparam>
    /// <typeparam name="TPieceDirection">Enum: type of direction</typeparam>
    /// <typeparam name="TMoveType">Enum: type of movement</typeparam>
    public interface IPreview<TPieceType> where TPieceType : System.Enum
    {
        /// <summary>
        /// The next piece to fall.
        /// </summary>
        TPieceType PieceType { get; set; }

        /// <summary>
        /// A grid containing the next piece.
        /// </summary>
        TPieceType[,] Grid { get; }

        /// <summary>
        /// Notifies subscribers whenever a the piece is updated.
        /// </summary>
        public event Action PieceUpdated;
    }
}
