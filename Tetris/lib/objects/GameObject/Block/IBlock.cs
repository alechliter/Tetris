using Lechliter.Tetris.Lib.Types;
using Tetris.Lib.Definitions.Types;
using Tetris.Lib.Objects.GameObject;

namespace Lechliter.Tetris.Lib.Objects
{
    public interface IBlock : IGameObject, ICopyObject<IBlock>
    {
        FloatDimensions Dim { get; }

        void MoveTo(Point newPoint);
        void MoveBy(Point vector);

        public bool IsSamePosition(IBlock block);
    }
}
