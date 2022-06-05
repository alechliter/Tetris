using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public enum TextColor
    {
        Red, Yellow, Blue, Green, Orange, Purple, DarkBlue, Default
    }
    public class ConsoleView : IView<TextColor, PieceType>
    {
        private static readonly int ConsoleOriginX;

        private static readonly int ConsoleOriginY;

        public TextColor Color { get; protected set; } // current text color

        /* Constructor */
        static ConsoleView()
        {
            Console.Clear();
            ConsoleOriginX = Console.CursorLeft;
            ConsoleOriginY = Console.CursorTop;
        }

        /* Private Method */
        private static void SetColor(TextColor color)
        {
            switch (color)
            {
                case TextColor.Red:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case TextColor.Yellow:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case TextColor.Blue:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case TextColor.DarkBlue:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case TextColor.Green:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case TextColor.Orange:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case TextColor.Purple:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }
        }
        private static void PrintBlock(PieceType type, int x, int y)
        {
            char symbol = '?';
            switch (type)
            {
                case PieceType.I:
                    SetColor(TextColor.Blue);
                    symbol = 'I';
                    break;
                case PieceType.O:
                    SetColor(TextColor.Yellow);
                    symbol = 'O';
                    break;
                case PieceType.T:
                    SetColor(TextColor.Purple);
                    symbol = 'T';
                    break;
                case PieceType.J:
                    SetColor(TextColor.DarkBlue);
                    symbol = 'J';
                    break;
                case PieceType.L:
                    SetColor(TextColor.Orange);
                    symbol = 'L';
                    break;
                case PieceType.S:
                    SetColor(TextColor.Green);
                    symbol = 'S';
                    break;
                case PieceType.Z:
                    SetColor(TextColor.Red);
                    symbol = 'Z';
                    break;
                case PieceType.Locked:
                    SetColor(TextColor.Default);
                    symbol = 'X';
                    break;
                case PieceType.Empty:
                    SetColor(TextColor.Default);
                    symbol = '·'; // unicode: 183
                    break;
                default:
                    Console.Error.WriteLine("ERROR: Invalid Type (PrintBlock)");
                    break;
            }
            WriteAt(symbol.ToString(), x * 2 , y);
        }

        protected static void WriteAt(string s, int x, int y)
        {
            try
            {
                Console.SetCursorPosition(ConsoleOriginY + x, ConsoleOriginY + y);
                Console.Write(s);
            }
            catch (ArgumentOutOfRangeException e)
            {
                Console.Clear();
                Console.WriteLine(e.Message);
            }
        }

        /* Public Method */
        public void Display(PieceType[,] blocks)
        {
            for (int y = 0; y < Tracker.GRID_DIM.Y; y++)
            {
                for(int x = 0; x < Tracker.GRID_DIM.X; x++)
                {
                    PrintBlock(blocks[x, y], x, y);                  
                }
                Console.WriteLine();
            }
        }
    }
}
