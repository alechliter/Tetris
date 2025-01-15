using Lechliter.Tetris.Lib.Exceptions;
using Ninject.Modules;
using TetrisIoC.lib.Kernels;

namespace Tetris.lib.Tetris
{
    public class TetrisApp
    {
        public static IReadOnlyKernel IoC
        {
            get
            {
                if (Kernel == null)
                {
                    throw new TetrisLibException("IoC accessed before initialized.");
                }
                return Kernel;
            }
        }

        private static ITetrisKernel? Kernel { get; set; }

        public TetrisApp(ITetrisKernel kernel)
        {
            Kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
        }

        public void Load(IEnumerable<INinjectModule> modules)
        {
            Kernel?.Load(modules);
        }

        public void Load(params INinjectModule[] modules)
        {
            Kernel?.Load(modules);
        }
    }
}
