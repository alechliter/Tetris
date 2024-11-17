using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Exceptions;
using Lechliter.Tetris.Lib.Models;
using Lechliter.Tetris.Lib.Objects;
using Tetris.lib.Design.Helpers;

namespace Lechliter.Tetris.Lib.Systems
{
    public class CollisionDetector : ICollisionDetector<ePieceType, eDirection, eMoveType>
    {
        public IFrameTimer LockTimerFalling { get; protected set; }

        public IFrameTimer LockTimerStationary { get; protected set; }

        #region Events

        public event Action? CollisionDetected;

        public event Action? LockPiece;

        #endregion

        private IGrid<ePieceType, eDirection, eMoveType> _Grid;

        public CollisionDetector(IGrid<ePieceType, eDirection, eMoveType> grid)
        {
            LockTimerFalling = new LockTimer(
                frame_count: ConfigurationHelper.GetInt("LockTimerFallingFrameCount", DEFAULT_NUM_FALLING_FRAMES)
            );
            LockTimerStationary = new LockTimer(
                frame_count: ConfigurationHelper.GetInt("LockTimerStationaryFrameCount", DEFAULT_NUM_STATIONARY_FRAMES)
            );
            _Grid = grid;
            InitializeLockPieceWatcher();
        }

        public void StopAndResetStationaryTimer()
        {
            LockTimerStationary.Stop();
            LockTimerStationary.Reset();
        }

        public bool DetectCollisions(ITetromino<ePieceType, eDirection, eMoveType> piece, eMoveType moveType)
        {
            bool isCollisionDetected = false;

            foreach (IBlock block in piece.Blocks)
            {
                _Grid.GridPosition(block.Position, out int x, out int y);

                isCollisionDetected = _Grid.IsInBounds(x, y) && !_Grid.IsCellEmpty(x, y);
                if (isCollisionDetected)
                {
                    switch (moveType)
                    {
                        case eMoveType.Translation:
                            piece.UndoMove(moveType);
                            RestartFallingTimerWhenFalling(piece);
                            RestartStationaryTimerWhenFalling(piece);
                            break;
                        case eMoveType.Rotation:
                            OnRotateCollision(piece, x, y);
                            break;
                        default:
                            throw new TetrisLibException($"Collision Detector: Unhandled collision from move type: {moveType.ToString()}");
                    }
                    CollisionDetected?.Invoke();
                    break;
                }
            }

            if (!isCollisionDetected && piece.Velocity.y > 0)
            {
                LockTimerFalling.Stop();
            }

            return isCollisionDetected;
        }

        public bool TryMove(ITetromino<ePieceType, eDirection, eMoveType> piece, params Movement[] moves)
        {
            bool isValidMove = true;

            // Copy tetromino and move copy with specified movement
            ITetromino<ePieceType, eDirection, eMoveType> potential_piece = piece.Copy();
            foreach (Movement movement in moves)
            {
                switch (movement.MoveType)
                {
                    case eMoveType.Translation:
                        for (int i = 0; i < movement.Count; i++)
                        {
                            potential_piece.Move(movement.Direction);
                        }
                        break;
                    case eMoveType.Rotation:
                        for (int i = 0; i < movement.Count; i++)
                        {
                            potential_piece.Rotate(movement.Direction);
                        }
                        break;
                }
            }

            // Check if copy is colliding with any grid pieces
            foreach (IBlock block in potential_piece.Blocks)
            {
                _Grid.GridPosition(block.Position, out int x, out int y);
                if (!_Grid.IsInBounds(x, y) || !_Grid.IsCellEmpty(x, y))
                {
                    isValidMove = false;
                    break;
                }
            }

            return isValidMove;
        }

        private void InitializeLockPieceWatcher()
        {
            LockTimerFalling.TimerFinished += () =>
            {
                LockPiece?.Invoke();
                LockTimerFalling.Reset();
                LockTimerFalling.Stop();
            };
            LockTimerStationary.TimerFinished += () =>
            {
                LockPiece?.Invoke();
                LockTimerStationary.Reset();
                LockTimerStationary.Stop();
            };
        }


