using Tetris.lib.Design.Helpers;

namespace Lechliter.Tetris.Lib.Systems
{
    public class ScoreBoard : IScore
    {
        public int Score { get; protected set; }

        public int Level { get; protected set; }

        public event Action<int>? NextLevel;

        public event Action<int>? UpdatedScore;

        private static readonly double LineMultiplier;

        private static readonly double Scale;

        private int NumClearedLines;

        static ScoreBoard()
        {
            LineMultiplier = ConfigurationHelper.GetFloat("ScoreLineMultiplier");
            Scale = ConfigurationHelper.GetFloat("ScoreScale");
        }

        public ScoreBoard()
        {
            Level = 0;
            Score = 0;
            NumClearedLines = 0;
        }

        public void Increase(int numLines)
        {
            Score += CalculateScore(numLines);
            AdvanceLevel(numLines);
            UpdatedScore?.Invoke(Score);
        }

        private int CalculateScore(int numLines)
        {
            int levelMultiplier = Level + 1;
            int lineScore = (int)Math.Floor(Scale * Math.Exp(LineMultiplier * numLines));
            return levelMultiplier * lineScore / 10 * 10;
        }

        private void AdvanceLevel(int numLines)
        {
            NumClearedLines += AdjustedLineAmount(numLines);
            if (NumClearedLines >= 5 * Level)
            {
                Level++;
                NumClearedLines = 0;
                NextLevel?.Invoke(Level);
            }
        }

        private int AdjustedLineAmount(int numLines)
        {
            return 2 * numLines - 1;
        }
    }
}
