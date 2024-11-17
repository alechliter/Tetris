namespace Lechliter.Tetris.Lib.Effects
{
    public interface ISoundEffect
    {
        void PlayCollision();

        void PlaySingleLine();

        void PlayMultiLine(int numLines);

        void PlayTetrisClear();

        void PlayBackground();
    }
}
