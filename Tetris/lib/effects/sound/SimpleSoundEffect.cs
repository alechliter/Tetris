using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Effects;
using Lechliter.Tetris.Lib.Systems;

namespace LechliterTetris_Console.Interfaces
{
    public class SimpleSoundEffect : ISoundEffect
    {
        private ITracker<ePieceType, eDirection, eMoveType> _Tracker;

        public SimpleSoundEffect(ITracker<ePieceType, eDirection, eMoveType> tracker)
        {
            _Tracker = tracker;
            Initialize();
        }

        public void PlayCollision()
        {
            Console.Beep(600, 200);
        }

        public void PlaySingleLine()
        {
            PlayLineClearBeep(0);
        }

        public void PlayMultiLine(int numLines)
        {
            for (int lineIndex = 0; lineIndex < numLines; lineIndex++)
            {
                PlayLineClearBeep(lineIndex);
            }
        }

        public void PlayTetrisClear()
        {
            PlayLineClearBeep(3);
        }

        public void PlayBackground()
        {
            return;
        }

        private void Initialize()
        {
            _Tracker.LinesCleared += PlayMultiLine;
            _Tracker.PieceLocked += PlayCollision;
        }

        private void PlayLineClearBeep(int lineCount)
        {
            const int baseFrequency = 400;
            const int frequencyIncrement = 135;
            const int baseDuration = 115;

            Console.Beep(baseFrequency + lineCount * frequencyIncrement, baseDuration);
        }
    }
}
