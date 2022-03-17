namespace Lechliter.Tetris_Console
{
    struct Point
    {
        public float x, y;
        public Point(float x, float y)
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

    }
    interface IGameObject
    {
        public Point Position { get; }
    }
}
