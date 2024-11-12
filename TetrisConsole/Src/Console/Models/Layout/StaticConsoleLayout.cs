using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Types;
using Lechliter.Tetris.TetrisConsole.Models;
using System;
using System.Collections.Generic;

namespace Lechliter.Tetris.TetrisConsole
{
    public class StaticConsoleLayout : ILayout<IntPoint, ePieceType[,], StaticComponent>
    {
        // Private Members
        private const int GRID_WIDTH = 20;
        private const int GRID_HEIGHT = 50;

        private IntDimensions Dimensions;
        private List<StaticComponent> Components;

        // Public Members
        public IntPoint Origin { get; }

        public ePieceType[,] Grid { get; protected set; }

        // Constructors
        public StaticConsoleLayout()
        {
            Origin = new IntPoint(Console.CursorLeft, Console.CursorTop);
            Dimensions = new IntDimensions(GRID_WIDTH, GRID_HEIGHT);
            Grid = new ePieceType[GRID_WIDTH, GRID_HEIGHT];
            Components = new List<StaticComponent>();
        }

        // Private Methods
        private void AdjustGridDimensions(StaticComponent component)
        {
            if (component.Position.X > Dimensions.X || component.Position.Y > Dimensions.Y)
            {
                Dimensions = new IntDimensions(component.Position.X + component.Size.X, component.Position.Y + component.Size.Y);
            }
        }

        private void UpdateGrid()
        {
            NewGrid();
            foreach (StaticComponent component in Components)
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
            Grid = new ePieceType[Dimensions.X, Dimensions.Y];
            for (int x = 0; x < Dimensions.X; x++)
            {
                for (int y = 0; y < Dimensions.Y; y++)
                {
                    Grid[x, y] = ePieceType.NotSet;
                }
            }
        }

        // Public Methods
        public void AddToGrid(StaticComponent component, IntPoint position)
        {
            Components.Add(component);
            AdjustGridDimensions(component);
            UpdateGrid();
        }
    }
}
