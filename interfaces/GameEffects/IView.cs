using System.Collections.Generic;

namespace Lechliter.Tetris_Console
{
    interface IView <TColor, TPieceType> where TColor : struct, System.Enum
                                                     where TPieceType : System.Enum 

    {
        TColor Color { get; }
        void Display(TPieceType[,] blocks);

    }
}
