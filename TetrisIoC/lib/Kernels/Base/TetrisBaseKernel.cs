using Ninject;
using Ninject.Modules;
using Ninject.Syntax;

namespace TetrisIoC.lib.Kernels
{
    public abstract class TetrisBaseKernel : ITetrisKernel
    {
        protected readonly IKernel Kernel;

        protected TetrisBaseKernel(IKernel kernel)
        {
            Kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
        }

        public void Load(IEnumerable<INinjectModule> modules)
        {
            Kernel.Load(modules);
        }

        public TInterface Get<TInterface>()
        {
            return Kernel.Get<TInterface>();
        }

        public TInterface? TryGet<TInterface>()
        {
            return Kernel.TryGet<TInterface>();
        }

        public IEnumerable<TInterface> GetAll<TInterface>()
        {
            return Kernel.GetAll<TInterface>();
        }

        public IBindingToSyntax<T> Bind<T>()
        {
            return Kernel.Bind<T>();
        }

        public IBindingToSyntax<T1, T2> Bind<T1, T2>()
        {
            return Kernel.Bind<T1, T2>();
        }

        public IBindingToSyntax<T1, T2, T3> Bind<T1, T2, T3>()
        {
            return Kernel.Bind<T1, T2, T3>();
        }

        public IBindingToSyntax<T1, T2, T3, T4> Bind<T1, T2, T3, T4>()
        {
            return Kernel.Bind<T1, T2, T3, T4>();
        }

        public IBindingToSyntax<T1> Rebind<T1>()
        {
            return Kernel.Rebind<T1>();
        }

        public IBindingToSyntax<T1, T2> Rebind<T1, T2>()
        {
            return Kernel.Rebind<T1, T2>();
        }

        public IBindingToSyntax<T1, T2, T3> Rebind<T1, T2, T3>()
        {
            return Kernel.Rebind<T1, T2, T3>();
        }

        public IBindingToSyntax<T1, T2, T3, T4> Rebind<T1, T2, T3, T4>()
        {
            return Kernel.Rebind<T1, T2, T3, T4>();
        }

        public void Unbind<T>()
        {
            Kernel.Unbind<T>();
        }
    }
}
