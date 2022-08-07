using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public interface IDynamicLayout<TComponent, TComponentCollection, TPoint> where TComponentCollection: ICollection<TComponent>
    {
        /// <summary>
        /// A collection of components to display
        /// </summary>
        public TComponentCollection Components { get; }

        /// <summary>
        /// The origin of the overall layout
        /// </summary>
        public TPoint Origin { get; set; }

        /// <summary>
        /// Displays all components in the layout
        /// </summary>
        public void DisplayAll();
        
        /// <summary>
        /// Displays the given component in the layout, adding the component to the layout if it does not already exist.
        /// </summary>
        /// <param name="component">The component to display in the layout.</param>
        public void DisplayComponent(TComponent component);

        /// <summary>
        /// Adds a component to the layout.
        /// </summary>
        /// <param name="component">A new component to add to the layout.</param>
        public void AddComponent(TComponent component);
    }
}
