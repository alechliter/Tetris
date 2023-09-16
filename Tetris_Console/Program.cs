using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lechliter.Tetris_Console
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
