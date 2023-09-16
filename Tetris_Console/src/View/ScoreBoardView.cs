using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    class ScoreBoardView
    {
        public DynamicComponent ScoreBoard { get; protected set; }

        public IntDimensions Dim 
        { 
            get
            {
                return new IntDimensions(WIDTH, formatScore().Length);
            }
        }

        private int Score = 0;

        private int Level = 0;

        private const int WIDTH = 20;

        public ScoreBoardView(int layer, IntPoint position)
        {
            ScoreBoard = new DynamicComponent(layer, position);
            ScoreBoard.Grid = DisplayScore(0, 0);
        }

        public void OnUpdate(int score = -1, int level = -1)
        {
            if (level > 0)
            {
                this.Level = level;
            }

            if (score > 0)
            {
                this.Score = score;
            }

            ScoreBoard.OnUpdate(DisplayScore(this.Score, this.Level));
        }

        private ComponentContent[,] DisplayScore(int score, int level)
        {
            string[] scoreDisplay = formatScore(score, level);
            ComponentContent[,] content = new ComponentContent[WIDTH, scoreDisplay.Length];

            for (int row = 0; row < scoreDisplay.Length; row++)
            {
                eTextColor color = row > 0 && row < scoreDisplay.Length ? eTextColor.Orange : eTextColor.Default;
                for (int col = 0; col < WIDTH; col++)
                {
                    content[col, row] = new ComponentContent(scoreDisplay[row][col], color);
                }
            }

            return content;
        }

        private string[] formatScore(int score = -1, int level = -1)
        {
            string bound = "".PadLeft(WIDTH, '_');
            string title = "Score";
            string[] lines =
            {
                $"{title} {score, WIDTH - 6}",
                bound,
                $"Level {level, WIDTH - 6}"
            };

            return lines;
        }

    }
}