        private void RestartFallingTimer()
        {
            LockTimerFalling.Reset();
            LockTimerFalling.Start();
        }

        private void RestartStationaryTimer()
        {
            LockTimerStationary.Reset();
            LockTimerStationary.Start();
        }

        private void RestartFallingTimerWhenFalling(ITetromino<ePieceType, eDirection, eMoveType> piece)
        {
            if (piece.Velocity.y < 0)
            {
                // Restart falling timer when bottom of piece has been hit
                if (!LockTimerFalling.IsRunning)
                {
                    RestartFallingTimer();
                }
            }
        }

        private void RestartStationaryTimerWhenFalling(ITetromino<ePieceType, eDirection, eMoveType> piece)
        {
            if (piece.Velocity.y < 0)
            {
                if (!LockTimerStationary.IsRunning)
                {
                    RestartStationaryTimer();
                }
            }
        }

        private bool MoveToNextEmptySpace(ITetromino<ePieceType, eDirection, eMoveType> piece, int x, int y)
        {
            List<List<Movement>> possibleMoves = GeneratePossibleMoves();

            List<Movement> possibleMoveSet = new List<Movement>();
            foreach (List<Movement> moveSet in possibleMoves)
            {
                if (TryMove(piece, moveSet.ToArray()))
                {
                    possibleMoveSet = moveSet;
                    break;
                }
            }
            foreach (Movement move in possibleMoveSet)
            {
                move.ExecuteMove(piece);
            }
            return possibleMoveSet.Any();
        }

        private static List<List<Movement>> GeneratePossibleMoves()
        {
            return new List<List<Movement>>()
            {
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Up, 1) },
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Left, 1) },
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Right, 1) },
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Down, 1) },

                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Up, 2) },
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Left, 2) },
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Right, 2) },
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Down, 2) },

                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Up, 1), new Movement(eMoveType.Translation, eDirection.Left, 1) },
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Up, 1), new Movement(eMoveType.Translation, eDirection.Right, 1) },
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Down, 1), new Movement(eMoveType.Translation, eDirection.Left, 1) },
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Down, 1), new Movement(eMoveType.Translation, eDirection.Right, 1) },

                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Up, 2), new Movement(eMoveType.Translation, eDirection.Left, 1) },
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Up, 2), new Movement(eMoveType.Translation, eDirection.Right, 1) },
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Down, 2), new Movement(eMoveType.Translation, eDirection.Left, 1) },
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Down, 2), new Movement(eMoveType.Translation, eDirection.Right, 1) },

                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Up, 1), new Movement(eMoveType.Translation, eDirection.Left, 2) },
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Up, 1), new Movement(eMoveType.Translation, eDirection.Right, 2) },
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Down, 1), new Movement(eMoveType.Translation, eDirection.Left, 2) },
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Down, 1), new Movement(eMoveType.Translation, eDirection.Right, 2) },

                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Up, 2), new Movement(eMoveType.Translation, eDirection.Left, 2) },
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Up, 2), new Movement(eMoveType.Translation, eDirection.Right, 2) },
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Down, 2), new Movement(eMoveType.Translation, eDirection.Left, 2) },
                new List<Movement>(){ new Movement(eMoveType.Translation, eDirection.Down, 2), new Movement(eMoveType.Translation, eDirection.Right, 2) },
            };
        }

        private void OnRotateCollision(ITetromino<ePieceType, eDirection, eMoveType> piece, int x, int y)
        {
            bool foundPossibleMove = MoveToNextEmptySpace(piece, x, y);

            if (!foundPossibleMove)
            {
                piece.UndoMove(eMoveType.Rotation);
            }
        }

        #region Constants

        private const int DEFAULT_NUM_FALLING_FRAMES = 8;

        private const int DEFAULT_NUM_STATIONARY_FRAMES = 2;

        #endregion
    }
}
