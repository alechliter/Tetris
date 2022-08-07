using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public struct IntDimensions
    {
        public int X, Y;
        public IntDimensions(int width, int height)
        {
            this.X = width;
            this.Y = height;
        }
    }
}
