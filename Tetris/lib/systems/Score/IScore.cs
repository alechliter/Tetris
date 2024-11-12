using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris.Lib.Systems
{
    public interface IScore
    {
        public int Score { get; }

        public int Level { get; }

        public event Action<int> NextLevel;

        public event Action<int> UpdatedScore;

        public void Increase(int numLines);
    }
}
