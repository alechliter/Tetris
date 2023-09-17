using Lechliter.Tetris_Console.lib.objects.Grid;
using Lechliter.Tetris_Console.src.tetris_console.views;
using LechliterTetris_Console.Interfaces;
using System;
using System.Collections.Generic;

namespace Lechliter.Tetris_Console
{
    public class TetrisConsoleController : ITetrisConsoleController
    {
        private IGrid<IntDimensions, Point> grid;
        private ICollisionDetector<ePieceType, eDirection, eMoveType> collisionDetector;
        private ITracker<ePieceType, eDirection, eMoveType> tracker;
        private IView<eTextColor, ePieceType> view;
        private IFrame frame;
        private IInputHandler<ConsoleKey, Action> inputHandler;
        private IScore scoreBoard;
        private ISoundEffect soundEffect;
        private ITetrisConsoleLayout<DynamicComponent, List<DynamicComponent>, IntPoint> layout;
        private bool isDone = false;
        private bool isDev = false;

        public void Run(string[] args)
        {
            SetFlags(args);
            StartGame();
        }

        private void InitializeInputHandler()
        {
            inputHandler.KeyEvent[ConsoleKey.UpArrow] = () => tracker.CurrentPiece.Drop(tracker);
            inputHandler.KeyEvent[ConsoleKey.DownArrow] = () => tracker.CurrentPiece.Move(eDirection.Down);
            inputHandler.KeyEvent[ConsoleKey.LeftArrow] = () => tracker.CurrentPiece.Move(eDirection.Left);
            inputHandler.KeyEvent[ConsoleKey.RightArrow] = () => tracker.CurrentPiece.Move(eDirection.Right);

            inputHandler.KeyEvent[ConsoleKey.S] = () => tracker.CurrentPiece.Rotate(eDirection.Left);
            inputHandler.KeyEvent[ConsoleKey.D] = () => tracker.CurrentPiece.Rotate(eDirection.Right);

            inputHandler.KeyEvent[ConsoleKey.N] = () => { tracker.LockPiece(); tracker.CurrentPiece.NewPiece(); };
            inputHandler.KeyEvent[ConsoleKey.Q] = () => isDone = true;

            inputHandler.KeyEvent[ConsoleKey.R] = () => { Console.Clear(); Display(); };
            inputHandler.KeyEvent[ConsoleKey.H] = () => Console.CursorVisible = false;
            inputHandler.KeyEvent[ConsoleKey.Spacebar] = () => tracker.HoldPiece();
        }

        private void InitializeEventListeners()
        {
            tracker.GameOver += () => isDone = true;
        }

        private void StartGame()
        {
            grid = new Grid();
            frame = new Frame();
            collisionDetector = new CollisionDetector(grid);
            inputHandler = new InputHandler();
            scoreBoard = new ScoreBoard();
            tracker = new Tracker(grid, collisionDetector, frame, inputHandler, scoreBoard);
            soundEffect = new SimpleSoundEffect(tracker);
            layout = new TetrisConsoleLayout(collisionDetector, tracker, scoreBoard, frame);
            view = new ConsoleView(layout);

            InitializeInputHandler();

            /* Set Up Cross-Component Listeners*/
            InitializeEventListeners();

            /* Add Components to the View Layout */
            AddComponents();

            while (!isDone)
            {
                inputHandler.HandleInput();
                frame.nextFrame();
            }

            Console.WriteLine("Done.");
            Console.Read();
        }

        private void SetFlags(string[] args)
        {
            foreach (string argument in args)
            {
                switch (argument)
                {
                    case "-d":
                        isDev = true;
                        break;
                    default:
                        break;
                }
            }
        }

        private void Display()
        {
            view?.Display();
        }

        private void AddComponents()
        {
            layout.AddComponents(isDev);
        }
    }
}
