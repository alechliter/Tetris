namespace Lechliter.Tetris.Lib.Systems
{
    public interface IReadOnlyInputHandler
    {
        event Action AnyKeyEvent;

        void HandleInput();
    }
}
