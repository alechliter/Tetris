using Lechliter.Tetris.Lib.Types;

namespace Lechliter.Tetris.Lib.Objects
{
    public class Block : IBlock
    {
        public Dimensions Dim { get { return dim; } protected set { dim = value; } }

        public Point Position { get { return origin; } }

        public static readonly Dimensions StandardDim = new Dimensions(1.0f, 1.0f);

        private Point origin;

        private Dimensions dim;

        public Block()
        {
            origin.x = 0.0f;
            origin.y = 0.0f;
            this.dim = StandardDim;
        }

        public Block(Point initialPos)
        {
            origin = initialPos;
            this.dim = StandardDim;
        }

        public void MoveTo(Point newPoint)
        {
            origin = newPoint;
        }
        public void MoveBy(Point vector)
        {
            origin += vector;
        }

        public IGameObject Copy()
        {
            return new Block(origin.Copy());
        }

        public bool IsSamePosition(IBlock block)
        {
            return block.Position.x == this.Position.x && block.Position.y == this.Position.y;
        }
    }
}
