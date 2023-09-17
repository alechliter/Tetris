using System;
using System.Collections.Generic;
using System.Text;

namespace LechliterTetris_Console.Interfaces
{
    public interface ISoundEffect
    {
        void PlayCollision();

        void PlaySingleLine();

        void PlayMultiLine(int numLines);

        void PlayTetrisClear();

        void PlayBackground();
    }
}
