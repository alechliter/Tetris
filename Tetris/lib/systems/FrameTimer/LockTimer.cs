namespace Lechliter.Tetris.Lib.Systems
{
    public class LockTimer : IFrameTimer
    {
        public int FramesRemaining { get { return remaining_frames; } }

        public bool IsRunning { get { return isCounting; } }

        public event Action TimerFinished;

        private int remaining_frames;

        private int initial_frame_count;

        private bool isCounting;

        private const int DEFAULT_FRAME_COUNT = 5;

        public LockTimer(int frame_count = DEFAULT_FRAME_COUNT)
        {
            Initialize(frame_count);
        }

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

        private void Initialize(int frame_count)
        {
            SetTimer(frame_count);
            Reset();
            isCounting = false;
        }
    }
}
