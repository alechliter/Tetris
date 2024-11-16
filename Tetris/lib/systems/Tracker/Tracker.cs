using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Objects;
using Lechliter.Tetris.Lib.Types;

namespace Lechliter.Tetris.Lib.Systems
{
    public class Tracker : ITracker<ePieceType, eDirection, eMoveType>
    {
        public ITetromino<ePieceType, eDirection, eMoveType> CurrentPiece { get; protected set; }

        public IPreview<ePieceType, eDirection, eMoveType> NextPiece { get; }

        public IPreview<ePieceType, eDirection, eMoveType> HeldPiece { get; }

        public event Action GameOver;

        public event Action<int> LinesCleared;

        public event Action PieceLocked;

        private readonly Point SpawnPoint;

        private readonly ICollisionDetector<ePieceType, eDirection, eMoveType> _CollisionDetector;

        private readonly IGrid<ePieceType, eDirection, eMoveType> _Grid;

        private readonly IFrame _Frame;

        private readonly IInputHandler<ConsoleKey, Action> _InputHandler;

        private readonly IScore _Score;

        private bool CanHoldPiece = true;

        public Tracker(
            IGrid<ePieceType, eDirection, eMoveType> grid,
            ICollisionDetector<ePieceType, eDirection, eMoveType> collisionDetector,
            IFrame frame,
            IInputHandler<ConsoleKey, Action> inputHandler,
            IScore score)
        {
            _CollisionDetector = collisionDetector;
            _Grid = grid;
            _Frame = frame;
            _InputHandler = inputHandler;
            _Score = score;

            SpawnPoint = new Point(grid.BoundsDim.X / 2 - 1, 0);
            CurrentPiece = new Tetromino(_Frame, SpawnPoint);
            NextPiece = new Preview();
            HeldPiece = new Preview(ePieceType.NotSet);

            _Grid.AddTetromino(CurrentPiece);

            StartSubscriptions();
        }

        #region Public Methods

        public void UpdateGrid(eMoveType _moveType)
        {
            _Grid.AddTetromino(CurrentPiece);
        }

        public void LockPiece()
        {
            _Grid.LockPieces();
            PieceLocked?.Invoke();
            ClearLines();
            ResetStationaryTimer();
            if (!IsGameOver())
            {
                CurrentPiece.NewPiece(NextPiece.Piece.Type);
                NextPiece.NewPiece();
                CanHoldPiece = true;
            }
        }

        public void HoldPiece()
        {
            if (CanHoldPiece)
            {
                if (HeldPiece.Piece.Type != ePieceType.NotSet)
                {
                    Tetromino temp = CurrentPiece.Copy() as Tetromino;
                    CurrentPiece.NewPiece(HeldPiece.Piece.Type);
                    HeldPiece.NewPiece(temp.Type);
                }
                else
                {
                    HeldPiece.NewPiece(CurrentPiece.Type);
                    CurrentPiece.NewPiece(NextPiece.Piece.Type);
                    NextPiece.NewPiece();
                }
                CanHoldPiece = false;
            }
        }

        public void MovePiece(eDirection direction)
        {
            CurrentPiece.Move(direction);
        }

        public void RotatePiece(eDirection direction)
        {
            CurrentPiece.Rotate(direction);
        }

        public void DropPiece()
        {
            CurrentPiece.Drop(this);
        }

        public void LoadNewPiece()
        {
            CurrentPiece.NewPiece();
        }

        public bool IsCollision(eMoveType moveType)
        {
            return _CollisionDetector.DetectCollisions(CurrentPiece, moveType);
        }

        public void ResetStationaryTimer()
        {
            _CollisionDetector.StopAndResetStationaryTimer();
        }

        public void NextFrame()
        {
            _CollisionDetector.LockTimerFalling.CountDown();
            _CollisionDetector.LockTimerStationary.CountDown();
        }

        #endregion

        #region Private Methods

        private void StartSubscriptions()
        {
            // Subscribes to changes in the position
            CurrentPiece.UpdatePosition += DetectCollisions;
            CurrentPiece.UpdatePosition += UpdateGrid;

            // Subscribe to timer events
            _CollisionDetector.LockPiece += LockPiece;

            // advance frame timers
            _Frame.FrameAction += NextFrame;

            _InputHandler.AnyKeyEvent += ResetStationaryTimer;

            _Score.NextLevel += _Frame.SpeedUp;
            LinesCleared += _Score.Increase;
        }

        private void DetectCollisions(eMoveType moveType)
        {
            _CollisionDetector.DetectCollisions(CurrentPiece, moveType);
        }

        private void ClearLines()
        {
            int numLines = _Grid.ClearLines();
            if (numLines > 0)
            {
                LinesCleared?.Invoke(numLines);
            }
        }

        /// <summary>
        /// Alerts all GameOver subscribers if a piece is out of bounds at the top of the board.
        /// </summary>
        /// <returns>True if the game is over.</returns>
        private bool IsGameOver()
        {
            bool isOver = !_Grid.IsLineEmpty(0);
            if (isOver)
            {
                GameOver?.Invoke();
            }
            return isOver;
        }

        private Block[] copyBlocks()
        {
            Block[] blocks = new Block[CurrentPiece.Blocks.Count];
            CurrentPiece.Blocks.CopyTo(blocks, 0);
            return blocks;
        }

        #endregion
    }
}
