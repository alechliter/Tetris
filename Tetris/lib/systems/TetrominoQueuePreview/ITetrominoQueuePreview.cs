using Lechliter.Tetris.Lib.Objects;

namespace Lechliter.Tetris.Lib.Systems
{
    public interface ITetrominoQueuePreview<TPieceType>
        where TPieceType : System.Enum
    {
        IReadOnlyList<IPreview<TPieceType>> Previews { get; }
    }
}
