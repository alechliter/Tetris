using Lechliter.Tetris.Lib.Definitions;

namespace Lechliter.Tetris.Lib.Systems
{
    public interface ITetrominoStash<TPieceType>
        where TPieceType : System.Enum
    {
        ePieceType Piece { get; }

        event Action<ePieceType> Updated;

        TPieceType Hold(ePieceType pieceType);
    }
}
