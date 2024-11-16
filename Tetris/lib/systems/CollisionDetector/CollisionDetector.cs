using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Exceptions;
using Lechliter.Tetris.Lib.Models;
using Lechliter.Tetris.Lib.Objects;

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

        private ITetromino<ePieceType, eDirection, eMoveType>? Piece;

        private IGrid<ePieceType, eDirection, eMoveType> _Grid;

        public CollisionDetector(IGrid<ePieceType, eDirection, eMoveType> grid)
        {
            LockTimerFalling = new LockTimer(NUM_FALLING_FRAMES);
            LockTimerStationary = new LockTimer(NUM_STATIONARY_FRAMES);
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

            this.Piece = piece;

            foreach (IBlock block in piece.Blocks)
            {
                _Grid.GridPosition(block.Position, out int x, out int y);

                isCollisionDetected = _Grid.IsInBounds(x, y) && _Grid.LockedPieces[x, y] != ePieceType.Empty;
                if (isCollisionDetected)
                {
                    switch (moveType)
                    {
                        case eMoveType.Translation:
                            piece.UndoMove(moveType);
                            RestartFallingTimerWhenFalling();
                            RestartStationaryTimerWhenFalling();
                            break;
                        case eMoveType.Rotation:
                            UndoRotation(x, y);
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

        public bool TryMove(ITetromino<ePieceType, eDirection, eMoveType> piece, ePieceType[,] grid_pieces, params Movement[] moves)
        {
            bool isValidMove = true;

            // Copy tetromino and move copy with specified movement
            ITetromino<ePieceType, eDirection, eMoveType> potential_piece = piece.Copy() as ITetromino<ePieceType, eDirection, eMoveType>;
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
                int x, y;
                _Grid.GridPosition(block.Position, out x, out y);
                if (!_Grid.IsInBounds(x, y) || grid_pieces[x, y] != ePieceType.Empty)
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

        private void RestartFallingTimerWhenFalling()
        {
            if (Piece.Velocity.y < 0)
            {
                // Restart falling timer when bottom of piece has been hit
                if (!LockTimerFalling.IsRunning)
                {
                    RestartFallingTimer();
                }
            }
        }

        private void RestartStationaryTimerWhenFalling()
        {
            if (Piece.Velocity.y < 0)
            {
                if (!LockTimerStationary.IsRunning)
                {
                    RestartStationaryTimer();
                }
            }
        }

        /// <summary>
        /// Determines whether the given list of moves for a piece on the grid is possible without causing any further collisions or leaving bounds.
        /// </summary>
        /// <param name="piece">Piece to check moves on.</param>
        /// <param name="grid_pieces">Grid that contains the piece and locked pieces.</param>
        /// <param name="x">The horizontal matrix index of the block that the piece has intersected with.</param>
        /// <param name="y">The vertical matrix index of the block that the piece has intersected with.</param>
        /// <param name="moves">The list of movements to test on the piece.</param>
        /// <returns>True if the list of movements is possible.</returns>
        private bool DirectionIsPossible(int x, int y, params Movement[] moves)
        {
            bool isPossible = true;

            Func<int, int, int, bool> noBoundaryToLeft = (int x_pivot, int x, int disp) => x_pivot < x && x_pivot - disp > 0;
            Func<int, int, int, bool> noBoundaryToRight = (int x_pivot, int x, int disp) => x_pivot > x && x_pivot + disp < _Grid.BoundsDim.X - 1;
            Func<int, int, bool> noBoundaryAbove = (int y, int disp) => y - disp > 0;
            Func<int, int, bool> noBoundaryBelow = (int y, int disp) => y + disp < _Grid.BoundsDim.Y - 1;

            int x_pivot, y_pivot;
            _Grid.GridPosition(Piece.Position, out x_pivot, out y_pivot);

            foreach (Movement move in moves)
            {
                switch (move.Direction)
                {
                    case eDirection.Up:
                        isPossible &= noBoundaryAbove(y, move.Count);
                        break;
                    case eDirection.Down:
                        isPossible &= noBoundaryBelow(y, move.Count);
                        break;
                    case eDirection.Left:
                        isPossible &= noBoundaryToLeft(x_pivot, x, move.Count);
                        break;
                    case eDirection.Right:
                        isPossible &= noBoundaryToRight(x_pivot, x, move.Count);
                        break;
                }
            }

            if (isPossible)
            {
                isPossible = TryMove(Piece, _Grid.LockedPieces, moves);
            }

            return isPossible;
        }

        private bool MoveToNextEmptySpace(int x, int y)
        {
            List<List<Movement>> possibleMoves = GeneratePossibleMoves();

            bool foundPossibleMove = false;
            foreach (List<Movement> moveSet in possibleMoves)
            {
                foundPossibleMove = DirectionIsPossible(x, y, moveSet.ToArray());
                if (foundPossibleMove)
                {
                    foreach (Movement move in moveSet)
                    {
                        move.ExecuteMove(Piece);
                    }
                    break;
                }
            }
            return foundPossibleMove;
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

        private void UndoRotation(int x, int y)
        {
            bool foundPossibleMove = MoveToNextEmptySpace(x, y);

            if (!foundPossibleMove)
            {
                Piece.UndoMove(eMoveType.Rotation);
            }
        }

        #region Constants

        private const int NUM_FALLING_FRAMES = 8;

        private const int NUM_STATIONARY_FRAMES = 2;

        #endregion
    }

    class CollisionPlayGround
    {
        // TODO: Move piece mpve testing here
    }
}
