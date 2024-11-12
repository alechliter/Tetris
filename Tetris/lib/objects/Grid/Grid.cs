using Lechliter.Tetris.Lib.Types;

namespace Lechliter.Tetris.Lib.Objects
{
    public class Grid : IGrid<IntDimensions, Point>
    {
        public IntDimensions GridDim { get; protected set; }

        public IntDimensions BoundsDim { get; protected set; }

        private static readonly IntDimensions DefaultBounds;

        private static readonly IntDimensions DefaultGrid;

        static Grid()
        {
            const int WIDTH = 12, HEIGHT = 22;
            DefaultBounds = new IntDimensions(WIDTH, HEIGHT);
            DefaultGrid = new IntDimensions(WIDTH - 2, HEIGHT - 2);
        }

        public Grid()
        {
            GridDim = DefaultGrid;
            BoundsDim = DefaultBounds;
        }

        public void GridPosition(Point point, out int X, out int Y)
        {
            X = (int)Math.Ceiling(point.x) % BoundsDim.X;
            Y = (int)Math.Ceiling(point.y) % BoundsDim.Y;
        }
    }
}
