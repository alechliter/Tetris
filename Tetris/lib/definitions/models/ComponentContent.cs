using Lechliter.Tetris.Lib.Definitions;

namespace Lechliter.Tetris.Lib.Models
{
    /// <summary>
    /// TODO: Remove from library. This should be a view specific concept.
    /// </summary>
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
