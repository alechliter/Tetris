using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Effects;
using Lechliter.Tetris.Lib.Systems;
using Lechliter.Tetris.Lib.Types;
using Lechliter.Tetris.TetrisConsole.Enumerations;
using System;
using System.Collections.Generic;
using TetrisIoC.lib.Modules;

namespace Lechliter.Tetris.TetrisConsole.Modules
{
    internal class TetrisConsoleModule : TetrisBaseModule
    {
        public override void Load()
        {
            Bind<ITetrisConsoleController>().To<TetrisConsoleController>().InSingletonScope();
            Bind<IView<eTextColor, ePieceType>>().To<ConsoleView>().InSingletonScope();
            Bind<ISoundEffect>().To<SimpleSoundEffect>().InSingletonScope();
            Bind<IInputHandler<ConsoleKey>, IReadOnlyInputHandler>().To<ConsoleInputHandler>().InSingletonScope();
            Bind<ITetrisConsoleLayout<DynamicComponent, List<DynamicComponent>, IntPoint>>().To<TetrisConsoleLayout>().InSingletonScope();
        }
    }
}
