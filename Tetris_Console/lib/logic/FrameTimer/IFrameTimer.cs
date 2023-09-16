using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public interface IFrameTimer
    {
        /// <summary>
        /// The remaining frames left in the counter before the TimerFinished action is invoked.
        /// </summary>
        public int FramesRemaining { get; }

        /// <summary>
        /// Action that emits once the timer has reached 0 frames left.
        /// </summary>
        public event Action TimerFinished;

        /// <summary>
        /// Decrements the remaining frames per frame. Must be subscribed to the frame tracker.
        /// </summary>
        public void CountDown();

        /// <summary>
        /// Reset the frame count to the intial frame count before the timer began.
        /// </summary>
        public void Reset();

        /// <summary>
        /// Start the timer from the current number of remaining frames.
        /// </summary>
        public void Start();

        /// <summary>
        /// Stop the timer.
        /// </summary>
        public void Stop();

        /// <summary>
        /// Set a new frame number to count down from.
        /// </summary>
        /// <param name="numFrames">Number of frames to count down from.</param>
        public void SetTimer(int numFrames);
    }
}
