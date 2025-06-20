﻿using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Types;
using Lechliter.Tetris.TetrisConsole.Enumerations;
using Lechliter.Tetris.TetrisConsole.Exceptions;
using Lechliter.Tetris.TetrisConsole.Models;
using System;
using System.Collections.Generic;

namespace Lechliter.Tetris.TetrisConsole
{
    public class ConsoleView : IView<eTextColor, ePieceType>
    {
        public static eTextColor Color { get; protected set; } // current text color

        private readonly ITetrisConsoleLayout<DynamicComponent, List<DynamicComponent>, IntPoint> _Layout;

        static ConsoleView()
        {
            System.Console.Clear();
            System.Console.CursorVisible = false;
            System.Console.SetWindowSize(100, 40);
            System.Console.Title = "Console Tetris";
        }

        public ConsoleView(ITetrisConsoleLayout<DynamicComponent, List<DynamicComponent>, IntPoint> layout)
        {
            _Layout = layout;
        }

        public void Display()
        {
            _Layout.DisplayAll();
        }

        public static ComponentContent[,] ConvertPieceGridToContentGrid(ePieceType[,] blocks)
        {
            int x_dim = blocks.GetLength(0);
            int y_dim = blocks.GetLength(1);

            ComponentContent[,] content = new ComponentContent[x_dim, y_dim];

            for (int y = 0; y < y_dim; y++)
            {
                for (int x = 0; x < x_dim; x++)
                {
                    content[x, y] = new ComponentContent();
                    content[x, y].Value = GetBlock(blocks[x, y]);
                    content[x, y].Color = GetBlockColor(blocks[x, y]);
                }
            }

            return content;
        }

        public static void SetColor(eTextColor color)
        {
            Color = color;
            switch (color)
            {
                case eTextColor.Red:
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case eTextColor.Yellow:
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case eTextColor.Blue:
                    System.Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case eTextColor.DarkBlue:
                    System.Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case eTextColor.Green:
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case eTextColor.Orange:
                    System.Console.ForegroundColor = ConsoleColor.White;
                    break;
                case eTextColor.Purple:
                    System.Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                default:
                    System.Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
        }

        public static eTextColor GetBlockColor(ePieceType type)
        {
            eTextColor color;
            switch (type)
            {
                case ePieceType.I:
                    color = eTextColor.Blue;
                    break;
                case ePieceType.O:
                    color = eTextColor.Yellow;
                    break;
                case ePieceType.T:
                    color = eTextColor.Purple;
                    break;
                case ePieceType.J:
                    color = eTextColor.DarkBlue;
                    break;
                case ePieceType.L:
                    color = eTextColor.Orange;
                    break;
                case ePieceType.S:
                    color = eTextColor.Green;
                    break;
                case ePieceType.Z:
                    color = eTextColor.Red;
                    break;
                case ePieceType.Locked:
                    color = eTextColor.Default;
                    break;
                case ePieceType.Empty:
                    color = eTextColor.Default;
                    break;
                case ePieceType.NotSet:
                    color = eTextColor.Default;
                    break;
                default:
                    color = eTextColor.Default;
                    break;
            }
            return color;
        }

        public static char GetBlock(ePieceType type)
        {
            char symbol = '?';
            switch (type)
            {
                case ePieceType.I:
                    symbol = 'I';
                    break;
                case ePieceType.O:
                    symbol = 'O';
                    break;
                case ePieceType.T:
                    symbol = 'T';
                    break;
                case ePieceType.J:
                    symbol = 'J';
                    break;
                case ePieceType.L:
                    symbol = 'L';
                    break;
                case ePieceType.S:
                    symbol = 'S';
                    break;
                case ePieceType.Z:
                    symbol = 'Z';
                    break;
                case ePieceType.Locked:
                    symbol = '■';
                    break;
                case ePieceType.Empty:
                    symbol = ' ';
                    // symbol = '·'; // unicode: 183
                    break;
                case ePieceType.NotSet:
                    symbol = ' ';
                    break;
                default:
                    throw new TetrisConsoleException("ERROR: Invalid Type (PrintBlock)");
            }
            return symbol;
        }
    }
}
