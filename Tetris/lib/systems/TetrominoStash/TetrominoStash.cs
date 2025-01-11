using Lechliter.Tetris.Lib.Definitions;

namespace Lechliter.Tetris.Lib.Systems
{
    public class TetrominoStash : ITetrominoStash<ePieceType>
    {
        public ePieceType Piece
        {
            get
            {
                return _Piece;
            }
            private set
            {
                _Piece = value;
                Updated?.Invoke(value);
            }
        }

        public event Action<ePieceType>? Updated;

        private ePieceType _Piece = ePieceType.NotSet;

        public TetrominoStash()
        {
        }

        public ePieceType Hold(ePieceType pieceType)
        {
            ePieceType heldPiece = Piece;

            Piece = pieceType;

            return heldPiece;
        }
    }
}
