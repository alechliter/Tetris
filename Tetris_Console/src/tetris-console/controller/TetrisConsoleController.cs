using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lechliter.Tetris_Console
{
    public class TetrisConsoleController : ITetrisConsoleController
    {
        private Point spawnPoint;
        private ITetromino<ePieceType, eDirection, eMoveType> tetromino;
        private ITracker<ePieceType, eDirection, eMoveType> tracker;
        private IView<eTextColor, ePieceType> view;
        private IFrame frame;
        private IInputHandler<ConsoleKey, Action> inputHandler;
        private IScore scoreBoard;
        private bool isDone = false;
        private bool isDev = false;

        public void Run(string[] args)
        {
            SetFlags(args);
            StartGame();
        }

        private void InitializeInputHandler()
        {
            inputHandler.KeyEvent[ConsoleKey.UpArrow] = () => (tetromino as Tetromino).Drop(tracker);//tetromino.Move(Direction.Up);
            inputHandler.KeyEvent[ConsoleKey.DownArrow] = () => tetromino.Move(eDirection.Down);
            inputHandler.KeyEvent[ConsoleKey.LeftArrow] = () => tetromino.Move(eDirection.Left);
            inputHandler.KeyEvent[ConsoleKey.RightArrow] = () => tetromino.Move(eDirection.Right);

            inputHandler.KeyEvent[ConsoleKey.S] = () => tetromino.Rotate(eDirection.Left);
            inputHandler.KeyEvent[ConsoleKey.D] = () => tetromino.Rotate(eDirection.Right);

            inputHandler.KeyEvent[ConsoleKey.N] = () => { tracker.LockPiece(); tetromino.NewPiece(); };
            inputHandler.KeyEvent[ConsoleKey.Q] = () => isDone = true;

            inputHandler.KeyEvent[ConsoleKey.R] = () => { Console.Clear(); Display(); };
            inputHandler.KeyEvent[ConsoleKey.H] = () => Console.CursorVisible = false;
            inputHandler.KeyEvent[ConsoleKey.Spacebar] = () => (tracker as Tracker).HoldPiece();
        }

        private void InitializeEventListeners()
        {
            frame.FrameAction += () => tetromino.Move(eDirection.Down); // move the tetromino down each frame
            frame.FrameAction += (tracker as Tracker).NextFrame;        // advance frame timers
            inputHandler.AnyKeyEvent += (tracker as Tracker).ResetStationaryTimer;
            tracker.GameOver += () => isDone = true;
            tracker.LinesCleared += scoreBoard.Increase;
            scoreBoard.NextLevel += (level) => frame.SpeedUp(level);
        }

        private void StartGame()
        {
            spawnPoint = new Point(Tracker.BOUNDS_DIM.X / 2 - 1, 0);
            tetromino = new Tetromino(spawnPoint);
            tracker = new Tracker(tetromino);
            view = new ConsoleView();
            frame = new Frame();
            inputHandler = new InputHandler();
            scoreBoard = new ScoreBoard();

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
            // Add Grid to Layout
            DynamicComponent gridComponent = new DynamicComponent(1, new IntPoint(0, 0), 2);
            gridComponent.Grid = ConsoleView.ConvertPieceGridToContentGrid(tracker.AllPieces);
            tracker.GridUpdate += () => gridComponent.OnUpdate(ConsoleView.ConvertPieceGridToContentGrid(tracker.AllPieces));
            ConsoleView.Layout.AddComponent(gridComponent);

            // ScoreBoard
            IntPoint scorePosition = gridComponent.Origin + new IntPoint(gridComponent.Dimensions.X * 2, 0);
            ScoreBoardView scoreBoardView = new ScoreBoardView(2, scorePosition);
            scoreBoard.UpdatedScore += (score) => scoreBoardView.OnUpdate(score: score);
            scoreBoard.NextLevel += (level) => scoreBoardView.OnUpdate(level: level);
            ConsoleView.Layout.AddComponent(scoreBoardView.ScoreBoard);

            // Next Tetromino Preview
            IntPoint nextPosition = gridComponent.Origin + new IntPoint(gridComponent.Dimensions.X * 2 + 2, 3 + scoreBoardView.Dim.Y);
            PreviewPieceView nextPiece = new PreviewPieceView(2, nextPosition, (tracker as Tracker).NextPiece as Preview);
            ConsoleView.Layout.AddComponent(nextPiece.Component);

            // Held Tetromino Preview
            IntPoint heldPosition = nextPosition + new IntPoint(0, nextPiece.Dim.Y + 2);
            PreviewPieceView heldPiece = new PreviewPieceView(2, heldPosition, (tracker as Tracker).HeldPiece as Preview);
            ConsoleView.Layout.AddComponent(heldPiece.Component);


            // Timer Display
            if (isDev)
            {
                IntPoint timerPosition = heldPosition + new IntPoint(0, heldPiece.Dim.Y + 2);
                DynamicComponent timerComponent = new DynamicComponent(1, timerPosition);
                timerComponent.Grid = (tracker as Tracker).DisplayTimer();
                frame.FrameAction += () => timerComponent.OnUpdate((tracker as Tracker).DisplayTimer());
                ConsoleView.Layout.AddComponent(timerComponent);
            }

            // Error Messages
            IntPoint errorPosition = gridComponent.Origin + new IntPoint(gridComponent.Dimensions.X * 2, heldPosition.Y + heldPiece.Dim.Y + 2);
            DynamicComponent errorComponent = new DynamicComponent(0, errorPosition);
            ErrorMessageHandler.NewErrorMessage += (ComponentContent[,] content) => errorComponent.OnUpdate(content);
            ConsoleView.Layout.AddComponent(errorComponent);
        }
    }
}
