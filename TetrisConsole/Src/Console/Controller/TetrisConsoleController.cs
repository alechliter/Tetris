using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Objects;
using Lechliter.Tetris.Lib.Systems;
using Lechliter.Tetris.Lib.Types;
using System;
using System.Collections.Generic;
using Tetris.lib.Tetris;

namespace Lechliter.Tetris.TetrisConsole
{
    public class TetrisConsoleController : ITetrisConsoleController
    {
        private bool isDone = false;
        private bool isDev = false;

        private readonly IFrame Frame;
        private readonly IInputHandler<ConsoleKey> InputHandler;
        private readonly ITetrisConsoleLayout<DynamicComponent, List<DynamicComponent>, IntPoint> Layout;
        private readonly ITracker<ePieceType, eDirection, eMoveType> Tracker;

        public TetrisConsoleController()
        {
            Frame = TetrisApp.IoC.Get<IFrame>();
            InputHandler = TetrisApp.IoC.Get<IInputHandler<ConsoleKey>>();
            Layout = TetrisApp.IoC.Get<ITetrisConsoleLayout<DynamicComponent, List<DynamicComponent>, IntPoint>>();
            Tracker = TetrisApp.IoC.Get<ITracker<ePieceType, eDirection, eMoveType>>();
        }

        public void Run(string[] args)
        {
            SetFlags(args);
            StartGame();
        }

        private void InitializeInputHandler()
        {
            InputHandler.AddKey(ConsoleKey.UpArrow, () => Tracker.DropPiece());
            InputHandler.AddKey(ConsoleKey.DownArrow, () => Tracker.MovePiece(eDirection.Down));
            InputHandler.AddKey(ConsoleKey.LeftArrow, () => Tracker.MovePiece(eDirection.Left));
            InputHandler.AddKey(ConsoleKey.RightArrow, () => Tracker.MovePiece(eDirection.Right));

            InputHandler.AddKey(ConsoleKey.S, () => Tracker.RotatePiece(eDirection.Left));
            InputHandler.AddKey(ConsoleKey.D, () => Tracker.RotatePiece(eDirection.Right));

            InputHandler.AddKey(ConsoleKey.N, () => { Tracker.LockPiece(); Tracker.LoadNextPiece(); });
            InputHandler.AddKey(ConsoleKey.Q, () => isDone = true);

            InputHandler.AddKey(ConsoleKey.Spacebar, () => Tracker.HoldPiece());
        }

        private void StartGame()
        {
            InitializeInputHandler();
            InitializeEventListeners();
            AddComponents();

            while (!isDone)
            {
                try
                {
                    InputHandler.HandleInput();
                    Frame.NextFrame();
                }
                catch (Exception e)
                {
                    ErrorMessageHandler.DisplayMessage(e.Message);
                }
            }

            System.Console.WriteLine("Done.");
            System.Console.Read();
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

        /// <summary>
        /// Set Up Cross-Component Listeners
        /// </summary>
        private void InitializeEventListeners()
        {
            Tracker.GameOver += () => isDone = true;
        }

        /// <summary>
        /// Add Components to the View Layout
        /// </summary>
        private void AddComponents()
        {
            Layout.AddComponents(isDev);
        }
    }
}
