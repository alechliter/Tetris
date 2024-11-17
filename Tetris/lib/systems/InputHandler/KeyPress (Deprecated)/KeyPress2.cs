using System;
using System.Collections.Generic;

namespace Lechliter.Tetris.Lib.Systems
{

    /// <summary>
    /// A class that provides support for key press and release events in a console application.
    /// </summary>
    public class KeyPress2
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