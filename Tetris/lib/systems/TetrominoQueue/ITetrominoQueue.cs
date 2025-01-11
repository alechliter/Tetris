using Lechliter.Tetris.Lib.Definitions;

namespace Lechliter.Tetris.Lib.Systems
{
    public interface ITetrominoQueue<TPieceType>
        where TPieceType : System.Enum
    {
        IEnumerable<ePieceType> Pieces { get; }

        event Action<IEnumerable<ePieceType>> Updated;

        TPieceType Next();
    }
}
