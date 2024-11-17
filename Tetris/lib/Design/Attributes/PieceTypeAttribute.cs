namespace Tetris.Lib.Design.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class PieceTypeAttribute : Attribute
    {
        public string Description { get; private set; }
        public bool IsStandardPiece { get; private set; }

        public PieceTypeAttribute(string description, bool isStandardPiece = false)
        {
            Description = description;
            IsStandardPiece = isStandardPiece;
        }
    }
}
