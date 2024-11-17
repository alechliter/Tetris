using Lechliter.Tetris.TetrisConsole.Enumerations;

namespace Lechliter.Tetris.TetrisConsole.Models
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
