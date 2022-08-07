using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public class ConsoleDynamicLayout : IDynamicLayout<DynamicComponent, List<DynamicComponent>, IntPoint>
    {
        // Public Members
        public IntPoint Origin { get; set; }

        public List<DynamicComponent> Components {
            get {
                List<DynamicComponent> components = new List<DynamicComponent>();

                foreach (List<DynamicComponent> componentList in this.Layers.Values)
                {
                    components.AddRange(componentList);
                }

                return components;
            }
            protected set {
                foreach (DynamicComponent component in value)
                {
                    AddComponent(component);
                }
            }
        }

        // Private Members
        private static readonly IntPoint ConsoleOrigin;

        private static IntPoint CursorPosition { get; set; }

        private SortedDictionary<int, List<DynamicComponent>> Layers { get; set; }

        // Constructors
        static ConsoleDynamicLayout()
        {
            try
            {
                ConsoleOrigin = new IntPoint(Console.CursorLeft, Console.CursorTop);
            } catch
            {
                ConsoleOrigin = new IntPoint(0, 0);
            }
        }

        public ConsoleDynamicLayout()
        {
            this.Origin = new IntPoint(ConsoleOrigin);
            this.Components = new List<DynamicComponent>();
            this.Layers = new SortedDictionary<int, List<DynamicComponent>>();
        }

        // Public Methods
        public void AddComponent(DynamicComponent component)
        {
            if (IsComponentIDUnique(component))
            {
                component.Display += (int id) => DisplayComponent(component);
                List<DynamicComponent> components;
                if (this.Layers.ContainsKey(component.Layer))
                {
                    this.Layers.TryGetValue(component.Layer, out components);
                    components.Add(component);
                } 
                else
                {
                    components = new List<DynamicComponent>() { component };
                    this.Layers.Add(component.Layer, components);
                }
            }
        }

        public void DisplayAll()
        {
            foreach (KeyValuePair<int, List<DynamicComponent>> layer in this.Layers)
            {
                foreach (DynamicComponent component in layer.Value)
                {
                    DisplayComponent(component, true);
                }
            }
        }

        public void DisplayComponent(DynamicComponent component)
        {
            DisplayComponent(component, false);
        }

        public void DisplayComponent(DynamicComponent component, bool ignoreLayer)
        {
            AddComponent(component);
            PrintComponent(component);
            if (!ignoreLayer)
            {
                foreach (KeyValuePair<int, List<DynamicComponent>> layer in this.Layers)
                {
                    if (layer.Key > component.Layer)
                    {
                        foreach(DynamicComponent layerComponent in layer.Value)
                        {
                            DisplayComponent(layerComponent, true);
                        }
                    }
                }
            }
        }

        // Private Methods
        private void PrintComponent(DynamicComponent component)
        {
            for (int y = 0; y < component.Dimensions.Y; y++)
            {
                SetCursorPosition(component.Origin);
                for (int x = 0; x < component.Dimensions.X; x++)
                {
                    MoveCursor(x, y);
                    ConsoleView.SetColor(component.Grid[x, y].Color);
                    WriteAt(component.Grid[x, y].Value.ToString(), x * component.Spacing, y);
                }
            }
            ResetCursorPosition();
        }

        private void SetCursorPosition(IntPoint point)
        {
            CursorPosition = ConsoleOrigin + this.Origin + point;
            Console.SetCursorPosition(CursorPosition.X, CursorPosition.Y);
        }

        private void ResetCursorPosition()
        {
            Console.SetCursorPosition(ConsoleOrigin.X, ConsoleOrigin.Y);
        }

        private void MoveCursor(int x, int y)
        {
            try
            {
                Console.CursorLeft += x;
                Console.CursorTop += y;
            }
            catch
            {
                ErrorMessageHandler.DisplayMessage($"Error: Attempted to Write at: {Console.CursorLeft + x}, {Console.CursorTop + y}");
            }
        }

        protected static void WriteAt(string s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(CursorPosition.X + x, CursorPosition.Y + y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }

        private bool IsComponentIDUnique(DynamicComponent component)
        {
            return this.Components.FindIndex((DynamicComponent c) => c.ComponentID == component.ComponentID) < 0;
        }
    }
}
