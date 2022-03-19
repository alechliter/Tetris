using System;
using System.Collections.Generic;

namespace Lechliter.Tetris_Console
{
    public class Program
    {
        private static Point spwanPoint;
        private static ITetromino<PieceType, Direction> tetromino;
        private static ITracker<PieceType, Direction> tracker;
        private static IView<TextColor, PieceType> view;
        private static IFrame frame;

        private static void SimpleTest(ITetromino<PieceType, Direction> tetromino){
            LogPosition(tetromino);
            tetromino.Move(Direction.Down);
            tetromino.Move(Direction.Right);
            tetromino.Move(Direction.Right);
            tetromino.Move(Direction.Left);
            LogPosition(tetromino);
        }

        private static void GeneratePiecesTest(ITracker<PieceType, Direction> tracker)
        {
            tracker.AllPieces[3, 10] = PieceType.O;
            tracker.AllPieces[4, 10] = PieceType.T;
            tracker.AllPieces[5, 10] = PieceType.J;
            tracker.AllPieces[6, 10] = PieceType.L;
            tracker.AllPieces[7, 10] = PieceType.I;
            tracker.AllPieces[8, 10] = PieceType.S;
            tracker.AllPieces[9, 10] = PieceType.Z;
        }

        private static void LogPosition(ITetromino<PieceType, Direction> piece)
        {
            Console.WriteLine("x: {0} y: {1}", piece.Position.x, piece.Position.y);
        }

        private static void Display(){
            Console.Clear();
            view?.Display(tracker.AllPieces);
        }

        private static bool HandleInput()
        {
            bool isDone = false;
            ConsoleKeyInfo key;
            if (Console.KeyAvailable)
            {
                key = Console.ReadKey(true);

                switch (key.Key)
                {
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
            return isDone;
        }

        static void Main(string[] args)
        {
            spwanPoint = new Point(Tracker.GRID_DIM.X / 2 - 1, 0);
            tetromino = new Tetromino(spwanPoint);
            tracker = new Tracker(tetromino);
            view = new ConsoleView();
            frame = new Frame();

            tracker.GridUpdate += Display; // Displays the grid whenever the grid is updated
            frame.FrameAction += () => tetromino.Move(Direction.Down); // move the tetromino down each frame

            // ! TEST: DELETE LATER
            SimpleTest(tetromino);
            // !

            // ! TEST: DELETE LATER
            GeneratePiecesTest(tracker);
            // !

            // ! TEST: DELETE LATER
            SimpleTest(tetromino);
            // !

            bool isDone = false;
            while(!isDone){
                isDone = HandleInput();
                frame.nextFrame();
            }

            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}
