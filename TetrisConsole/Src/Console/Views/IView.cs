namespace Lechliter.Tetris.TetrisConsole
{
    public interface IView<TColor, TPieceType> where TColor : struct, System.Enum
                                                     where TPieceType : System.Enum

    {
        static TColor Color { get; }
        void Display();

    }
}
