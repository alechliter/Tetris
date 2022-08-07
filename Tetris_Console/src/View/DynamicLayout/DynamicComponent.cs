using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public class DynamicComponent : IComponent<IntPoint, ComponentContent[,], IntDimensions>
    {
        public static int NextID { get; protected set; }

        public IntPoint Origin { get; set; }

        public IntDimensions Dimensions { get; set; }

        public int Layer { get; set; }

        public ComponentContent[,] Grid { get; set; }

        public Action<int> Display { get; set; }

        public int ComponentID { get; }

        static DynamicComponent()
        {
            DynamicComponent.NextID = 0;
        }

        public DynamicComponent()
        {
            this.ComponentID = DynamicComponent.NextID;
            DynamicComponent.NextID++;
        }
    }
}
