using Lechliter.Tetris.Lib.Models;
using Lechliter.Tetris.Lib.Types;
using System;

namespace Lechliter.Tetris.TetrisConsole
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

        public event Action<int> Display;

        public int ComponentID { get; }

        public int Spacing { get; protected set; }

        static DynamicComponent()
        {
            DynamicComponent.NextID = 0;
        }

        public DynamicComponent(int layer = 0, IntPoint origin = new IntPoint(), int spacing = 1)
        {
            this.Layer = layer;
            this.Origin = origin;
            this.Grid = new ComponentContent[,] { };
            this.Spacing = spacing;

            this.ComponentID = DynamicComponent.NextID;
            DynamicComponent.NextID++;
        }

        public void OnUpdate(ComponentContent[,] grid)
        {
            this.Grid = grid;
            this.Display?.Invoke(this.ComponentID);
        }
    }
}
