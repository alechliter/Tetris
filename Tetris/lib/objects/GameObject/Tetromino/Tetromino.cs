using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Exceptions;
using Lechliter.Tetris.Lib.Systems;
using Lechliter.Tetris.Lib.Types;
using Tetris.lib.Design.Helpers;

namespace Lechliter.Tetris.Lib.Objects
{
    public class Tetromino : ITetromino<ePieceType, eDirection, eMoveType>
    {

        #region Public Members

        public ICollection<IBlock> Blocks { get; private set; }

        public ePieceType Type { get; private set; }

        public Point Position { get { return Pivot; } }

        public Point Velocity { get; private set; }

        public eDirection Rotation { get; private set; }

        public event Action<eMoveType>? UpdatePosition;

        #endregion

        #region Private Members

        private Point Pivot;

        private Point InitialPos;

        private double Angle;

        private static int BlocksCount
        {
            get
            {
                return ConfigurationHelper.GetInt("BlocksPerTetrominoCount", DEFAULT_BLOCKS_COUNT);
            }
        }

        #endregion

        public Tetromino(Point initialPos, ePieceType pieceType)
        {
            this.InitialPos = initialPos;
            Blocks = new List<IBlock>();
            Velocity = new Point();
            InitializePiece(pieceType);
        }

        #region  Public Methods

        public void Move(eDirection direction, bool emitEvent = true)
        {
            move_blocks(direction);

            if (emitEvent)
            {
                UpdatePosition?.Invoke(eMoveType.Translation);
            }
        }

        public void Rotate(eDirection direction)
        {
            double angle = 0.0;

            switch (direction)
            {
                case eDirection.Left:
                    angle = -Math.PI / 2.0; // Rotate 90 degrees to the left
                    break;
                case eDirection.Right:
                    angle = Math.PI / 2.0; // Rotate 90 degrees to the right
                    break;
                default:
                    throw new TetrisLibException("Tetromino: Invalid rotation direction.");
            }

            foreach (IBlock block in Blocks)
            {
                RotateAboutPivot(block, angle);
            }

            Rotation = direction;
            this.Angle = angle;
            UpdatePosition?.Invoke(eMoveType.Rotation);

        }

        public void Drop(ITracker<ePieceType, eDirection, eMoveType> tracker)
        {
            while (!tracker.IsCollision(eMoveType.Translation))
            {
                move_blocks(eDirection.Down);
            }
        }

        public void UndoMove(eMoveType moveType)
        {
            switch (moveType)
            {
                case eMoveType.Translation:
                    Velocity = -1 * Velocity;
                    MoveBlocksBy(Velocity);
                    UpdatePosition?.Invoke(eMoveType.Undo);
                    break;
                case eMoveType.Rotation:
                    foreach (IBlock block in Blocks)
                    {
                        RotateAboutPivot(block, -Angle);
                    }
                    UpdatePosition?.Invoke(eMoveType.Undo);
                    break;
            }
        }

        public ITetromino<ePieceType, eDirection, eMoveType> Copy()
        {
            Tetromino copy_piece = new Tetromino(Position, Type);

            copy_piece.Pivot = Pivot.Copy();
            copy_piece.Blocks = new List<IBlock>();
            foreach (IBlock block in Blocks)
            {
                copy_piece.Blocks.Add(block.Copy());
            }
            copy_piece.Velocity = Velocity.Copy();
            copy_piece.Rotation = Rotation;
            copy_piece.Angle = Angle;

            return copy_piece;
        }

        #endregion

        #region  Private Methods
        private void InitializePiece(ePieceType type)
        {
            Pivot = this.InitialPos;

            this.Type = type;

            Blocks = new List<IBlock>(BlocksCount);
            ConstructTetromino(Blocks, type, ref Pivot);

            UpdatePosition?.Invoke(eMoveType.Spawn);
        }

        private void MoveBlocksBy(Point vector)
        {
            Pivot += vector;
            foreach (IBlock block in Blocks)
            {
                block.MoveBy(vector);
            }
        }

        private void RotateAboutPivot(IBlock block, double angle)
        {
            // Rotation axis vector
            Point vector = block.Position - Pivot;

            // rotates vector that points from the pivot to the center of the block
            float newX, newY;
            newX = vector.x * (float)Math.Cos(angle) - vector.y * (float)Math.Sin(angle);
            newY = vector.x * (float)Math.Sin(angle) + vector.y * (float)Math.Cos(angle);

            vector.x = newX;
            vector.y = newY;

            block.MoveTo(vector + Pivot);
        }

