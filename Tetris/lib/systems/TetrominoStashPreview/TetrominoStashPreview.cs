using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Objects;

namespace Lechliter.Tetris.Lib.Systems
{
    public class TetrominoStashPreview : ITetrominoStashPreview<ePieceType>
    {
        public IPreview<ePieceType> Preview { get; private set; }

        private readonly ITetrominoStash<ePieceType> _TetrominoStash;

        public TetrominoStashPreview(ITetrominoStash<ePieceType> tetrominoStash)
        {
            _TetrominoStash = tetrominoStash;
            Preview = new Preview();

            _TetrominoStash.Updated += OnStashUpdate;
        }

        private void OnStashUpdate(ePieceType stashedPiece)
        {
            Preview.PieceType = stashedPiece;
        }
    }
}
