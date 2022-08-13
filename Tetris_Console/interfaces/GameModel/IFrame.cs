using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public interface IFrame
    {
        /// <summary>
        /// Action event to invoke for each frame.
        /// </summary>
        public event Action FrameAction;

        /// <summary>
        /// Determines if the next frame is reached. Invokes [frameAction] for each frame.
        /// </summary>
        bool nextFrame();

        /// <summary>
        /// Decreases the time between frames;
        /// </summary>
        void SpeedUp(int ms = 150);
    }
}
