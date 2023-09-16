using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public class Block : IBlock
    {
        /* Private members */
        private Point origin;
        private Dimensions dim;

        /* Public members */
        public Dimensions Dim { get { return dim; } protected set { dim = value; }}
        public Point Position { get { return origin; } }
        public static readonly Dimensions StandardDim = new Dimensions(1.0f, 1.0f);

        /* Constructor */
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

        /* Public methods */
        public void MoveTo(Point newPoint)
        {
            origin = newPoint;
        }
        public void MoveBy(Point vector)
        {
            origin += vector;
        }

        public Block Copy()
        {
            return new Block(origin.Copy());
        }

        public bool IsSamePosition(Block block){
            return block.Position.x == this.Position.x && block.Position.y == this.Position.y;
        }
    }
}
