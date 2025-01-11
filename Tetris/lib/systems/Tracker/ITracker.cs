namespace Lechliter.Tetris.Lib.Systems
{
    /// <summary>
    /// ITracker records where the tetromino is with relation to the stationary blocks and the boundaries of the
    /// grid.
    /// </summary>
    /// <typeparam name="TPieceType">Enum: type of tetromino piece</typeparam>
    /// <typeparam name="TDirection">Enum: direction of movement</typeparam>
    public interface ITracker<TPieceType, TDirection, TMoveType> where TPieceType : System.Enum
                                                    where TDirection : System.Enum
                                                    where TMoveType : System.Enum

    {
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

        public void MovePiece(TDirection direction);

        public void RotatePiece(TDirection direction);

        public void DropPiece();

        public void LoadNextPiece();

        public void UpdateGrid();
        public void UpdateGrid(TMoveType moveType);

        public void HoldPiece();

        public bool IsCollision(TMoveType moveType);

        public void ResetStationaryTimer();

        public void NextFrame();
    }
}
