using Tetris.lib.Design.Helpers;

namespace Lechliter.Tetris.Lib.Objects
{
    public class Frame : IFrame
    {

        public event Action? FrameAction;

        private long Past;

        private long Now;

        private long TicksDiff;

        private long IntervalMS;

        private long InitialInteravalMS;

        private static int DefaultInterval
        {
            get
            {
                return ConfigurationHelper.GetInt("FrameIntervalDefault", DEFAULT_INTERVAL);
            }
        }

        public Frame(long? interval = null)
        {
            Past = DateTime.Now.Ticks;
            Now = Past;
            TicksDiff = Now - Past;
            IntervalMS = interval.HasValue ? interval.Value : DefaultInterval;
            InitialInteravalMS = IntervalMS;
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
            IntervalMS = InitialInteravalMS - ms * 100;
        }

        #region Constants

        private const int DEFAULT_INTERVAL = 1000;

        #endregion
    }
}
