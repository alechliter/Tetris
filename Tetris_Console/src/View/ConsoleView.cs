using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public class ConsoleView : IView<eTextColor, ePieceType>
    {
        /* Private Members */

        /* Public Members */
        public static DynamicConsoleLayout Layout;

        public static eTextColor Color { get; protected set; } // current text color

        /* Constructor */
        static ConsoleView()
        {
            Console.Clear();
            Console.CursorVisible = false;
            Console.SetWindowSize(100, 40);
            Console.Title = "Console Tetris";
            Layout = new DynamicConsoleLayout();
        }

        /* Private Methods */

        /* Public Method */
        public void Display()
        {
            Layout.DisplayAll();
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
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case eTextColor.Yellow:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case eTextColor.Blue:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case eTextColor.DarkBlue:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case eTextColor.Green:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case eTextColor.Orange:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case eTextColor.Purple:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
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
                    symbol = 'X';
                    break;
                case ePieceType.Empty:
                    symbol = '·'; // unicode: 183
                    break;
                case ePieceType.NotSet:
                    symbol = ' ';
                    break;
                default:
                    ErrorMessageHandler.DisplayMessage("ERROR: Invalid Type (PrintBlock)");
                    break;
            }
            return symbol;
        }
    }
}
