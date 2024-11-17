using Lechliter.Tetris.Lib.Types;
using Tetris.Lib.Definitions.Types;

namespace Lechliter.Tetris.Lib.Objects
{
    public class Block : IBlock
    {
        public FloatDimensions Dim { get; private set; }

        public Point Position { get; private set; }

        public static readonly FloatDimensions StandardDim = new FloatDimensions(1.0f, 1.0f);

        public Block() : this(new Point())
        {
        }

        public Block(Point initialPos)
        {
            Position = initialPos;
            Dim = StandardDim;
        }

        public void MoveTo(Point newPoint)
        {
            Position = newPoint;
        }

        public void MoveBy(Point vector)
        {
            Position += vector;
        }

        public IBlock Copy()
        {
            return new Block(Position.Copy());
        }

        public bool IsSamePosition(IBlock block)
        {
            return block.Position.x == this.Position.x && block.Position.y == this.Position.y;
        }
    }
}
