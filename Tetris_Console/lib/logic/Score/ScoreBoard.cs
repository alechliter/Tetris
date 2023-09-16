using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    class ScoreBoard : IScore
    {
        public int Score { get; protected set; }

        public int Level { get; protected set; }

        private static readonly double K = 1.05;

        private static readonly double Scale = 15;

        private int NumClearedLines;

        public event Action<int> NextLevel;

        public event Action<int> UpdatedScore;

        public ScoreBoard()
        {
            Level = 0;
            Score = 0;
            NumClearedLines = 0;
        }

        public void Increase(int numLines)
        {
            Score += (int)Math.Floor(Scale * Math.Exp(K * numLines)) * (Level + 1) / 10 * 10;
            AdvanceLevel(numLines);
            UpdatedScore?.Invoke(Score);
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
