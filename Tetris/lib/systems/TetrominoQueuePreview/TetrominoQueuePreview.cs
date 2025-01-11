using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Objects;

namespace Lechliter.Tetris.Lib.Systems
{
    public class TetrominoQueuePreview : ITetrominoQueuePreview<ePieceType>
    {
        public IReadOnlyList<IPreview<ePieceType>> Previews { get; private set; }

        private readonly ITetrominoQueue<ePieceType> _TetrominoQueue;

        public TetrominoQueuePreview(ITetrominoQueue<ePieceType> tetrominoQueue)
        {
            _TetrominoQueue = tetrominoQueue;
            Previews = CreatePreviewCollection();

            _TetrominoQueue.Updated += OnQueueUpdate;
        }

        private void OnQueueUpdate(IEnumerable<ePieceType> queuedPieces)
        {
            int previewIndex = 0;
            foreach (ePieceType pieceType in queuedPieces)
            {
                IPreview<ePieceType>? preview = Previews.ElementAtOrDefault(previewIndex);
                if (preview == null)
                {
                    break;
                }
                preview.PieceType = pieceType;
                previewIndex++;
            }
        }

        private IReadOnlyList<IPreview<ePieceType>> CreatePreviewCollection()
        {
            List<IPreview<ePieceType>> previews = new List<IPreview<ePieceType>>();

            foreach (ePieceType pieceType in _TetrominoQueue.Pieces)
            {
                IPreview<ePieceType> preview = new Preview();
                preview.PieceType = pieceType;
                previews.Add(preview);
            }

            return previews;
        }
    }
}
