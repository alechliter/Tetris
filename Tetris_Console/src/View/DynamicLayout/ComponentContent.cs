using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public struct ComponentContent
    {
        public char Value;

        public eTextColor Color;

        public ComponentContent(char value, eTextColor color)
        {
            Value = value;
            Color = color;
        }
    }
}
