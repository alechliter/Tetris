using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Effects;
using Lechliter.Tetris.Lib.Objects;
using Lechliter.Tetris.Lib.Systems;
using Lechliter.Tetris.Lib.Types;
using Lechliter.Tetris.TetrisConsole.Enumerations;
using System;
using System.Collections.Generic;

namespace Lechliter.Tetris.TetrisConsole
{
    public class TetrisConsoleController : ITetrisConsoleController
    {
        private readonly IGrid<ePieceType, eDirection, eMoveType> grid;
        private readonly ICollisionDetector<ePieceType, eDirection, eMoveType> collisionDetector;
        private readonly ITetrominoQueue<ePieceType> tetrominoQueue;
        private readonly ITetrominoStash<ePieceType> tetrominoStash;
        private readonly ITetrominoStashPreview<ePieceType> tetrominoStashPreview;
        private readonly ITetrominoQueuePreview<ePieceType> tetrominoQueuePreview;
        private readonly ITracker<ePieceType, eDirection, eMoveType> tracker;
        private readonly IView<eTextColor, ePieceType> view;
        private readonly IFrame frame;
        private readonly IInputHandler<ConsoleKey> inputHandler;
        private readonly IScore scoreBoard;
        private readonly ISoundEffect soundEffect;
        private readonly ITetrisConsoleLayout<DynamicComponent, List<DynamicComponent>, IntPoint> layout;

        private bool isDone = false;
        private bool isDev = false;

        public TetrisConsoleController()
        {
            grid = new Grid();
            frame = new Frame();
            collisionDetector = new CollisionDetector(grid);
            tetrominoQueue = new TetrominoQueue();
            tetrominoStash = new TetrominoStash();
            tetrominoQueuePreview = new TetrominoQueuePreview(tetrominoQueue);
            tetrominoStashPreview = new TetrominoStashPreview(tetrominoStash);
            inputHandler = new ConsoleInputHandler();
            scoreBoard = new ScoreBoard();
            tracker = new Tracker(
                grid: grid,
                collisionDetector: collisionDetector,
                tetrominoQueue: tetrominoQueue,
                tetrominoStash: tetrominoStash,
                frame: frame,
                inputHandler: inputHandler,
                score: scoreBoard);
            soundEffect = new SimpleSoundEffect(tracker);
            layout = new TetrisConsoleLayout(
                collisionDetector: collisionDetector,
                tracker: tracker,
                tetrominoQueuePreview: tetrominoQueuePreview,
                tetrominoStashPreview: tetrominoStashPreview,
                grid: grid,
                scoreBoard: scoreBoard,
                frame: frame);
            view = new ConsoleView(layout);
        }

        public void Run(string[] args)
        {
            SetFlags(args);
            StartGame();
        }

        private void InitializeInputHandler()
        {
            inputHandler.AddKey(ConsoleKey.UpArrow, () => tracker.DropPiece());
            inputHandler.AddKey(ConsoleKey.DownArrow, () => tracker.MovePiece(eDirection.Down));
            inputHandler.AddKey(ConsoleKey.LeftArrow, () => tracker.MovePiece(eDirection.Left));
            inputHandler.AddKey(ConsoleKey.RightArrow, () => tracker.MovePiece(eDirection.Right));

            inputHandler.AddKey(ConsoleKey.S, () => tracker.RotatePiece(eDirection.Left));
            inputHandler.AddKey(ConsoleKey.D, () => tracker.RotatePiece(eDirection.Right));

            inputHandler.AddKey(ConsoleKey.N, () => { tracker.LockPiece(); tracker.LoadNextPiece(); });
            inputHandler.AddKey(ConsoleKey.Q, () => isDone = true);

            inputHandler.AddKey(ConsoleKey.R, () => { Console.Clear(); Display(); });
            inputHandler.AddKey(ConsoleKey.H, () => Console.CursorVisible = false);
            inputHandler.AddKey(ConsoleKey.Spacebar, () => tracker.HoldPiece());
        }

        private void InitializeEventListeners()
        {
            tracker.GameOver += () => isDone = true;
        }

        private void StartGame()
        {
            InitializeInputHandler();

            /* Set Up Cross-Component Listeners*/
            InitializeEventListeners();

            /* Add Components to the View Layout */
            AddComponents();

            while (!isDone)
            {
                try
                {
                    inputHandler.HandleInput();
                    frame.NextFrame();
                }
                catch (Exception e)
                {
                    ErrorMessageHandler.DisplayMessage(e.Message);
                }
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
