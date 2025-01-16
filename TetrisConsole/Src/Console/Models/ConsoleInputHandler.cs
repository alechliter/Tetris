using Lechliter.Tetris.Lib.Objects;
using Lechliter.Tetris.Lib.Systems;
using System;
using System.Collections.Generic;

namespace Lechliter.Tetris.TetrisConsole
{
    public class ConsoleInputHandler : IInputHandler<ConsoleKey>
    {
        public IDictionary<ConsoleKey, Action> KeyEvent { get; }

        public event Action? AnyKeyEvent;

        private readonly Queue<ConsoleKeyInfo> PressedKeys;

        private readonly IFrame Timer;

        private static readonly int QUEUE_LIMIT = 10;

        public ConsoleInputHandler()
        {
            KeyEvent = new Dictionary<ConsoleKey, Action>();
            PressedKeys = new Queue<ConsoleKeyInfo>();
            Timer = new Frame(interval: 10);
            Timer.FrameAction += InvokeNextWaitingKey;
        }

        public void HandleInput()
        {
            ConsoleKeyInfo key;
            if (System.Console.KeyAvailable)
            {
                key = System.Console.ReadKey(true);
                if (Timer.NextFrame())
                {
                    PressedKeys.Clear();
                    InvokeKeyAction(key);
                }
                else
                {
                    PressedKeys.Enqueue(key);
                    if (PressedKeys.Count > QUEUE_LIMIT)
                    {
                        PressedKeys.Dequeue();
                    }
                }
            }
        }

        public void AddKey(ConsoleKey key, Action action)
        {
            if (KeyEvent.ContainsKey(key))
            {
                KeyEvent[key] += action;
            }
            else
            {
                KeyEvent.Add(key, action);
            }
        }

        private void InvokeKeyAction(ConsoleKeyInfo key)
        {
            if (KeyEvent.TryGetValue(key.Key, out Action? keyEvent))
            {
                keyEvent?.Invoke();
                AnyKeyEvent?.Invoke();
            }
        }

        private void InvokeNextWaitingKey()
        {
            if (PressedKeys.Count > 0)
            {
                ConsoleKeyInfo key = PressedKeys.Dequeue();
                InvokeKeyAction(key);
            }
            while (System.Console.KeyAvailable)
            {
                System.Console.ReadKey(true);
            }
        }
    }
}
