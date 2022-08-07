using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console.src.View.DynamicLayout
{
    public class Component : IComponent<IntPoint, char[,]>
    {
        public IntPoint Origin { get; set; }
        public int Layer { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public char[,] Grid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Action<int> Display { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int ComponentID { get; }
    }
}