        private static void ConstructTetromino(ICollection<IBlock> blocks, ePieceType type, ref Point pivot)
        {
            switch (type)
            {
                case ePieceType.I:
                    pivot += new Point(Block.StandardDim.X / 2.0f, Block.StandardDim.Y / 2.0f); // offsets pivot from the center of the grid
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X / 2.0f, 3 * Block.StandardDim.Y / 2.0f)));
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X / 2.0f, Block.StandardDim.Y / 2.0f)));
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X / 2.0f, -Block.StandardDim.Y / 2.0f)));
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X / 2.0f, -3 * Block.StandardDim.Y / 2.0f)));
                    break;
                case ePieceType.O:
                    pivot += new Point(Block.StandardDim.X / 2.0f, Block.StandardDim.Y / 2.0f); // offsets pivot from the center of the grid
                    blocks.Add(new Block(pivot + new Point(Block.StandardDim.X / 2.0f, Block.StandardDim.Y / 2.0f)));
                    blocks.Add(new Block(pivot + new Point(Block.StandardDim.X / 2.0f, -Block.StandardDim.Y / 2.0f)));
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X / 2.0f, Block.StandardDim.Y / 2.0f)));
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X / 2.0f, -Block.StandardDim.Y / 2.0f)));
                    break;
                case ePieceType.T:
                    blocks.Add(new Block(pivot));
                    blocks.Add(new Block(pivot + new Point(Block.StandardDim.X, 0.0f)));
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X, 0.0f)));
                    blocks.Add(new Block(pivot + new Point(0.0f, -Block.StandardDim.Y)));
                    break;
                case ePieceType.J:
                    blocks.Add(new Block(pivot));
                    blocks.Add(new Block(pivot + new Point(0.0f, -Block.StandardDim.Y)));
                    blocks.Add(new Block(pivot + new Point(0.0f, Block.StandardDim.Y)));
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X, Block.StandardDim.Y)));
                    break;
                case ePieceType.L:
                    blocks.Add(new Block(pivot));
                    blocks.Add(new Block(pivot + new Point(0.0f, -Block.StandardDim.Y)));
                    blocks.Add(new Block(pivot + new Point(0.0f, Block.StandardDim.Y)));
                    blocks.Add(new Block(pivot + new Point(Block.StandardDim.X, Block.StandardDim.Y)));
                    break;
                case ePieceType.S:
                    blocks.Add(new Block(pivot));
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X, 0.0f)));
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X, -Block.StandardDim.Y)));
                    blocks.Add(new Block(pivot + new Point(0.0f, Block.StandardDim.Y)));
                    break;
                case ePieceType.Z:
                    blocks.Add(new Block(pivot));
                    blocks.Add(new Block(pivot + new Point(Block.StandardDim.X, 0.0f)));
                    blocks.Add(new Block(pivot + new Point(Block.StandardDim.X, -Block.StandardDim.Y)));
                    blocks.Add(new Block(pivot + new Point(0.0f, Block.StandardDim.Y)));
                    break;
                case ePieceType.NotSet:
                    break;
                default:
                    throw new TetrisLibException("Tetromino: Invalid Piece Type");
            }
        }

        private void move_blocks(eDirection direction)
        {
            Point velocity = new Point();
            switch (direction)
            {
                case eDirection.Up:
                    velocity.y -= Block.StandardDim.Y;
                    MoveBlocksBy(velocity);
                    break;
                case eDirection.Down:
                    velocity.y += Block.StandardDim.Y;
                    MoveBlocksBy(velocity);
                    break;
                case eDirection.Left:
                    velocity.x -= Block.StandardDim.X;
                    MoveBlocksBy(velocity);
                    break;
                case eDirection.Right:
                    velocity.x += Block.StandardDim.X;
                    MoveBlocksBy(velocity);
                    break;
                default:
                    throw new TetrisLibException("Tetromino: Invalid direction");
            }
            this.Velocity = velocity;
        }

        #endregion

        #region Constants

        private const int DEFAULT_BLOCKS_COUNT = 4;

        #endregion
    }
}