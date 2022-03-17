using System;
using System.Collections.Generic;

namespace Lechliter.Tetris_Console
{
    class Program
    {
        private static Point spwanPoint;
        private static ITetromino<PieceType, Direction> tetromino;
        private static ITracker<PieceType, Direction> tracker;
        private static IView<TextColor, PieceType> view;

        private static void SimpleTest(ITetromino<PieceType, Direction> tetromino){
            LogPosition(tetromino);
            tetromino.Move(Direction.Down);
            tetromino.Move(Direction.Right);
            tetromino.Move(Direction.Right);
            tetromino.Move(Direction.Left);
            LogPosition(tetromino);
        }
        private static void LogPosition(ITetromino<PieceType, Direction> piece)
        {
            Console.WriteLine("x: {0} y: {1}", piece.Position.x, piece.Position.y);
        }

        private static void Display(){
            Console.Clear();
            view?.Display(tracker.AllPieces);
        }
        static void Main(string[] args)
        {
            spwanPoint = new Point(Tracker.GRID_DIM.X, 0);
            tetromino = new Tetromino(spwanPoint);

            // ! TEST: DELETE LATER
            SimpleTest(tetromino);
            // !

            tracker = new Tracker(tetromino);
            view = new ConsoleView();

            tracker.GridUpdate += Display; // Displays the grid whenever the grid is updated

            // ! TEST: DELETE LATER
            tracker.AllPieces[3, 10] = PieceType.O;
            tracker.AllPieces[4, 10] = PieceType.T;
            tracker.AllPieces[5, 10] = PieceType.J;
            tracker.AllPieces[6, 10] = PieceType.L;
            tracker.AllPieces[7, 10] = PieceType.I;
            tracker.AllPieces[8, 10] = PieceType.S;
            tracker.AllPieces[9, 10] = PieceType.Z;
            // !

            // ! TEST: DELETE LATER
            SimpleTest(tetromino);
            // !

            tetromino.Rotate(Direction.Left);
            
            tetromino.Rotate(Direction.Left);

            // long past = DateTime.Now.Ticks;
            // long now = past;
            // long ticks_diff = now - past;
            // long interval_ms = 300;

            // while(true){
            //     now = DateTime.Now.Ticks;
            //     ticks_diff = now - past;
            //     if(ticks_diff >= interval_ms * TimeSpan.TicksPerMillisecond){
            //         view.Display(Tracker.AllPieces);
            //         past = now;
            //     }
            // }

            // IList<ConsoleKey> SupportedKeys = new List<ConsoleKey>() {
            //     ConsoleKey.C, ConsoleKey.V,
            //     ConsoleKey.LeftArrow, ConsoleKey.RightArrow, ConsoleKey.UpArrow, ConsoleKey.DownArrow
            // };
            // foreach(ConsoleKey key in SupportedKeys){
            //     KeyPress.AddKey(key);
            // }
            // KeyPress.AddPressedEvent(ConsoleKey.LeftArrow, () => tetromino.Move(Direction.Left));
            // KeyPress.AddPressedEvent(ConsoleKey.RightArrow, () => tetromino.Move(Direction.Right));
            // KeyPress.AddPressedEvent(ConsoleKey.UpArrow, () => tetromino.Move(Direction.Up));
            // KeyPress.AddPressedEvent(ConsoleKey.DownArrow, () => tetromino.Move(Direction.Down));

            // while(true){
            //     KeyPress.HandleAllEvents();
            // }


            long past = DateTime.Now.Ticks;
            long now = past;
            long ticks_diff = now - past;
            long interval_ms = 1000;

            bool isDone = false;
            while(!isDone){

                ConsoleKeyInfo key;
                if(Console.KeyAvailable){
                    key = Console.ReadKey(true);

                    switch(key.Key){
                        case ConsoleKey.LeftArrow:
                            tetromino.Move(Direction.Left);
                            break;
                        case ConsoleKey.RightArrow:
                            tetromino.Move(Direction.Right);
                            break;
                        case ConsoleKey.UpArrow:
                            // TODO: Replace with Drop 
                            tetromino.Move(Direction.Up);
                            break;
                        case ConsoleKey.DownArrow:
                            tetromino.Move(Direction.Down);
                            break;  
                        case ConsoleKey.C:
                            tetromino.Rotate(Direction.Left);
                            break;  
                        case ConsoleKey.V:
                            tetromino.Rotate(Direction.Right);
                            break; 
                        case ConsoleKey.Q:
                            isDone = true;
                            break;
                        case ConsoleKey.N:
                            tetromino.NewPiece();
                            break;
                    }
                }

                now = DateTime.Now.Ticks;
                ticks_diff = now - past;
                if(ticks_diff >= interval_ms * TimeSpan.TicksPerMillisecond){
                    tetromino.Move(Direction.Down);
                    past = now;
                }

            }

            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}
