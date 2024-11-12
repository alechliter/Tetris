namespace Lechliter.Tetris.TetrisConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            ITetrisConsoleController tetrisConsoleController = new TetrisConsoleController();
            tetrisConsoleController.Run(args);
        }
    }
}
