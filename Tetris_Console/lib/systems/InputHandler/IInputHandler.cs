using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public interface IInputHandler<TKey, TEvent>
    {
        public IDictionary<TKey, TEvent> KeyEvent { get; }

        public TEvent AnyKeyEvent { get; set; }

        void HandleInput();

        public void AddKey(ConsoleKey key, Action action);
    }
}
