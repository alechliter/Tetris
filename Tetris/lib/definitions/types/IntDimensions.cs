namespace Lechliter.Tetris.Lib.Types
{
    public struct IntDimensions
    {
        public int X { get; private set; }

        public int Y { get; private set; }

        public IntDimensions(int width = 0, int height = 0)
        {
            X = width;
            Y = height;
        }
    }
}
