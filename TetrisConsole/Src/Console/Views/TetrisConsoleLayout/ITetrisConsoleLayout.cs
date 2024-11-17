using System.Collections.Generic;

namespace Lechliter.Tetris.TetrisConsole
{
    public interface ITetrisConsoleLayout<TComponent, TComponentCollection, TPoint> : IDynamicLayout<TComponent, TComponentCollection, TPoint> where TComponentCollection : ICollection<TComponent>
    {
        public void AddComponents(bool isDev);
    }
}
