using System.Collections.Generic;

namespace Lechliter.Tetris_Console.src.tetris_console.views
{
    public interface ITetrisConsoleLayout<TComponent, TComponentCollection, TPoint> : IDynamicLayout<TComponent, TComponentCollection, TPoint> where TComponentCollection : ICollection<TComponent>
    {
        public void AddComponents(bool isDev);
    }
}
