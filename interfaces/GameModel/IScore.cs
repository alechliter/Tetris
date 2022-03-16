using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console.Interfaces
{
    interface IScore
    {
        int Score { get; protected set; }
        void Increase(int numLines);
    }
}
