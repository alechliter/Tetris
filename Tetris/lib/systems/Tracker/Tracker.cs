using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Exceptions;
using Lechliter.Tetris.Lib.Objects;
using Lechliter.Tetris.Lib.Types;
using Tetris.lib.Design.Helpers;

namespace Lechliter.Tetris.Lib.Systems
{
    public class Tracker : ITracker<ePieceType, eDirection, eMoveType>
    {
        public event Action? GameOver;

        public event Action<int>? LinesCleared;

        public event Action? PieceLocked;

        private ITetromino<ePieceType, eDirection, eMoveType> CurrentPiece
        {
            get
            {
                if (_CurrentPiece == null)
                {
                    throw new TetrisLibException(ERROR_CURRENT_PIECE_BEFORE_INIT);
                }
                return _CurrentPiece;
            }
            set
            {
                _CurrentPiece = value;
                UpdateGrid();
            }
        }

        private bool CanHoldPiece = true;

        private ITetromino<ePieceType, eDirection, eMoveType>? _CurrentPiece;

        private readonly Point SpawnPoint;

        private readonly ICollisionDetector<ePieceType, eDirection, eMoveType> _CollisionDetector;

        private readonly ITetrominoQueue<ePieceType> _TetrominoQueue;

        private readonly ITetrominoStash<ePieceType> _TetrominoStash;

        private readonly IGrid<ePieceType, eDirection, eMoveType> _Grid;

        private readonly IFrame _Frame;

        private readonly IInputHandler<ConsoleKey, Action> _InputHandler;

        private readonly IScore _Score;

        public Tracker(
            IGrid<ePieceType, eDirection, eMoveType> grid,
            ICollisionDetector<ePieceType, eDirection, eMoveType> collisionDetector,
            ITetrominoQueue<ePieceType> tetrominoQueue,
            ITetrominoStash<ePieceType> tetrominoStash,
            IFrame frame,
            IInputHandler<ConsoleKey, Action> inputHandler,
            IScore score)
        {
            _CollisionDetector = collisionDetector;
            _TetrominoQueue = tetrominoQueue;
            _TetrominoStash = tetrominoStash;
            _Grid = grid;
            _Frame = frame;
            _InputHandler = inputHandler;
            _Score = score;

            SpawnPoint = new Point(grid.BoundsDim.X / 2 - 1, -2);

            LoadNextPiece();

            StartSubscriptions();
        }

        #region Public Methods

        public void UpdateGrid()
        {
            UpdateGrid(eMoveType.NotSet);
        }

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
                LoadNextPiece();
                CanHoldPiece = true;
            }
        }

        public void HoldPiece()
        {
            if (CanHoldPiece)
            {
                ePieceType heldPieceType = _TetrominoStash.Hold(CurrentPiece.Type);
                if (heldPieceType == ePieceType.NotSet)
                {
                    LoadNextPiece();
                }
                else
                {
                    CurrentPiece = NewPiece(heldPieceType);
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

        public void LoadNextPiece()
        {
            ePieceType nextPieceType = _TetrominoQueue.Next();
            CurrentPiece = NewPiece(nextPieceType);
            _Grid.AddTetromino(CurrentPiece);
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
            CurrentPiece.Move(eDirection.Down);

            _CollisionDetector.LockTimerFalling.CountDown();
            _CollisionDetector.LockTimerStationary.CountDown();
        }

        #endregion

        #region Private Methods

        private void StartSubscriptions()
        {
            // Subscribe to timer events
            _CollisionDetector.LockPiece += LockPiece;

            // advance frame timers
            _Frame.FrameAction += NextFrame;
            _Frame.SpeedChange += OnSpeedChange;

            _InputHandler.AnyKeyEvent += ResetStationaryTimer;

            _Score.NextLevel += OnNextLevel;
            LinesCleared += _Score.Increase;
        }

        private ITetromino<ePieceType, eDirection, eMoveType> NewPiece(ePieceType pieceType)
        {
            ITetromino<ePieceType, eDirection, eMoveType> newPiece = new Tetromino(SpawnPoint, pieceType);

            newPiece.UpdatePosition += DetectCollisions;
            newPiece.UpdatePosition += UpdateGrid;

            _Grid.AddTetromino(newPiece);

            return newPiece;
        }

        private void OnNextLevel(int level)
        {
            _Frame.SpeedUp(level * ConfigurationHelper.GetInt("NextLevelFrameRateChange"));
        }

        private void OnSpeedChange(long currentIntervalMS, long initialIntervalMS)
        {
            _CollisionDetector.OnSpeedChange(currentIntervalMS, initialIntervalMS);
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

        #region Constants

        const string ERROR_CURRENT_PIECE_BEFORE_INIT = "Tracker current piece accessed before initialized.";

        #endregion
    }
}
