namespace Lechliter.Tetris.Lib.Types
{
    public struct IntPoint
    {
        public int X, Y;
        public IntPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public IntPoint(IntPoint point)
        {
            X = point.X;
            Y = point.Y;
        }

        public static IntPoint operator +(IntPoint p1, IntPoint p2)
        {
            return new IntPoint(p1.X + p2.X, p1.Y + p2.Y);
        }
        public static IntPoint operator -(IntPoint p1, IntPoint p2)
        {
            return new IntPoint(p1.X - p2.X, p1.Y - p2.Y);
        }
    }

}
