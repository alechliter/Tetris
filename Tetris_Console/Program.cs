using System;
using System.Collections.Generic;

namespace Lechliter.Tetris_Console
{
    public class Program
    {
        private static Point spwanPoint;
        private static ITetromino<ePieceType, eDirection, eMoveType> tetromino;
        private static ITracker<ePieceType, eDirection, eMoveType> tracker;
        private static IView<eTextColor, ePieceType> view;
        private static IFrame frame;
        private static IInputHandler<ConsoleKey, Action> inputHandler;
        private static bool isDone = false;

        private static void SimpleTest(ITetromino<ePieceType, eDirection, eMoveType> tetromino){
            LogPosition(tetromino);
            tetromino.Move(eDirection.Down);
            tetromino.Move(eDirection.Right);
            tetromino.Move(eDirection.Right);
            tetromino.Move(eDirection.Left);
            LogPosition(tetromino);
        }

        private static void GeneratePiecesTest(ITracker<ePieceType, eDirection, eMoveType> tracker)
        {
            tracker.AllPieces[3, 10] = ePieceType.O;
            tracker.AllPieces[4, 10] = ePieceType.T;
            tracker.AllPieces[5, 10] = ePieceType.J;
            tracker.AllPieces[6, 10] = ePieceType.L;
            tracker.AllPieces[7, 10] = ePieceType.I;
            tracker.AllPieces[8, 10] = ePieceType.S;
            tracker.AllPieces[9, 10] = ePieceType.Z;
        }

        private static void LogPosition(ITetromino<ePieceType, eDirection, eMoveType> piece)
        {
            Console.WriteLine("x: {0} y: {1}", piece.Position.x, piece.Position.y);
        }

        private static void Display(){
            view?.Display(tracker.AllPieces);
        }

        static void InitializeInputHandler()
        {
            inputHandler.KeyEvent[ConsoleKey.UpArrow] = () => (tetromino as Tetromino).Drop(tracker);//tetromino.Move(Direction.Up);
            inputHandler.KeyEvent[ConsoleKey.DownArrow] = () => tetromino.Move(eDirection.Down);
            inputHandler.KeyEvent[ConsoleKey.LeftArrow] = () => tetromino.Move(eDirection.Left);
            inputHandler.KeyEvent[ConsoleKey.RightArrow] = () => tetromino.Move(eDirection.Right);

            inputHandler.KeyEvent[ConsoleKey.C] = () => tetromino.Rotate(eDirection.Left);
            inputHandler.KeyEvent[ConsoleKey.V] = () => tetromino.Rotate(eDirection.Right);

            inputHandler.KeyEvent[ConsoleKey.N] = () => { tracker.LockPiece(); tetromino.NewPiece();  };
            inputHandler.KeyEvent[ConsoleKey.Q] = () => isDone = true;

            inputHandler.KeyEvent[ConsoleKey.R] = () => {
                Console.Clear();
                Display();
            };
        }

        static void StartGame()
        {
            spwanPoint = new Point(Tracker.BOUNDS_DIM.X / 2 - 1, 0);
            tetromino = new Tetromino(spwanPoint);
            tracker = new Tracker(tetromino);
            view = new ConsoleView();
            frame = new Frame();
            inputHandler = new InputHandler();

            InitializeInputHandler();

            /* Subscribe to Events */
            tracker.GridUpdate += Display; // Displays the grid whenever the grid is updated
            frame.FrameAction += () => tetromino.Move(eDirection.Down); // move the tetromino down each frame
            frame.FrameAction += (tracker as Tracker).NextFrame; // advance frame timers
            inputHandler.AnyKeyEvent += (tracker as Tracker).ResetStationaryTimer;

            while (!isDone)
            {
                inputHandler.HandleInput();
                frame.nextFrame();
            }

            Console.WriteLine("Done.");
            Console.Read();
        }

        static void Main(string[] args)
        {
            //StartGame();
            ConsoleDynamicLayout layout = new ConsoleDynamicLayout();

            DynamicComponent componentTest = new DynamicComponent();
            componentTest.Grid = new ComponentContent[,] { { new ComponentContent('a', eTextColor.Green) } , { new ComponentContent('b', eTextColor.Red) } };
            layout.AddComponent(componentTest);

            DynamicComponent componentTest2 = new DynamicComponent(1, new IntPoint(1, 0));
            componentTest2.Grid = new ComponentContent[,] { { new ComponentContent('a', eTextColor.Green) }, { new ComponentContent('b', eTextColor.Red) } };
            layout.AddComponent(componentTest2);

            layout.DisplayAll();
        }
    }
}
