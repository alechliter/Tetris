using System;
using System.Collections.Generic;
using System.Text;

namespace LechliterTetris_Console.Interfaces
{
    interface ISoundEffect <TSoundPlayer>
    {
        struct Sounds
        {
            TSoundPlayer collision;
            TSoundPlayer singleLine;
            TSoundPlayer multiLine;
            TSoundPlayer tetrisClear;
            TSoundPlayer background;
        }

        Sounds Players { get; protected set; }
        
        void PlayCollision();
        void PlaySingleLine();
        void PlayMultiLine(int numLines);
        void PlayTetrisClear();
        void PlayBackground();
    }
}
