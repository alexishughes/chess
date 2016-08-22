using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Chess3
{
    // The Constants class in this program is mostly a collection of look-up tables to make the program run faster. 
    // So when the program wants to find all the squares a knight can go to it looks up the UInt64 knightMove[index] (where index is the
    // number of a square where the knight is presently and ANDs that with the unoccupied squares on the board (for moves) or the opponent's squares
    // (for takes)

    // the class is static. this means that you cannot create an object of type Constants, rather each property is available straight from the class.

    // the class has a static constructor that is run just once, sometime before the class is used.

    // to access the properties you type (for instance) Constants.knightMove[32]
    public static class Constants
    {
        // Here are the constants that will be stored.
        public static byte[,] rot = new byte[4, 64]; // translates an address in the unrotated bitboard to one of the rotated bitboards (for info see Hyatt's paper)
        public static byte[,] irot = new byte[4, 64]; // the inverse transformation of rot - translates an address back to unrotated bitboard.
        public static byte[] diagLength = new byte[15] { 1, 2, 3, 4, 5, 6, 7, 8, 7, 6, 5, 4, 3, 2, 1 }; // the lengths of the 15 diagonals accross the board
        public static byte[] diagShift = new byte[15] { 0, 1, 3, 6, 10, 15, 21, 28, 36, 43, 49, 54, 58, 61, 63 }; // how many squares go before each diagonal
        public static byte[] diagMask = new byte[15]; // an 8bit number consisting of n 1s leading from the right where n is diagLength
        public static byte[] whichDiag = new byte[64]; // a look-up table. In a rotated bitboard, this constant returns the number (from 0 to 14) of the diagonal that contains the i'th square
        public static byte[] diagPos = new byte[64]; // a lookup table which returns the position (from 0 to 7) - along its own diagonal of the i'th square
        public static byte[,] lineAvail = new byte[8, 256]; // lineAvail[pos,byte] - given an 8 (or less) square line (or diagonal): represent the occupied squares with 1s in the byte
                                                            // pos is the index of the piece within the line. lineAvail[pos, byteLineOcc] tells you which of the 8 squares the piece can move to.
        public static byte[,] lineTake = new byte[8, 256];  // lineTake[pos, byteLineOcc] & byteLineOppo tells you which pieces you can take of your opponents.
        public static UInt64[][, ,] lineInsert = new UInt64[][, ,] { new UInt64[4, 8, 256], new UInt64[4, 8, 256], new UInt64[4, 15, 256], new UInt64[4, 15, 256] };
                                                            // not yet implemented, this massive 384kB array allows an 8bit line or diagonal to be inserted into the 64bit UInt
        public static UInt64[] knightMove = new UInt64[64]; // the squares a knight can move to on the board
        public static UInt64[] kingMove = new UInt64[64];   // the squares a king can move to

        static Constants()
        {



            // sets up whichDiag and diagPos
            byte index = 0;
            for (byte diag = 0; diag < 15; diag++)
            {
                for (byte i = 0; i < diagLength[diag]; i++)
                {
                    whichDiag[index] = diag;
                    diagPos[index] = i;
                    index++;
                }
            }

            // rot[0,] just maps a square onto the same square.
            for (byte i = 0; i < 64; i++)
            {
                rot[0, i] = i;
                irot[0, i] = i;

            }

            // rot[1,] maps the square onto a board rotated by 90degrees
            // irot[1,] reverses it
            for (byte i = 0; i < 64; i++)
            {
                rot[1, i] = (byte)((i % 8) * 8 + 7 - (i / 8));
            }

            for (byte i = 0; i < 64; i++)
            {
                irot[1, rot[1, i]] = i;
            }


            // sets up the 45deg rotation rot[2,]

            index = 0;




            for (byte diag = 0; diag < 8; diag++)
            {
                for (byte diagPos = 0; diagPos < diagLength[diag]; diagPos++)
                {
                    irot[2, index] = (byte)(((diag - diagPos) * 8) + diagPos);
                    index++;
                }
            }

            for (byte diag = 8; diag < 15; diag++)
            {
                for (byte diagPos = 0; diagPos < diagLength[diag]; diagPos++)
                {
                    irot[2, index] = (byte)(((49 + diagPos + diag) - 8 * diagPos));
                    index++;
                }
            }


            for (byte diag = 0; diag < 15; diag++)
            {
                diagMask[diag] = (byte)((1 << diagLength[diag]) - 1);
            }



            for (byte i = 0; i < 64; i++)
            {
                rot[2, (irot[2, i])] = i;
            }

            for (byte i = 0; i < 64; i++)
            {
                rot[3, i] = rot[2, (irot[1, i])];
            }

            for (byte i = 0; i < 64; i++)
            {
                irot[3, rot[3, i]] = i;
            }


            // setting up lineAvail amd lineTake :
            // given a row of 8 squares (for queen, rook and bishop moves)
            // if the occupied squares were 01000101 and the position was 2 (the third square)
            // lineAvail should come out as 00111000 and
            // lineTake  should come out as 01000100
            // to find out which pieces you can actually take you have to & with the bitboard of the oppents pieces.
            //                              01000001
            // to get                       01000000

            // create 256 look-ups for each position along the row.
            for (int pos = 0; pos < 8; pos++)
            {
                // to convert a byte into a bitarray it must be placed in a byte array.
                byte[] bytLine = new byte[1];
                for (int l = 0; l < 256; l++)
                {
                    bytLine[0] = (byte)l;
                    BitArray bitLine = new BitArray(bytLine);
                    lineAvail[pos, l] = 0;
                    lineTake[pos, l] = 0;
                    for (int i = pos + 1; i < 8; i++)
                    {
                        if (!bitLine[i]) { lineAvail[pos, l] |= (byte)(1 << i); } else { lineTake[pos, l] |= (byte)(1 << i); break; }
                        // start at the square to the left (+1) of pos. if it is empty put a 1 in the corresponding lineAvail square and leave lineTake as zero.
                        // as soon as we hit a piece (the else clause) put a 1 in lineTake and stop looking.


                    }
                    for (int i = pos - 1; i >= 0; i--)
                    {
                        if (!bitLine[i]) { lineAvail[pos, l] |= (byte)(1 << i); } else { lineTake[pos, l] |= (byte)(1 << i); break; }
                        // does the same looking to the right of the piece.
                    }

                }
            }

            // to set up knightMove the eight moves are listed (it is the same addition or subtraction no matter which square you are on)
            // if the square is far enough from the edge of the board add the required index to the UInt64.

            for (byte i = 0; i < 64; i++)
            {
                knightMove[i] = 0;
                if (((i % 8) > 1) && ((i / 8) < 7)) { knightMove[i] |= (UInt64)1 << (i + 6); }
                if (((i % 8) > 0) && ((i / 8) < 6)) { knightMove[i] |= (UInt64)1 << (i + 15); }
                if (((i % 8) < 7) && ((i / 8) < 6)) { knightMove[i] |= (UInt64)1 << (i + 17); }
                if (((i % 8) < 6) && ((i / 8) < 7)) { knightMove[i] |= (UInt64)1 << (i + 10); }
                if (((i % 8) < 6) && ((i / 8) > 0)) { knightMove[i] |= (UInt64)1 << (i - 6); }
                if (((i % 8) < 7) && ((i / 8) > 1)) { knightMove[i] |= (UInt64)1 << (i - 15); }
                if (((i % 8) > 0) && ((i / 8) > 1)) { knightMove[i] |= (UInt64)1 << (i - 17); }
                if (((i % 8) > 1) && ((i / 8) > 0)) { knightMove[i] |= (UInt64)1 << (i - 10); }
            }

            // kingMoves are set up the same

            for (byte i = 0; i < 64; i++)
            {
                kingMove[i] = 0;
                if (i % 8 > 0) { kingMove[i] |= (UInt64)1 << i - 1; }
                if ((i % 8 > 0) && (i / 8 > 0)) { kingMove[i] |= (UInt64)1 << i - 9; }
                if ((i / 8 > 0)) { kingMove[i] |= (UInt64)1 << i - 8; }
                if ((i % 8 < 7) && (i / 8 > 0)) { kingMove[i] |= (UInt64)1 << i - 7; }
                if ((i % 8 < 7)) { kingMove[i] |= (UInt64)1 << i + 1; }
                if ((i % 8 < 7) && (i / 8 < 7)) { kingMove[i] |= (UInt64)1 << i + 9; }
                if ((i / 8 < 7)) { kingMove[i] |= (UInt64)1 << i + 8; }
                if ((i % 8 > 0) && (i / 8 < 7)) { kingMove[i] |= (UInt64)1 << i + 7; }
            }
        }
    }
}
