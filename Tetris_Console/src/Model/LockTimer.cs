using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public class LockTimer : IFrameTimer
    {
        /* Private Members */

        private int remaining_frames;

        private int initial_frame_count;

        private bool isCounting;

        private const int DEFAULT_FRAME_COUNT = 5;

        /* Public Members*/
        public int FramesRemaining { get { return remaining_frames; } }

        public bool IsRunning { get { return isCounting; } }

        public event Action TimerFinished;

        /* Constructors */
        public LockTimer(int frame_count = DEFAULT_FRAME_COUNT)
        {
            Initialize(frame_count);
        }

        /* Private Methods */
        private void Initialize(int frame_count)
        {
            SetTimer(frame_count);
            Reset();
            isCounting = false;
        }

        /* Public Methods */

        public void Start()
        {
            isCounting = true;
        }

        public void Stop()
        {
            isCounting = false;
        }

        public void CountDown()
        {
            if (isCounting)
            {
                remaining_frames--;
                if (remaining_frames <= 0)
                {
                    Stop();
                    Reset();
                    TimerFinished?.Invoke();
                }
            }
        }

        public void Reset()
        {
            remaining_frames = initial_frame_count;
        }

        public void SetTimer(int numFrames)
        {
            initial_frame_count = numFrames;
        }
    }
}
