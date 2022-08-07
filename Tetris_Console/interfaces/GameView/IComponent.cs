using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public interface IComponent<TPoint, TGrid>
    {
        /// <summary>
        /// The next ID to assign to a new component.
        /// </summary>
        public static int NextID { get; protected set; }

        /// <summary>
        /// The origin of the component.
        /// </summary>
        public TPoint Origin { get; set; }

        /// <summary>
        /// The layer number of the component in the layout.
        /// </summary>
        public int Layer { get; set; }

        /// <summary>
        /// The unique component identication index.
        /// </summary>
        public int ComponentID { get; protected set; }

        /// <summary>
        /// The contents of the component to display in a layout.
        /// </summary>
        public TGrid Grid { get; set; }

        /// <summary>
        /// An event that emits when the component needs to be displayed.
        /// </summary>
        public Action<int> Display { get; set; }

        /// <summary>
        /// A method that subscribes to the component's content provider.
        /// </summary>
        /// <param name="grid">The new grid content to display.</param>
        public void OnUpdate(TGrid grid)
        {
            this.Display.Invoke(this.ComponentID);
        }
    }
}
