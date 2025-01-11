using Lechliter.Tetris.Lib.Definitions;
using Lechliter.Tetris.Lib.Exceptions;
using System.Collections.Concurrent;
using Tetris.lib.Design.Helpers;

namespace Lechliter.Tetris.Lib.Systems
{
    public class TetrominoQueue : ITetrominoQueue<ePieceType>
    {
        public IEnumerable<ePieceType> Pieces { get { return Queue.ToList(); } }

        public event Action<IEnumerable<ePieceType>>? Updated;

        private int TetrominoQueueSize
        {
            get { return ConfigurationHelper.GetInt("TetrominoQueueSize", DEFAULT_QUEUE_SIZE); }
        }

        private readonly ISet<ePieceType> AvailablePieces;

        private readonly Random Random;

        private readonly ConcurrentQueue<ePieceType> Queue;

        private static readonly IEnumerable<ePieceType> StandardPieces;

        static TetrominoQueue()
        {
            StandardPieces = PieceTypeExtensions.StandardPieces();
        }

        public TetrominoQueue()
        {
            Random = new Random(DateTime.Now.GetHashCode());
            Queue = new ConcurrentQueue<ePieceType>();
            AvailablePieces = new HashSet<ePieceType>(StandardPieces);

            FillQueue();
        }

        public ePieceType Next()
        {
            if (Queue.TryDequeue(out ePieceType pieceType))
            {
                AddToQueue();
                AvailablePieces.Add(pieceType);
                return pieceType;
            }
            else
            {
                throw new TetrisLibException("An unexpected error occurred trying to retrieve the next piece.");
            }
        }

        private void FillQueue()
        {
            for (int i = 0; i < TetrominoQueueSize; i++)
            {
                AddToQueue(emitEvent: false);
            }
            Updated?.Invoke(Pieces);
        }

        private void AddToQueue(bool emitEvent = true)
        {
            ePieceType pieceType = RandType();

            AvailablePieces.Remove(pieceType);

            Queue.Enqueue(pieceType);
            Updated?.Invoke(Pieces);
        }

        private ePieceType RandType()
        {
            int randomType = Random.Next(AvailablePieces.Count());
            ePieceType newType = AvailablePieces.ElementAtOrDefault(randomType);

            if (newType == ePieceType.NotSet)
            {
                throw new TetrisLibException("Tetromino: Invalid Tetromino Type (RandType)");
            }

            return newType;
        }

        #region Constants

        const int DEFAULT_QUEUE_SIZE = 1;

        #endregion
    }
}
