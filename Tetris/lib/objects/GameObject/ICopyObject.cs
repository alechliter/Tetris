using Lechliter.Tetris.Lib.Objects;

namespace Tetris.Lib.Objects.GameObject
{
    public interface ICopyObject<TObject> where TObject : IGameObject
    {
        TObject Copy();
    }
}
