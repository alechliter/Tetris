using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    /// <summary>
    /// Type of tetromino piece. 
    /// Pieces: I, O, T, J, L, S, Z, 
    /// X - Locked Piece,
    /// E - Empty space
    /// </summary>
    public enum PieceType
    {
        Empty, Locked, I, O, T, J, L, S, Z
    }
    /// <summary>
    /// Direction of travel.
    /// Directions: Up, Down, Left, Right
    /// </summary>
    public enum Direction
    {
        NotSet, Up, Down, Left, Right
    }

    public enum MoveType
    {
        NotSet, Translation, Rotation, Spawn, Undo
    }

    public class Tetromino : ITetromino<PieceType, Direction, MoveType>
    {
        /* Private members */
        private Point pivot;
        private Point initialPos;
        private Point velocity;
        private double angle;
        private Direction rotation_direction;
        private PieceType type;
        private static Random rand;
        private static readonly int NUM_TYPES = 7;
        private static readonly int NUM_BLOCKS = 4;

        /* Public members */
        public ICollection<IBlock> Blocks { get; set; }
        public Point Position { get { return pivot; } }
        public PieceType Type { get { return type; } }

        public Point Velocity { get { return velocity; } }

        public Direction Rotation {  get { return rotation_direction; } }

        public event Action<MoveType> UpdatePosition;

        /* Constructor ------------------------------------------------------------------------*/
        public Tetromino(Point initialPos)
        {
            Initialize(initialPos);
        }

        /* Private Methods --------------------------------------------------------------------*/
        private void Initialize(Point initialPos)
        {
            this.initialPos = initialPos;
            velocity = new Point();
            NewPiece();
        }
        private void MoveBlocksBy(Point vector)
        {
            pivot += vector;
            foreach (IBlock block in Blocks){
                block.MoveBy(vector);
            }
        }

        private void RotateAboutPivot(IBlock block, double angle) {
            // Rotation axis vector
            Point vector = block.Position - pivot;

            // rotates vector that points from the pivot to the center of the block
            float newX, newY;
            newX = vector.x * (float)Math.Cos(angle) - vector.y * (float)Math.Sin(angle);
            newY = vector.x * (float)Math.Sin(angle) + vector.y * (float)Math.Cos(angle);
            
            vector.x = newX;
            vector.y = newY;

            block.MoveTo(vector + pivot);
        }
        private static void ConstructTetromino(ICollection<IBlock> blocks, PieceType type, ref Point pivot)
        {
            switch (type)
            {
                case PieceType.I:
                    pivot += new Point(Block.StandardDim.X / 2.0f, Block.StandardDim.Y / 2.0f); // offsets pivot from the center of the grid
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X / 2.0f, 3 * Block.StandardDim.Y / 2.0f)));
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X / 2.0f, Block.StandardDim.Y / 2.0f)));
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X / 2.0f, -Block.StandardDim.Y / 2.0f)));
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X / 2.0f, -3 * Block.StandardDim.Y / 2.0f)));
                    break;
                case PieceType.O:
                    pivot += new Point(Block.StandardDim.X / 2.0f, Block.StandardDim.Y / 2.0f); // offsets pivot from the center of the grid
                    blocks.Add(new Block(pivot + new Point(Block.StandardDim.X / 2.0f, Block.StandardDim.Y / 2.0f)));
                    blocks.Add(new Block(pivot + new Point(Block.StandardDim.X / 2.0f, -Block.StandardDim.Y / 2.0f)));
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X / 2.0f, Block.StandardDim.Y / 2.0f)));
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X / 2.0f, -Block.StandardDim.Y / 2.0f)));
                    break;
                case PieceType.T:
                    blocks.Add(new Block(pivot));
                    blocks.Add(new Block(pivot + new Point(Block.StandardDim.X , 0.0f)));
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X, 0.0f)));
                    blocks.Add(new Block(pivot + new Point(0.0f, -Block.StandardDim.Y)));
                    break;
                case PieceType.J:
                    blocks.Add(new Block(pivot));
                    blocks.Add(new Block(pivot + new Point(0.0f, -Block.StandardDim.Y)));
                    blocks.Add(new Block(pivot + new Point(0.0f, Block.StandardDim.Y)));
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X, Block.StandardDim.Y)));
                    break;
                case PieceType.L:
                    blocks.Add(new Block(pivot));
                    blocks.Add(new Block(pivot + new Point(0.0f, -Block.StandardDim.Y)));
                    blocks.Add(new Block(pivot + new Point(0.0f, Block.StandardDim.Y)));
                    blocks.Add(new Block(pivot + new Point(Block.StandardDim.X, Block.StandardDim.Y)));
                    break;
                case PieceType.S:
                    blocks.Add(new Block(pivot));
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X, 0.0f)));
                    blocks.Add(new Block(pivot + new Point(-Block.StandardDim.X, -Block.StandardDim.Y)));
                    blocks.Add(new Block(pivot + new Point(0.0f, Block.StandardDim.Y)));
                    break;
                case PieceType.Z:
                    blocks.Add(new Block(pivot));
                    blocks.Add(new Block(pivot + new Point(Block.StandardDim.X, 0.0f)));
                    blocks.Add(new Block(pivot + new Point(Block.StandardDim.X, -Block.StandardDim.Y)));
                    blocks.Add(new Block(pivot + new Point(0.0f, Block.StandardDim.Y)));
                    break;
                default:
                    Console.Error.WriteLine("ERROR: Invalid Piece Type");
                    break;
            }
        }

        private void move_blocks(Direction direction)
        {
            Point velocity = new Point();
            switch (direction)
            {
                case Direction.Up:
                    velocity.y -= Block.StandardDim.Y;
                    MoveBlocksBy(velocity);
                    break;
                case Direction.Down:
                    velocity.y += Block.StandardDim.Y;
                    MoveBlocksBy(velocity);
                    break;
                case Direction.Left:
                    velocity.x -= Block.StandardDim.X;
                    MoveBlocksBy(velocity);
                    break;
                case Direction.Right:
                    velocity.x += Block.StandardDim.X;
                    MoveBlocksBy(velocity);
                    break;
                default:
                    Console.Error.WriteLine("ERROR: Invalid direction");
                    break;
            }
            this.velocity = velocity;
        }

        /*  Public Methods --------------------------------------------------------------------*/
        public static PieceType RandType()
        {
            PieceType newType = PieceType.O;
            int randomType = rand.Next(NUM_TYPES);

            switch (randomType)
            {
                case 0: // I
                    newType = PieceType.I;
                    break;
                case 1: // O
                    break;
                case 2: // T
                    newType = PieceType.T;
                    break;
                case 3: // J
                    newType = PieceType.J;
                    break;
                case 4: // L
                    newType = PieceType.L;
                    break;
                case 5: // S
                    newType = PieceType.S;
                    break;
                case 6: // Z
                    newType = PieceType.Z;
                    break;
                default:
                    Console.Error.WriteLine("ERROR: Invalid Tetromino Type (RandType)");
                    break;
            }
            return newType;
        }

        /// <summary>
        /// Creates a new instance of a tetromino with the same initial position.
        /// </summary>
        /// <returns> A new instances of Tetromino at intialPos. </returns>
        public Tetromino CreatePiece()
        {
            return new Tetromino(initialPos);
        }

        public void Move(Direction direction)
        {
            move_blocks(direction);
            // Broadcasts change to every subscriber
            UpdatePosition?.Invoke(MoveType.Translation);
        }

        public void Move(Direction direction, MoveType moveType = MoveType.Translation)
        {
            move_blocks(direction);
            UpdatePosition?.Invoke(moveType);
        }

        public void Rotate(Direction direction)
        {
            double angle = 0.0;
            
            switch (direction)
	        {
	            case Direction.Left:
	            	angle = -Math.PI / 2.0; // Rotate 90 degrees to the left
	            	break;
	            case Direction.Right:
	            	angle = Math.PI / 2.0; // Rotate 90 degrees to the right
	            	break; 
	            default:
	            	Console.Error.WriteLine("ERROR: Invalid rotation direction.");
	            	break;
	        }
            
            foreach(IBlock block in Blocks){
                RotateAboutPivot(block, angle);
            }

            rotation_direction = direction;
            this.angle = angle;
            UpdatePosition?.Invoke(MoveType.Rotation);
            
        }

        public void Drop(ITracker<PieceType, Direction, MoveType> tracker)
        {
            while (!(tracker as Tracker).isCollision(MoveType.Translation))
            {
                move_blocks(Direction.Down);
            }
        }

        public void UndoMove(MoveType moveType)
        {
            switch (moveType)
            {
                case MoveType.Translation:
                    velocity = -1 * velocity;
                    MoveBlocksBy(velocity);
                    UpdatePosition?.Invoke(MoveType.Undo);
                    break;
                case MoveType.Rotation:
                    foreach (IBlock block in Blocks)
                    {
                        RotateAboutPivot(block, -angle);
                    }
                    UpdatePosition?.Invoke(MoveType.Undo);
                    break;
            }
        }

        public void NewPiece(){
            pivot = this.initialPos;

            /* Picks a random type for the tetromino piece*/
            rand = new Random(DateTime.Now.GetHashCode()); // Creates a random object with a random seed
            type = RandType();
            
            /* Creates a list of blocks and positions them according to the type */
            Blocks = new List<IBlock>(NUM_BLOCKS);
            ConstructTetromino(Blocks, type, ref pivot);
            
            UpdatePosition?.Invoke(MoveType.Spawn);
        }

        public Tetromino Copy()
        {
            Tetromino copy_piece = new Tetromino(Position);

            copy_piece.type = type;
            copy_piece.pivot = pivot.Copy();
            copy_piece.Blocks = new List<IBlock>();
            foreach (Block block in Blocks)
            {
                copy_piece.Blocks.Add(block.Copy());
            }
            copy_piece.velocity = velocity.Copy();
            copy_piece.rotation_direction = rotation_direction;
            copy_piece.angle = angle;

            return copy_piece;
        }
        
    }
}