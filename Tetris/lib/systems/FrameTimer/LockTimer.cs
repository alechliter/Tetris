namespace Lechliter.Tetris.Lib.Systems
{
    public class LockTimer : IFrameTimer
    {
        public int FramesRemaining { get; private set; }

        public bool IsRunning { get; private set; }

        public event Action? TimerFinished;

        private int InitialFrameCount { get; set; }

        private const int DEFAULT_FRAME_COUNT = 5;

        public LockTimer(int frame_count = DEFAULT_FRAME_COUNT)
        {
            Initialize(frame_count);
        }

        public void Start()
        {
            IsRunning = true;
        }

        public void Stop()
        {
            IsRunning = false;
        }

        public void CountDown()
        {
            if (IsRunning)
            {
                FramesRemaining--;
                if (FramesRemaining <= 0)
                {
                    Stop();
                    Reset();
                    TimerFinished?.Invoke();
                }
            }
        }

        public void Reset()
        {
            FramesRemaining = InitialFrameCount;
        }

        public void SetTimer(int numFrames)
        {
            InitialFrameCount = numFrames;
        }

        private void Initialize(int frame_count)
        {
            SetTimer(frame_count);
            Reset();
            IsRunning = false;
        }
    }
}
