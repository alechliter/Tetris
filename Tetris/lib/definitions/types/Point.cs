namespace Lechliter.Tetris.Lib.Types
{
    public struct Point
    {
        public float x, y;
        public Point(float x = 0, float y = 0)
        {
            this.x = x;
            this.y = y;
        }

        public static Point operator +(Point p1, Point p2)
        {
            return new Point(p1.x + p2.x, p1.y + p2.y);
        }
        public static Point operator -(Point p1, Point p2)
        {
            return new Point(p1.x - p2.x, p1.y - p2.y);
        }
        public static Point operator *(Point p1, Point p2)
        {
            return new Point(p1.x * p2.x, p1.y * p2.y);
        }
        public static Point operator *(int c, Point p1)
        {
            return new Point(p1.x * c, p1.y * c);
        }

        public Point Copy()
        {
            return new Point(x, y);
        }

    }
}
