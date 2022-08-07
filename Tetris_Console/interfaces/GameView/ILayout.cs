
namespace Lechliter.Tetris_Console
{
    public interface ILayout<TPosition, TGrid, TComponent>
    {
        public TPosition Origin { get; }

        public TGrid Grid { get; }

        public void AddToGrid(TComponent component, TPosition position);
    }
}
