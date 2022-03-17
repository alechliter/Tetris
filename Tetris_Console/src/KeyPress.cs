using System;
using System.Collections.Generic;

/*
    Replace with a better design. This method is too slow.
*/

namespace Lechliter.Tetris_Console
{
    /// <summary>
    /// Key-Event structure that saves the events and properties of a given keyboard key.
    /// </summary>
    class KeyEvent
    {
        public Action DownEvent;
        public Action UpEvent;
        public ConsoleKey Key {get; set;}
        public bool IsPressed {get; set;} = false;

        public KeyEvent(ConsoleKey key)
        {
            Key = key;
        }
    }

    /// <summary>
    /// A class that provides support for key press and release events in a console application.
    /// </summary>
    class KeyPress
    {
        /* Private Members */
        private static IDictionary<ConsoleKey, KeyEvent> KeyEvents;

        /* Public Members */
        public static ICollection<ConsoleKey> SupportedKeys;

        // Constructor
        static KeyPress()
        {
            Initialize();
        }

        /* Private Methods ------------------------------------------------------------------*/

        private static void Initialize()
        {
            KeyEvents = new Dictionary<ConsoleKey, KeyEvent>();
            SupportedKeys = new List<ConsoleKey>();
        }

        private static bool ChangeEvent(ConsoleKey key, Action newEvent, Action<KeyEvent, Action> add_to_event){
            KeyEvent keyEvent;
            bool keySupported = KeyEvents.TryGetValue(key, out keyEvent);            

            if(keySupported){
                add_to_event?.Invoke(keyEvent, newEvent);
            }

            return keySupported;
        }
        
        /* Public Methods ------------------------------------------------------------------ */
        public static void HandleAllEvents(){
            foreach (KeyValuePair<ConsoleKey, KeyEvent> pair in KeyEvents){
                if(Console.KeyAvailable){
                    ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                    if(!pair.Value.IsPressed && pair.Key == pressedKey.Key){
                        pair.Value.IsPressed = true;
                        pair.Value.DownEvent?.Invoke();                        
                    }else if(pair.Value.IsPressed && pair.Key != pressedKey.Key){
                        pair.Value.IsPressed = false;
                        pair.Value.UpEvent?.Invoke();
                    }
                }else if(pair.Value.IsPressed){
                    pair.Value.IsPressed = false;
                    pair.Value.UpEvent?.Invoke();
                }
            }
        }
        public static bool AddKey(ConsoleKey key)
        {
            bool success = !SupportedKeys.Contains(key);

            if(success){
                KeyEvent key_event = new KeyEvent(key);
                KeyEvents.Add(key, key_event);

                SupportedKeys.Add(key);
            }

            // returns false if the key is already supported
            return success;
        }
        public static bool AddPressedEvent(ConsoleKey key, Action newEvent)
        {
            return ChangeEvent(key, newEvent, (x, y) => x.UpEvent += y);
        }

        public static bool AddReleasedEvent(ConsoleKey key, Action newEvent)
        {
            return ChangeEvent(key, newEvent, (x, y) => x.DownEvent += y);
        }

        public static bool RemoveReleasedEvent(ConsoleKey key, Action newEvent)
        {
            return ChangeEvent(key, newEvent, (x, y) => x.UpEvent -= y);
        }

        public static bool RemovePressedEvent(ConsoleKey key, Action newEvent)
        {
            return ChangeEvent(key, newEvent, (x, y) => x.DownEvent -= y);
        }

    }

}