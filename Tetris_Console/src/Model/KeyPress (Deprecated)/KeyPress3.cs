using System;
using System.Collections.Generic;
using System.Threading;

namespace Lechliter.Tetris_Console
{
    /// <summary>
    /// KeyState Source: https://stackoverflow.com/questions/4351258/c-sharp-arrow-key-input-for-a-console-app
    /// </summary>
    public class KeyState
    {
        /// <summary>
        /// Codes representing keyboard keys.
        /// </summary>
        /// <remarks>
        /// Key code documentation:
        /// http://msdn.microsoft.com/en-us/library/dd375731%28v=VS.85%29.aspx
        /// </remarks>
        internal enum KeyCode : int
        {
            /// <summary>
            /// The left arrow key.
            /// </summary>
            Left = 0x25,

            /// <summary>
            /// The up arrow key.
            /// </summary>
            Up,

            /// <summary>
            /// The right arrow key.
            /// </summary>
            Right,

            /// <summary>
            /// The down arrow key.
            /// </summary>
            Down,

            A = 0x41,
            B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z

        }

        /// <summary>
        /// Provides keyboard access.
        /// </summary>
        internal static class NativeKeyboard
        {
            /// <summary>
            /// A positional bit flag indicating the part of a key state denoting
            /// key pressed.
            /// </summary>
            private const int KeyPressed = 0x8000;

            /// <summary>
            /// Returns a value indicating if a given key is pressed.
            /// </summary>
            /// <param name="key">The key to check.</param>
            /// <returns>
            /// <c>true</c> if the key is pressed, otherwise <c>false</c>.
            /// </returns>
            public static bool IsKeyDown(KeyCode key)
            {
                return (GetKeyState((int)key) & KeyPressed) != 0;
            }

            /// <summary>
            /// Gets the key state of a key.
            /// </summary>
            /// <param name="key">Virtuak-key code for key.</param>
            /// <returns>The state of the key.</returns>
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            private static extern short GetKeyState(int key);
        }
    }
    public class KeyPress3 
    {
        public event Action moveLeft, moveRight, moveDown;

        public event Action rotateLeft, rotateRight;

        public event Action drop;

        public event Action save;

        //private Timer timer;

        private string input = "";

        private HashSet<ConsoleKey> keys_pressed;

        private const int WAIT_TIME = 1;

        public KeyPress3() { }

        public void Frame(Action frame_action, Action parse_action){
            input = "";
            keys_pressed = new HashSet<ConsoleKey>();

            Thread thread = new Thread(new ThreadStart(frame_action));
            thread.Start();
            Thread.Sleep(WAIT_TIME);

            parse_action();
        }

        public void ReadInput(){
            int new_input = Console.Read();
            while (true)
            {
                if (new_input != -1)
                {
                    input = $"{input}{(char)new_input}";
                    System.Diagnostics.Debug.WriteLine(input);
                }
                new_input = Console.Read();
            }
        }

        public void ReadKey()
        {
            while (true)
            {
                //keys_pressed.Add(Console.ReadKey(true).Key);
                if (KeyState.NativeKeyboard.IsKeyDown(KeyState.KeyCode.Up))
                {
                    Console.WriteLine("Up Pressed");
                }
            }
        }

        public void ParseInput()
        {
            input = input.ToLower();
            char[] characters = input.ToCharArray();
            HashSet<char> char_set = new HashSet<char>(characters);

            System.Diagnostics.Debug.WriteLine($"Parsed input: { string.Join(", ", char_set)}");

            foreach (char character in char_set)
            {
                switch (character)
                {
                    case 'c':
                        break;
                }
            }
        }

        public void ParseKeysPressed()
        {
            foreach (ConsoleKey key in keys_pressed)
            {
                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        drop.Invoke();
                        break;
                    case ConsoleKey.DownArrow:
                        moveDown.Invoke();
                        break;
                    case ConsoleKey.LeftArrow:
                        moveLeft.Invoke();
                        break;
                    case ConsoleKey.RightArrow:
                        moveRight.Invoke();
                        break;
                    case ConsoleKey.C:
                        rotateLeft.Invoke();
                        break;
                    case ConsoleKey.V:
                        rotateRight.Invoke();
                        break;
                    case ConsoleKey.X:
                        save.Invoke();
                        break;
                }
            }
        }
    }
}