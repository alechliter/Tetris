namespace TetrisIoC.lib.Kernels
{
    public interface IReadOnlyKernel
    {
        TInterface Get<TInterface>();

        TInterface? TryGet<TInterface>();

        IEnumerable<TInterface> GetAll<TInterface>();
    }
}
