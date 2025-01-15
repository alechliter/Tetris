using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Objects;
using Lechliter.Tetris.Lib.Systems;
using TetrisIoC.lib.Modules;

namespace Tetris.lib.Modules
{
    public class TetrisStandardModule : TetrisBaseModule
    {
        public override void Load()
        {
            Bind<IGrid<ePieceType, eDirection, eMoveType>>().To<Grid>().InSingletonScope();
            Bind<IFrame>().To<Frame>().InSingletonScope();
            Bind<ICollisionDetector<ePieceType, eDirection, eMoveType>>().To<CollisionDetector>().InSingletonScope();
            Bind<ITetrominoQueue<ePieceType>>().To<TetrominoQueue>().InSingletonScope();
            Bind<ITetrominoStash<ePieceType>>().To<TetrominoStash>().InSingletonScope();
            Bind<ITetrominoStashPreview<ePieceType>>().To<TetrominoStashPreview>().InSingletonScope();
            Bind<ITetrominoQueuePreview<ePieceType>>().To<TetrominoQueuePreview>().InSingletonScope();
            Bind<ITracker<ePieceType, eDirection, eMoveType>>().To<Tracker>().InSingletonScope();
            Bind<IScore>().To<ScoreBoard>().InSingletonScope();
        }
    }
}
