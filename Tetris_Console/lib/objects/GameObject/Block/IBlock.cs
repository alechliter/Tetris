namespace Lechliter.Tetris_Console
{
    public struct Dimensions
    {
        public float X, Y;
        public Dimensions(float width, float height)
        {
            this.X = width;
            this.Y = height;
        }
    }
    public interface IBlock : IGameObject
    {
        Dimensions Dim { get; }

        void MoveTo(Point newPoint);
        void MoveBy(Point vector);

        public bool IsSamePosition(IBlock block);
    }
}
