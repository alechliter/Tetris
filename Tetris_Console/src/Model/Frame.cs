using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public class Frame : IFrame
    {
        private long past;
        private long now;
        private long ticks_diff;
        private long interval_ms;

        private const long DEFAULT_INTERVAL = 1000;

        public event Action FrameAction;

        public Frame()
        {
            past = DateTime.Now.Ticks;
            now = past;
            ticks_diff = now - past;
            interval_ms = DEFAULT_INTERVAL;
        }

        public bool nextFrame()
        {
            now = DateTime.Now.Ticks;
            ticks_diff = now - past;
            
            bool isNewFrame = ticks_diff >= interval_ms * TimeSpan.TicksPerMillisecond;

            if (isNewFrame)
            {
                FrameAction?.Invoke();
                past = now;
            }
            return isNewFrame;
        }

        public void SpeedUp(int ms)
        {
            interval_ms = DEFAULT_INTERVAL - ms * 100;
        }
    }
}
