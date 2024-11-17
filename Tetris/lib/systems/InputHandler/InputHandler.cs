using Lechliter.Tetris.Lib.Objects;

namespace Lechliter.Tetris.Lib.Systems
{
    public class InputHandler : IInputHandler<ConsoleKey, Action>
    {
        public IDictionary<ConsoleKey, Action> KeyEvent { get; }

        public Action? AnyKeyEvent { get; set; }

        private readonly Queue<ConsoleKeyInfo> PressedKeys;

        private readonly IFrame Timer;

        private static readonly ConsoleKey[] DEFAULT_KEYS = {
            ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.LeftArrow, ConsoleKey.RightArrow,
            ConsoleKey.C, ConsoleKey.V,
            ConsoleKey.Q, ConsoleKey.N
        };

        private static readonly int QUEUE_LIMIT = 10;

        public InputHandler()
        {
            KeyEvent = new Dictionary<ConsoleKey, Action>();
            PressedKeys = new Queue<ConsoleKeyInfo>();
            Timer = new Frame(interval: 10);
            Timer.FrameAction += InvokeNextWaitingKey;

            InitializeKeyEvents();
        }

        public void HandleInput()
        {
            ConsoleKeyInfo key;
            if (Console.KeyAvailable)
            {
                key = Console.ReadKey(true);
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
            if (!KeyEvent.ContainsKey(key))
            {
                KeyEvent.Add(key, action);
            }
        }

        private void InitializeKeyEvents()
        {
            KeyEvent.Clear();
            foreach (ConsoleKey key in DEFAULT_KEYS)
            {
                AddKey(key, () => { });
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
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }
    }
}
