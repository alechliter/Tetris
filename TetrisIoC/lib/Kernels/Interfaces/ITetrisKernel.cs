using Ninject.Modules;
using Ninject.Syntax;

namespace TetrisIoC.lib.Kernels
{
    public interface ITetrisKernel : IReadOnlyKernel
    {
        void Load(IEnumerable<INinjectModule> modules);

        public IBindingToSyntax<T> Bind<T>();
        public IBindingToSyntax<T1, T2> Bind<T1, T2>();
        public IBindingToSyntax<T1, T2, T3> Bind<T1, T2, T3>();
        public IBindingToSyntax<T1, T2, T3, T4> Bind<T1, T2, T3, T4>();
        public void Unbind<T>();
        public IBindingToSyntax<T1> Rebind<T1>();
        public IBindingToSyntax<T1, T2> Rebind<T1, T2>();
        public IBindingToSyntax<T1, T2, T3> Rebind<T1, T2, T3>();
        public IBindingToSyntax<T1, T2, T3, T4> Rebind<T1, T2, T3, T4>();
    }
}
