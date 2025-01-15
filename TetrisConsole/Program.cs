using Lechliter.Tetris.TetrisConsole.Modules;
using Tetris.lib.Modules;
using Tetris.lib.Tetris;
using TetrisIoC.lib.Kernels;

namespace Lechliter.Tetris.TetrisConsole
{
    public class Program
    {
        static void Main(string[] args)
        {
            TetrisApp app = new TetrisApp(new TetrisStandardKernel());
            app.Load(new TetrisStandardModule(), new TetrisConsoleModule());

            ITetrisConsoleController controller = TetrisApp.IoC.Get<ITetrisConsoleController>();
            controller.Run(args);
        }
    }
}
