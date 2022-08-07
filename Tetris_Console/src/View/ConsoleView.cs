using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    public class ConsoleView : IView<eTextColor, ePieceType>
    {
        private static readonly int ConsoleOriginX;

        private static readonly int ConsoleOriginY;

        public eTextColor Color { get; protected set; } // current text color

        /* Constructor */
        static ConsoleView()
        {
            Console.Clear();
            Console.CursorVisible = false;
            ConsoleOriginX = Console.CursorLeft;
            ConsoleOriginY = Console.CursorTop;
        }

        /* Private Method */
        private static void SetColor(eTextColor color)
        {
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
        private static void PrintBlock(ePieceType type, int x, int y)
        {
            char symbol = '?';
            switch (type)
            {
                case ePieceType.I:
                    SetColor(eTextColor.Blue);
                    symbol = 'I';
                    break;
                case ePieceType.O:
                    SetColor(eTextColor.Yellow);
                    symbol = 'O';
                    break;
                case ePieceType.T:
                    SetColor(eTextColor.Purple);
                    symbol = 'T';
                    break;
                case ePieceType.J:
                    SetColor(eTextColor.DarkBlue);
                    symbol = 'J';
                    break;
                case ePieceType.L:
                    SetColor(eTextColor.Orange);
                    symbol = 'L';
                    break;
                case ePieceType.S:
                    SetColor(eTextColor.Green);
                    symbol = 'S';
                    break;
                case ePieceType.Z:
                    SetColor(eTextColor.Red);
                    symbol = 'Z';
                    break;
                case ePieceType.Locked:
                    SetColor(eTextColor.Default);
                    symbol = 'X';
                    break;
                case ePieceType.Empty:
                    SetColor(eTextColor.Default);
                    symbol = '·'; // unicode: 183
                    break;
                case ePieceType.NotSet:
                    SetColor(eTextColor.Default);
                    symbol = ' ';
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
        public void Display(ePieceType[,] blocks)
        {
            for (int y = 0; y < Tracker.BOUNDS_DIM.Y; y++)
            {
                for(int x = 0; x < Tracker.BOUNDS_DIM.X; x++)
                {
                    PrintBlock(blocks[x, y], x, y);                  
                }
                Console.WriteLine();
            }
        }
    }
}
