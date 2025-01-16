namespace Lechliter.Tetris.Lib.Systems
{
    public interface IInputHandler<TKey> : IReadOnlyInputHandler
    {
        void AddKey(TKey key, Action action);
    }
}
