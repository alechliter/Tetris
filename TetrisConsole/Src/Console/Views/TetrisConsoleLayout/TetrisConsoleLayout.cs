using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Objects;
using Lechliter.Tetris.Lib.Systems;
using Lechliter.Tetris.Lib.Types;
using Lechliter.Tetris.TetrisConsole.Enumerations;
using Lechliter.Tetris.TetrisConsole.Models;
using System;
using System.Collections.Generic;

namespace Lechliter.Tetris.TetrisConsole
{
    public class TetrisConsoleLayout : DynamicConsoleLayout, ITetrisConsoleLayout<DynamicComponent, List<DynamicComponent>, IntPoint>
    {
        private readonly ICollisionDetector<ePieceType, eDirection, eMoveType> _CollisionDetector;

        private readonly ITracker<ePieceType, eDirection, eMoveType> _Tracker;

        private readonly IGrid<ePieceType, eDirection, eMoveType> _Grid;

        private readonly IScore _ScoreBoard;

        private readonly IFrame _Frame;

        public TetrisConsoleLayout(
            ICollisionDetector<ePieceType, eDirection, eMoveType> collisionDetector,
            ITracker<ePieceType, eDirection, eMoveType> tracker,
            IGrid<ePieceType, eDirection, eMoveType> grid,
            IScore scoreBoard,
            IFrame frame)
        {
            _CollisionDetector = collisionDetector;
            _Tracker = tracker;
            _Grid = grid;
            _ScoreBoard = scoreBoard;
            _Frame = frame;
        }

        public void AddComponents(bool isDev)
        {
            // Add Grid to Layout
            DynamicComponent gridComponent = new DynamicComponent(1, new IntPoint(0, 0), 2);
            gridComponent.Grid = ConsoleView.ConvertPieceGridToContentGrid(_Grid.Pieces);
            _Grid.Update += () => gridComponent.OnUpdate(ConsoleView.ConvertPieceGridToContentGrid(_Grid.Pieces));
            AddComponent(gridComponent);

            // ScoreBoard
            IntPoint scorePosition = gridComponent.Origin + new IntPoint(gridComponent.Dimensions.X * 2, 0);
            ScoreBoardView scoreBoardView = new ScoreBoardView(2, scorePosition);
            _ScoreBoard.UpdatedScore += (score) => scoreBoardView.OnUpdate(score: score);
            _ScoreBoard.NextLevel += (level) => scoreBoardView.OnUpdate(level: level);
            AddComponent(scoreBoardView.ScoreBoard);

            // Next Tetromino Preview
            IntPoint nextPosition = gridComponent.Origin + new IntPoint(gridComponent.Dimensions.X * 2 + 2, 3 + scoreBoardView.Dim.Y);
            PreviewPieceView nextPiece = new PreviewPieceView(2, nextPosition, _Tracker.NextPiece);
            AddComponent(nextPiece.Component);

            // Held Tetromino Preview
            IntPoint heldPosition = nextPosition + new IntPoint(0, nextPiece.Dim.Y + 2);
            PreviewPieceView heldPiece = new PreviewPieceView(2, heldPosition, _Tracker.HeldPiece);
            AddComponent(heldPiece.Component);


            // Timer Display
            if (isDev)
            {
                IntPoint timerPosition = heldPosition + new IntPoint(0, heldPiece.Dim.Y + 2);
                DynamicComponent timerComponent = new DynamicComponent(1, timerPosition);
                timerComponent.Grid = DisplayTimer();
                _Frame.FrameAction += () => timerComponent.OnUpdate(DisplayTimer());
                AddComponent(timerComponent);
            }

            // Error Messages
            IntPoint errorPosition = gridComponent.Origin + new IntPoint(gridComponent.Dimensions.X * 2, heldPosition.Y + heldPiece.Dim.Y + 2);
            DynamicComponent errorComponent = new DynamicComponent(0, errorPosition);
            ErrorMessageHandler.NewErrorMessage += (ComponentContent[,] content) => errorComponent.OnUpdate(content);
            AddComponent(errorComponent);
        }

        private ComponentContent[,] DisplayTimer()
        {
            string fallingTimer = $"Falling Timer: {_CollisionDetector.LockTimerFalling.FramesRemaining}";
            string stationaryTimer = $"Stationary Timer: {_CollisionDetector.LockTimerStationary.FramesRemaining}";

            ComponentContent[,] timers = new ComponentContent[Math.Max(fallingTimer.Length, stationaryTimer.Length), 2];

            for (int i = 0; i < fallingTimer.Length; i++)
            {
                timers[i, 0] = new ComponentContent(fallingTimer[i], eTextColor.Yellow);
            }

            for (int i = 0; i < stationaryTimer.Length; i++)
            {
                timers[i, 1] = new ComponentContent(stationaryTimer[i], eTextColor.Yellow);
            }

            return timers;
        }

    }
}
