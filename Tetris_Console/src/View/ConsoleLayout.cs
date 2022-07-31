using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public struct IntPoint
    {
        public int X, Y;
        public IntPoint(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    public struct Component
    {
        public PieceType[,] Elements;
        public IntPoint Position;
        public IntDimensions Size;

        public Component(PieceType[,] elements, IntPoint position)
        {
            Elements = elements;
            Position = position;
            Size = new IntDimensions(elements.GetLength(0), elements.GetLength(1));
        }
    }

    public class ConsoleLayout : ILayout<IntPoint, PieceType[,], Component>
    {
        // Private Members
        private const int GRID_WIDTH = 20;
        private const int GRID_HEIGHT = 50;

        private IntDimensions Dimensions;
        private List<Component> Components;

        // Public Members
        public IntPoint Origin { get; }

        public PieceType[,] Grid { get; protected set; }

        // Constructors
        public ConsoleLayout()
        {
            Origin = new IntPoint(Console.CursorLeft, Console.CursorTop);
            Dimensions = new IntDimensions(GRID_WIDTH, GRID_HEIGHT);
            Grid = new PieceType[GRID_WIDTH, GRID_HEIGHT];
            Components = new List<Component>();
        }

        // Private Methods
        private void AdjustGridDimensions(Component component)
        {
            if (component.Position.X > Dimensions.X || component.Position.Y > Dimensions.Y)
            {

                Dimensions.X = component.Position.X + component.Size.X;
                Dimensions.Y = component.Position.Y + component.Size.Y;
            }
        }

        private void UpdateGrid()
        {
            NewGrid();
            foreach (Component component in Components)
            {
                for (int x = component.Position.X; x < component.Size.X; x++)
                {
                    for (int y = component.Position.Y; y < component.Size.Y; y++)
                    {
                        Grid[x, y] = component.Elements[x, y];
                    }
                }
            }
        }

        private void NewGrid()
        {
            Grid = new PieceType[Dimensions.X, Dimensions.Y];
            for (int x = 0; x < Dimensions.X; x++)
            {
                for (int y = 0; y < Dimensions.Y; y++)
                {
                    Grid[x, y] = PieceType.NotSet;
                }
            }
        }

        // Public Methods
        public void AddToGrid(Component component, IntPoint position)
        {
            Components.Add(component);
            AdjustGridDimensions(component);
            UpdateGrid();
        }
    }
}
