using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public class InputHandler : IInputHandler<ConsoleKey, Action>
    {
        public IDictionary<ConsoleKey, Action> KeyEvent { get; }

        private static readonly ConsoleKey[] DEFAULT_KEYS = { 
            ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow,
            ConsoleKey.C, ConsoleKey.V,
            ConsoleKey.Q, ConsoleKey.N
        };

        public InputHandler()
        {
            KeyEvent = new Dictionary<ConsoleKey, Action>();

            foreach(ConsoleKey key in DEFAULT_KEYS)
            {
                AddKey(key, () => {});
            }
        }

        public void HandleInput()
        {
            ConsoleKeyInfo key;
            if (Console.KeyAvailable)
            {
                key = Console.ReadKey(true);
                Action keyEvent = null;
                if (KeyEvent.TryGetValue(key.Key, out keyEvent))
                {
                    keyEvent.Invoke();
                }
            }
        }

        public void AddKey(ConsoleKey key, Action action)
        {
            if (!KeyEvent.ContainsKey(key))
            {
                KeyEvent.Add(key, action);
            }
        }
    }
}
