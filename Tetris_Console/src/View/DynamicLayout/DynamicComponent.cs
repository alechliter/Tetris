using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public class DynamicComponent : IComponent<IntPoint, ComponentContent[,], IntDimensions>
    {
        public static int NextID { get; protected set; }

        public IntPoint Origin { get; set; }

        public IntDimensions Dimensions
        {
            get
            {
                int width = this.Grid.GetLength(0);
                int height = this.Grid.GetLength(1);
                return new IntDimensions(width, height);
            }
        }

        public int Layer { get; set; }

        public ComponentContent[,] Grid { get; set; }

        public Action<int> Display { get; set; }

        public int ComponentID { get; }

        static DynamicComponent()
        {
            DynamicComponent.NextID = 0;
        }

        public DynamicComponent(int layer = 0, IntPoint origin = new IntPoint())
        {
            this.Layer = layer;
            this.Origin = origin;
            this.Grid = new ComponentContent[,] { };

            this.ComponentID = DynamicComponent.NextID;
            DynamicComponent.NextID++;
        }
    }
}
