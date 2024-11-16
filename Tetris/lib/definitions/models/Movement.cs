using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Objects;

namespace Lechliter.Tetris.Lib.Models

{
    public struct Movement
    {
        public eMoveType moveType;
        public eDirection direction;
        public int num_times;

        public Movement(eMoveType moveType = eMoveType.NotSet, eDirection direction = eDirection.NotSet, int num_times = 1)
        {
            this.moveType = moveType;
            this.direction = direction;
            this.num_times = num_times;
        }

        public void ExecuteMove(ITetromino<ePieceType, eDirection, eMoveType> piece)
        {
            for (int i = 0; i < num_times; i++)
            {
                piece.Move(direction);
            }
        }
    }
}