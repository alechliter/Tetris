namespace Lechliter.Tetris.Lib.Objects
{
    public class Frame : IFrame
    {

        public event Action? FrameAction;

        private long past;

        private long now;

        private long ticks_diff;

        private long interval_ms;

        private long initial_interval_ms;

        private const long DEFAULT_INTERVAL = 1000;

        public Frame(long interval = DEFAULT_INTERVAL)
        {
            past = DateTime.Now.Ticks;
            now = past;
            ticks_diff = now - past;
            interval_ms = interval;
            initial_interval_ms = interval_ms;
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
            interval_ms = initial_interval_ms - ms * 100;
        }
    }
}
