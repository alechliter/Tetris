namespace Lechliter.Tetris.Lib.Types
{
    public struct IntDimensions
    {
        public int X { get; private set; }

        public int Y { get; private set; }

        public IntDimensions(int width, int height)
        {
            this.X = width;
            this.Y = height;
        }
    }
}
