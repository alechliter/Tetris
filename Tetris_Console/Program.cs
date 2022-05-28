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
        private static IInputHandler<ConsoleKey, Action> inputHandler;
        private static bool isDone = false;

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

        static void InitializeInputHandler()
        {
            inputHandler.KeyEvent[ConsoleKey.UpArrow] = () => tetromino.Move(Direction.Up);
            inputHandler.KeyEvent[ConsoleKey.DownArrow] = () => tetromino.Move(Direction.Down);
            inputHandler.KeyEvent[ConsoleKey.LeftArrow] = () => tetromino.Move(Direction.Left);
            inputHandler.KeyEvent[ConsoleKey.RightArrow] = () => tetromino.Move(Direction.Right);

            inputHandler.KeyEvent[ConsoleKey.C] = () => tetromino.Rotate(Direction.Left);
            inputHandler.KeyEvent[ConsoleKey.V] = () => tetromino.Rotate(Direction.Right);

            inputHandler.KeyEvent[ConsoleKey.N] = () => tetromino.NewPiece();
            inputHandler.KeyEvent[ConsoleKey.Q] = () => isDone = true;
        }

        static void Main(string[] args)
        {
            spwanPoint = new Point(Tracker.GRID_DIM.X / 2 - 1, 0);
            tetromino = new Tetromino(spwanPoint);
            tracker = new Tracker(tetromino);
            view = new ConsoleView();
            frame = new Frame();
            inputHandler = new InputHandler();
            
            InitializeInputHandler();

            tracker.GridUpdate += Display; // Displays the grid whenever the grid is updated
            frame.FrameAction += () => tetromino.Move(Direction.Down); // move the tetromino down each frame

            while(!isDone){
                inputHandler.HandleInput();
                frame.nextFrame();
            }

            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}
