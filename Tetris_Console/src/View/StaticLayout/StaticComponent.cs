namespace Lechliter.Tetris_Console
{
    public struct StaticComponent
    {
        public ePieceType[,] Elements;
        public IntPoint Position;
        public IntDimensions Size;

        public StaticComponent(ePieceType[,] elements, IntPoint position)
        {
            Elements = elements;
            Position = position;
            Size = new IntDimensions(elements.GetLength(0), elements.GetLength(1));
        }
    }
}
