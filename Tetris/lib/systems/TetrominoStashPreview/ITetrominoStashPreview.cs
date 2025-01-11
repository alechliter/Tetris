using Lechliter.Tetris.Lib.Objects;

namespace Lechliter.Tetris.Lib.Systems
{
    public interface ITetrominoStashPreview<TPieceType>
        where TPieceType : System.Enum
    {
        IPreview<TPieceType> Preview { get; }
    }
}
