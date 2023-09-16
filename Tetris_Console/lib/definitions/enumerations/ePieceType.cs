using System;
using System.Collections.Generic;
using System.Text;

namespace Lechliter.Tetris_Console
{
    /// <summary>
    /// Type of tetromino piece. 
    /// Pieces: I, O, T, J, L, S, Z, 
    /// Locked - Locked Piece,
    /// Empty - Empty space
    /// NotSet - Not Set
    /// </summary>
    public enum ePieceType
    {
        NotSet = -1, Empty, Locked, I, O, T, J, L, S, Z
    }
}
