using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public struct Movement
    {
        public MoveType moveType;
        public Direction direction;
        public int num_times;

        public Movement(MoveType moveType = MoveType.NotSet, Direction direction = Direction.NotSet, int num_times = 1)
        {
            this.moveType = moveType;
            this.direction = direction;
            this.num_times = num_times;
        }

        public void ExecuteMove(ITetromino<PieceType, Direction, MoveType> piece)
        {
            for (int i = 0; i < num_times; i++)
            {
                (piece as Tetromino).Move(direction, MoveType.Undo);
            }
        }
    }

    public class CollisionDetector : ICollisionDetector<PieceType, Direction, MoveType>
    {
        /* Private Members */
        private static readonly int NUM_FALLING_FRAMES = 8;

        private static readonly int NUM_STATIONARY_FRAMES = 2;

        private ITetromino<PieceType, Direction, MoveType> piece;

        private PieceType[,] grid_pieces;

        /* Public Members */
        public IFrameTimer LockTimerFalling { get; protected set; }
        public IFrameTimer LockTimerStationary { get; protected set; }

        public event Action CollisionDetected;

        public event Action LockPiece;

        /* Constructors */
        public CollisionDetector()
        {
            Initialize();
        }

        /* Private Methods */
        private void Initialize()
        {
            LockTimerFalling = new LockTimer(NUM_FALLING_FRAMES);
            LockTimerStationary = new LockTimer(NUM_STATIONARY_FRAMES);
            InitializeLockPieceWatcher();
        }
        private void InitializeLockPieceWatcher()
        {
            LockTimerFalling.TimerFinished += () =>
            {
                LockPiece?.Invoke();
                LockTimerFalling.Reset();
                (LockTimerFalling as LockTimer).Stop();
            };
            LockTimerStationary.TimerFinished += () =>
            {               
                LockPiece?.Invoke();
                RestartStationaryTimer();             
            };
        }

        private bool isInBounds(int x, int y)
        {
            return x >= 0 && x < Tracker.BOUNDS_DIM.X && y >= 0 && y < Tracker.BOUNDS_DIM.Y;
        }


        private void RestartFallingTimer()
        {
            LockTimerFalling.Reset();
            (LockTimerFalling as LockTimer).Start();
        }

        private void RestartFallingTimerWhenFalling()
        {
            if ((piece as Tetromino).Velocity.y < 0)
            {
                // Restart falling timer when bottom of piece has been hit
                if (!(LockTimerFalling as LockTimer).IsRunning)
                {
                    RestartFallingTimer();
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
            Func<int, int, int, bool> noBoundaryToRight = (int x_pivot, int x, int disp) => x_pivot > x && x_pivot + disp < Tracker.BOUNDS_DIM.X - 1;
            Func<int, int, bool> noBoundaryAbove = (int y, int disp) => y - disp > 0;
            Func<int, int, bool> noBoundaryBelow = (int y, int disp) => y + disp < Tracker.BOUNDS_DIM.Y - 1;

            int x_pivot, y_pivot;
            Tracker.GridPosition(piece.Position, out x_pivot, out y_pivot);

            foreach (Movement move in moves)
            {
                switch (move.direction)
                {
                    case Direction.Up:
                        isPossible &= noBoundaryAbove(y, move.num_times);
                        break;
                    case Direction.Down:
                        isPossible &= noBoundaryBelow(y, move.num_times);
                        break;
                    case Direction.Left:
                        isPossible &= noBoundaryToLeft(x_pivot, x, move.num_times);
                        break;
                    case Direction.Right:
                        isPossible &= noBoundaryToRight(x_pivot, x, move.num_times);
                        break;
                }
            }

            if (isPossible)
            {
                isPossible = TryMove(piece, grid_pieces, moves);
            }

            return isPossible;
        }

        private void MoveToNextEmptySpace(int x, int y)
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
                        move.ExecuteMove(piece);
                    }
                    break;
                }
            }

            if (!foundPossibleMove)
            {
                (piece as Tetromino).UndoMove(MoveType.Rotation);
            }
        }

        private static List<List<Movement>> GeneratePossibleMoves()
        {
            return new List<List<Movement>>()
            {
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Up, 1) },
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Left, 1) },
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Right, 1) },
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Down, 1) },

                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Up, 2) },
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Left, 2) },
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Right, 2) },
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Down, 2) },

                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Up, 1), new Movement(MoveType.Translation, Direction.Left, 1) },
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Up, 1), new Movement(MoveType.Translation, Direction.Right, 1) },
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Down, 1), new Movement(MoveType.Translation, Direction.Left, 1) },
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Down, 1), new Movement(MoveType.Translation, Direction.Right, 1) },

                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Up, 2), new Movement(MoveType.Translation, Direction.Left, 1) },
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Up, 2), new Movement(MoveType.Translation, Direction.Right, 1) },
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Down, 2), new Movement(MoveType.Translation, Direction.Left, 1) },
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Down, 2), new Movement(MoveType.Translation, Direction.Right, 1) },

                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Up, 1), new Movement(MoveType.Translation, Direction.Left, 2) },
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Up, 1), new Movement(MoveType.Translation, Direction.Right, 2) },
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Down, 1), new Movement(MoveType.Translation, Direction.Left, 2) },
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Down, 1), new Movement(MoveType.Translation, Direction.Right, 2) },

                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Up, 2), new Movement(MoveType.Translation, Direction.Left, 2) },
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Up, 2), new Movement(MoveType.Translation, Direction.Right, 2) },
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Down, 2), new Movement(MoveType.Translation, Direction.Left, 2) },
                new List<Movement>(){ new Movement(MoveType.Translation, Direction.Down, 2), new Movement(MoveType.Translation, Direction.Right, 2) },
            };
        }

        [Obsolete]
        private void ParseMovementDirections((int vertical, int horizontal)[] options, int x, int y)
        {
            // TODO: This has bugs, if you can't figure it out, replace it with the original version. This solution isn't that much better anyway...
            Func<int, int, int, bool> noBoundaryToLeft = (int x_pivot, int x, int disp) => x_pivot < x && x_pivot - disp > 0;
            Func<int, int, int, bool> noBoundaryToRight = (int x_pivot, int x, int disp) => x_pivot > x && x_pivot + disp < Tracker.BOUNDS_DIM.X - 1;

            int x_pivot, y_pivot;
            Tracker.GridPosition(piece.Position, out x_pivot, out y_pivot);

            bool undoMove = true;
            foreach ((int vertical, int horizontal) in options)
            {
                Dictionary<Direction, bool> possibleDirections = new Dictionary<Direction, bool>()
                {
                    { Direction.Up,  vertical > 0 && y - vertical > 0 },
                    { Direction.Down, vertical > 0 && y + vertical < Tracker.BOUNDS_DIM.Y - 1 },
                    { Direction.Left, horizontal > 0 && noBoundaryToLeft(x_pivot, x, horizontal) },
                    { Direction.Right, horizontal > 0 && noBoundaryToRight(x_pivot, x, horizontal) }
                };

                List<Movement> vertical_left = new List<Movement>();
                List<Movement> vertical_right = new List<Movement>();

                foreach (KeyValuePair<Direction, bool> possibleDirection in possibleDirections)
                {
                    if (possibleDirection.Value)
                    {
                        if(possibleDirection.Key == Direction.Left)
                        {
                            vertical_left.Add(new Movement(MoveType.Translation, possibleDirection.Key, num_times: horizontal));
                        }
                        else if (possibleDirection.Key == Direction.Right)
                        {
                            vertical_right.Add(new Movement(MoveType.Translation, possibleDirection.Key, num_times: horizontal));
                        }
                        else
                        {
                            vertical_left.Add(new Movement(MoveType.Translation, possibleDirection.Key, num_times: vertical));
                            vertical_right.Add(new Movement(MoveType.Translation, possibleDirection.Key, num_times: vertical));
                        }
                    }
                }

                bool canMoveUpLeft = TryMove(piece, grid_pieces, vertical_left.ToArray());
                bool canMoveUpRight = TryMove(piece, grid_pieces, vertical_right.ToArray());

                if (canMoveUpLeft)
                {
                    foreach (Movement movement in vertical_left)
                    {
                        movement.ExecuteMove(piece);
                    }
                    undoMove = false;
                    break;
                }else if (canMoveUpRight)
                {
                    foreach (Movement movement in vertical_right)
                    {
                        movement.ExecuteMove(piece);
                    }
                    undoMove = false;
                    break;
                }
            }
            if (undoMove)
            {
                (piece as Tetromino).UndoMove(MoveType.Rotation);
            }
        }

        private void UndoRotation(int x, int y)
        {
            (int vertical, int horizontal)[] options = { (1, 0), (0, 1), (2, 0), (0, 2), (1, 1), (2, 1), (1, 2) , (2, 2) };
            MoveToNextEmptySpace(x, y);
        }

        /* Public Methods */
        public void RestartStationaryTimer()
        {
            LockTimerStationary.Reset();
            (LockTimerStationary as LockTimer).Start();
            //Console.WriteLine("Stationary Timer Restarted!");
        }

        public bool DetectCollisions(ITetromino<PieceType, Direction, MoveType> piece, PieceType[,] grid_pieces, MoveType moveType)
        {
            bool isCollisionDetected = false;

            this.piece = piece;
            this.grid_pieces = grid_pieces;

            foreach (IBlock block in piece.Blocks)
            {
                int x, y;
                Tracker.GridPosition(block.Position, out x, out y);

                isCollisionDetected = isInBounds(x, y) && grid_pieces[x, y] != PieceType.Empty;
                if (isCollisionDetected)
                {
                    switch (moveType)
                    {
                        case MoveType.Translation:
                            (piece as Tetromino).UndoMove(moveType);
                            RestartFallingTimerWhenFalling();
                            break;
                        case MoveType.Rotation:
                            UndoRotation(x, y);
                            break;
                        default:
                            Console.WriteLine("Unhandled collision from move type: {0}", moveType.ToString());
                            break;
                    }
                    CollisionDetected?.Invoke();
                    break;
                }
            }

            if (!isCollisionDetected && (piece as Tetromino).Velocity.y > 0)
            {
                (LockTimerFalling as LockTimer).Stop();
            }

            return isCollisionDetected;
        }

        /// <summary>
        /// Determines whether the given list of moves for a piece on the grid is possible without causing any further collisions. 
        /// </summary>
        /// <param name="piece">Piece to check moves on.</param>
        /// <param name="grid_pieces">Grid that contains the piece and locked pieces.</param>
        /// <param name="moves">The list of movements to test on the piece.</param>
        /// <returns>True if the list of movements is possible.</returns>
        public bool TryMove(ITetromino<PieceType, Direction, MoveType> piece, PieceType[,] grid_pieces, params Movement[] moves)
        {
            bool isValidMove = true;

            // Copy tetromino and move copy with specified movement
            Tetromino potential_piece = (piece as Tetromino).Copy();
            foreach (Movement movement in moves)
            {
                switch (movement.moveType)
                {
                    case MoveType.Translation:
                        for(int i = 0; i < movement.num_times; i++)
                            potential_piece.Move(movement.direction);
                        break;
                    case MoveType.Rotation:
                        for (int i = 0; i < movement.num_times; i++)
                            potential_piece.Rotate(movement.direction);
                        break;
                }
            }
            // Check if copy is colliding with any grid pieces
            foreach (IBlock block in potential_piece.Blocks)
            {
                int x, y;
                Tracker.GridPosition(block.Position, out x, out y);
                if (!isInBounds(x, y) || grid_pieces[x, y] != PieceType.Empty)
                {
                    isValidMove = false;
                    break;
                }
            }

            return isValidMove;
        }
    }
}
