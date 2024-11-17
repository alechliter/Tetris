namespace Lechliter.Tetris.Lib.Objects
{
    public interface IFrame
    {
        /// <summary>
        /// Action event to invoke for each frame.
        /// </summary>
        public event Action FrameAction;

        /// <summary>
        /// Action event to invoke on frame rate changes.
        /// </summary>
        public event Action<long, long> SpeedChange;

        /// <summary>
        /// Determines if the next frame is reached. Invokes [frameAction] for each frame.
        /// </summary>
        bool NextFrame();

        /// <summary>
        /// Decreases the time between frames;
        /// </summary>
        void SpeedUp(int ms = 150);
    }
}
