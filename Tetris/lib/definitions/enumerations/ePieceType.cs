using System.Reflection;
using Tetris.Lib.Design.Attributes;

namespace Lechliter.Tetris.Lib.Definitions
{
    /// <summary>
    /// Type of tetromino piece. 
    /// </summary>
    public enum ePieceType
    {
        [PieceType("Not Set")]
        NotSet = 0,

        [PieceType("Empty Space")]
        Empty = 1,

        [PieceType("Locked Piece")]
        Locked = 2,

        [PieceType(description: "I Piece", isStandardPiece: true)]
        I = 3,

        [PieceType(description: "O Piece", isStandardPiece: true)]
        O = 4,

        [PieceType(description: "T Piece", isStandardPiece: true)]
        T = 5,

        [PieceType(description: "J Piece", isStandardPiece: true)]
        J = 6,

        [PieceType(description: "L Piece", isStandardPiece: true)]
        L = 7,

        [PieceType(description: "S Piece", isStandardPiece: true)]
        S = 8,

        [PieceType(description: "Z Piece", isStandardPiece: true)]
        Z = 9,
    }

    public static class PieceTypeExtensions
    {
        public static IEnumerable<ePieceType> StandardPieces()
        {
            return Enum.GetValues<ePieceType>().Where((piece) => piece.IsStandardPiece());
        }

        public static bool IsStandardPiece(this ePieceType pieceType)
        {
            Type type = pieceType.GetType();
            MemberInfo? memberInfo = type.GetMember(pieceType.ToString()).FirstOrDefault();
            PieceTypeAttribute? attribute = memberInfo?.GetCustomAttribute<PieceTypeAttribute>();
            return attribute != null ? attribute.IsStandardPiece : false;
        }
    }
}
