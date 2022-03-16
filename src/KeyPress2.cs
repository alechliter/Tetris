using System;
using System.Collections.Generic;

namespace Lechliter.Tetris_Console
{

    /// <summary>
    /// A class that provides support for key press and release events in a console application.
    /// </summary>
    class KeyPress2
    {
        /* Private Members */

        /* Public Members */
        public static event Action PressedEvent;
        public static event Action ReleasedEvent;

        /* Constructor */
        static KeyPress2()
        { }

        /* Private Methods ------------------------------------------------------------------*/
        
        /* Public Methods ------------------------------------------------------------------ */

        public static void HandleAllEvents(){
            if(Console.KeyAvailable){
                PressedEvent?.Invoke();
            }else{
                ReleasedEvent?.Invoke();
            }
        }
    }

}