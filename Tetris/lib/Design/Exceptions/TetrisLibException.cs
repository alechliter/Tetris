namespace Lechliter.Tetris.Lib.Exceptions
{
    public class TetrisLibException : Exception
    {
        public bool IsSoftError { get; private set; }

        public TetrisLibException(string message, bool isSoftError = false) : base(message)
        {
            IsSoftError = isSoftError;
        }
    }
}
