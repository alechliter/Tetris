using Ninject;
using Ninject.Modules;

namespace TetrisIoC.lib.Kernels
{
    public class TetrisStandardKernel : TetrisBaseKernel
    {
        public TetrisStandardKernel(params INinjectModule[] modules) : base(new StandardKernel(modules))
        {
        }
    }
}
