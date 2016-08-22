using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess3
{


    public enum huAi // human or computer player
    { human, ai }

    public enum piece // this enumerator gives an  integer value for each piece
    {
        pawn, enPassantePawn, rook, knight, bishop, queen, king
    }

    public enum handicap // not yet implemented the handicap is to allow players of different levels to play
    {
        none, theMove, pawnAgainstTheMove, pawnAndMove, pawnAndTwo, theExchange, theKnight, theKnightAndMove, theRook, theRookAndMove, twoMinorPieces, theQueenAgainstTheRook, theQueen
    }
    public enum wB // 0 = white 1= black
    {
        white, black
    }




    
}
