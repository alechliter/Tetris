using Ninject;
using Tetris.lib.Design.Helpers;

namespace Lechliter.Tetris.Lib.Objects
{
    public class Frame : IFrame
    {

        public event Action? FrameAction;

        public event Action<long, long>? SpeedChange;

        private long Past;

        private long Now;

        private long TicksDiff;

        private long IntervalMS;

        private long InitialIntervalMS;

        private static int DefaultInterval
        {
            get
            {
                return ConfigurationHelper.GetInt("FrameIntervalDefault", DEFAULT_INTERVAL);
            }
        }

        [Inject]
        public Frame() : this(DefaultInterval)
        {

        }

        public Frame(long? interval = null)
        {
            Past = DateTime.Now.Ticks;
            Now = Past;
            TicksDiff = Now - Past;
            IntervalMS = interval.HasValue ? interval.Value : DefaultInterval;
            InitialIntervalMS = IntervalMS;
        }

        public bool NextFrame()
        {
            Now = DateTime.Now.Ticks;
            TicksDiff = Now - Past;

            bool isNewFrame = TicksDiff >= IntervalMS * TimeSpan.TicksPerMillisecond;

            if (isNewFrame)
            {
                FrameAction?.Invoke();
                Past = Now;
            }
            return isNewFrame;
        }

        public void SpeedUp(int ms)
        {
            IntervalMS = Math.Max(InitialIntervalMS - ms, 0);
            SpeedChange?.Invoke(IntervalMS, InitialIntervalMS);
        }

        #region Constants

        private const int DEFAULT_INTERVAL = 1000;

        #endregion
    }
}
