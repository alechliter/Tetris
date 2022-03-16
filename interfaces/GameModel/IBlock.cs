namespace Lechliter.Tetris_Console
{   
    struct Dimensions
    {
        public float X, Y;
        public Dimensions(float width, float height)
        {
            this.X = width;
            this.Y = height;
        }
    }
    interface IBlock : IGameObject
    {
        Dimensions Dim { get; }

        void MoveTo(Point newPoint);
        void MoveBy(Point vector);
    }
}
