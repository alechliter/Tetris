using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Objects;

namespace Lechliter.Tetris.Lib.Models

{
    public struct Movement
    {
        public eMoveType MoveType;

        public eDirection Direction;

        public int Count;

        public Movement(eMoveType moveType = eMoveType.NotSet, eDirection direction = eDirection.NotSet, int count = 1)
        {
            this.MoveType = moveType;
            this.Direction = direction;
            this.Count = count;
        }

        public void ExecuteMove(ITetromino<ePieceType, eDirection, eMoveType> piece)
        {
            for (int i = 0; i < Count; i++)
            {
                piece.Move(Direction);
            }
        }
    }
}