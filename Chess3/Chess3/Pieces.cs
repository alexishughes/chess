using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Collections;

namespace Chess3
{

    public abstract class Piece
    {
        private int _index; // each piece knows the index of the square it is in (as each square knows which piece is in it)
        private piece _rank; // the piece enumerator
        private wB _colour; // 0 white, 1 black
        public int index { get { return _index; } set { _index = value; } }
        public piece rank { get { return _rank; } set { _rank = value; } }
        public wB colour { get { return _colour; } set { _colour = value; } }
        public int colRank { get { return (7 * (int)_colour + (int)_rank); } } // colrank is  aproperty derived from colour and rank and tells you which bitboard to look in for the piece.

        public Image drawMe() // returns an image of the piece. the sources are stored as an array of bitmaps
        {
            Image drawMe = new Image();
            drawMe.Source = imgPiece.bmiPiece[colRank];
            return drawMe;
        }

        public virtual UInt64 moves(Board thisBoard) // every piece will have a bitboard of squares it can move to this virtaul method (always overridden) simply returns the unoccupied squares on the board
        {
            UInt64 free = ~(thisBoard.occ[0]);
            return free;
        }

        public virtual UInt64 takes(Board thisBoard) // the virtual method for takes returns all the oppenent's squares.
        {
            UInt64 oppo = thisBoard.bw[0, ((int)this.colour + 1) % 2];
            return oppo;
        }

        
        // The attacked by UInt64s show any piece the squares that are attaking it.


        // first the lie attack board.
        public UInt64 LineAttackedBy(Board thisBoard)
        {

            UInt64 squares = 0;

            // starts with an empty square and works with the up an r90 rotaions of the bitboard.
            for (byte rotNum = 0; rotNum < 2; rotNum++)
            {
                int rotRow = Constants.rot[rotNum, index] / 8;
                int rotCol = Constants.rot[rotNum, index] % 8;

                byte lineOcc = BitConverter.GetBytes(thisBoard.occ[rotNum])[rotRow];
                byte[] lineTakes = new byte[1] { Constants.lineTake[rotCol, lineOcc] };
                BitArray bitLineTakes = new BitArray(lineTakes);
                for (byte i = 0; i < 8; i++)
                {
                    if (bitLineTakes[i])
                    {
                        squares |= (UInt64)1 << Constants.irot[rotNum, i + 8 * rotRow];
                    }
                }
            }
            // once all the take squares are found for horizontal and vertical stripes this is ANDed with the oppositon's queens and rooks to see if any of them can attack.

            byte oppo;
            if (colour == wB.white)
            { oppo = 1; }
            else
            { oppo = 0; }
            squares &= (thisBoard.bitBoard[0, (7 * oppo + (int)piece.rook)]) | (thisBoard.bitBoard[0, (7 * oppo + (int)piece.queen)]);

            return squares;
        }

        public UInt64 DiagAttackedBy(Board thisBoard)
        {

            UInt64 squares = 0;
            for (byte rotNum = 2; rotNum < 4; rotNum++) // goes through bitboard rotations 2,3
            {
                byte rotIndex = Constants.rot[rotNum, index]; // gets the new index on the rotated bitboard
                byte diag = Constants.whichDiag[rotIndex];  // gets the diagonal and position along it for the square on the particular rotated board
                byte diagPos = Constants.diagPos[rotIndex]; //
                byte diagShift = Constants.diagShift[diag];
                byte diagOcc = (byte)((thisBoard.occ[rotNum] >> diagShift) & 255); // shifts the board so that the 8 bits nearest to the diagonal are in the least significant places and then masks them with an 8bit bitmask.
                                                                                   // note there may be some pieces from the next line depending on how long the diagonal is but they will be masked off later.
                byte[] diagTakes = new byte[1] { (byte)(Constants.lineTake[diagPos, diagOcc] & Constants.diagMask[diag]) }; // gets the lineTake lookup and masks it with the appropirate number of 1 bits for the diag.
                BitArray bitDiagTakes = new BitArray(diagTakes); // formats into a bitArray because C# does not have bitscan forward (BSF)
                for (byte i = 0; i < 8; i++)
                {
                    if (bitDiagTakes[i])
                    {
                        squares |= (UInt64)1 << Constants.irot[rotNum, (i + diagShift)];
                    }
                }
            }
            // note: in the next issue I will create a 384k lookup of UInt64s for each diagonal and each board this will eliminiate the need for looping through the 8 bits and should make things faster.



            int oppo;
            if (colour == wB.white)
            { oppo = 1; }
            else
            { oppo = 0; }
            // finds the colour index of the opponent.
            
            // ands the resulant take squares with the ones that contain bishops and queens.
            squares &= (thisBoard.bitBoard[0, (7 * oppo + (int)piece.bishop)]) | (thisBoard.bitBoard[0, (7 * oppo + (int)piece.queen)]);

            return squares;
        }

        public UInt64 KnightAttackedBy(Board thisBoard)
        {
            // this method is much simpler just get the moves and AND them with the opposing Knights
            UInt64 squares = Constants.knightMove[index];
            int oppo;
            if (colour == wB.white)
            {oppo = 1;}
            else
            {oppo = 0;}
            squares &= thisBoard.bitBoard[0,7*oppo + (int)piece.knight];
         
            
            return squares;
        }

        public UInt64 PawnAttackedBy(Board thisBoard)
        {

            UInt64 squares = 0;
            if (colour == wB.white)
            {
                if (index % 8 > 0)
                { squares |= (UInt64)1 << index + 7; }
                if (index % 8 < 7)
                { squares |= (UInt64)1 << index + 9; }

                squares &= thisBoard.bitBoard[0, 8];

            }
            else
            {
                if (index % 8 > 0)
                { squares |= (UInt64)1 << index - 9; }
                if (index % 8 < 7)
                { squares |= (UInt64)1 << index - 7; }

                squares &= thisBoard.bitBoard[0, 1];

            }



            return squares;
        }


        public UInt64 KingAttackedBy(Board thisBoard)
        {

            
            UInt64 squares = Constants.kingMove[index];

            int oppo;
            if (colour == wB.white)
            {oppo = 1;}
            else
            {oppo = 0;}
            squares &= thisBoard.bitBoard[0,7*oppo + (int)piece.king];

            return squares;
         

            
        }


        //once all the attacks are listed they can ne ORed together to return all the squares that are attacking the piece.
        public UInt64 AttackedBy(Board thisBoard)
        {
            UInt64 squares = 0;
            squares |= KingAttackedBy(thisBoard);
            squares |= PawnAttackedBy(thisBoard);
            squares |= KnightAttackedBy(thisBoard);
            squares |= LineAttackedBy(thisBoard);
            squares |= DiagAttackedBy(thisBoard);
            return squares;
        }




    }




    public class Pawn : Piece
    {
        public Pawn(wB colour, int index) // the constructor for the Pawn class takes two arguments. the use of rank as a property is another duplication of effort. I could use typeof but the enumerator can be cast to an integer.
        {

            base.rank = piece.pawn;
            base.colour = colour;
            base.index = index;
        }

        // the moves and takes for the pawn class
        public override UInt64 moves(Board thisBoard)
        {
            UInt64 squares = 0;
            if (this.colour == wB.white)
            {
                squares |= (UInt64)1 << index + 8;
                if (index < 16)
                {
                    squares |= (UInt64)1 << index + 16;
                }
            }
            else
            {
                squares |= (UInt64)1 << index - 8;
                if (index > 47)
                {
                    squares |= (UInt64)1 << index - 16;
                }
            }

            UInt64 free = ~(thisBoard.occ[0]);

            return free & squares;
        }

        public override UInt64 takes(Board thisBoard)
        {
            UInt64 squares = 0;
            if (this.colour == wB.white)
            {
                if (this.index % 8 > 0)
                { squares |= (UInt64)1 << index + 7; }
                if (this.index % 8 < 7)
                { squares |= (UInt64)1 << index + 9; }

                squares &= thisBoard.bw[0, 1];

            }
            else
            {
                if (this.index % 8 > 0)
                { squares |= (UInt64)1 << index - 9; }
                if (this.index % 8 < 7)
                { squares |= (UInt64)1 << index - 7; }

                squares &= thisBoard.bw[0, 0];

            }



            return squares;
        }

        
    }

    public class Rook : Piece
    {
        public Rook(wB colour, int index)
        {
            base.rank = piece.rook;
            base.colour = colour;
            base.index = index;
        }

        public override UInt64 moves(Board thisBoard) // this method is similar to the RookAttackedBy method on piece and I hope to refactor the two with a common method in the next version.
        {
            UInt64 squares = 0;


            for (byte rotNum = 0; rotNum < 2; rotNum++)
            {
                int rotRow = Constants.rot[rotNum, index] / 8;
                int rotCol = Constants.rot[rotNum, index] % 8;

                byte lineOcc = BitConverter.GetBytes(thisBoard.occ[rotNum])[rotRow];
                byte[] lineMoves = new byte[1] { Constants.lineAvail[rotCol, lineOcc] };
                BitArray bitLineMoves = new BitArray(lineMoves);
                for (byte i = 0; i < 8; i++)
                {
                    if (bitLineMoves[i])
                    {
                        squares |= (UInt64)1 << Constants.irot[rotNum, i + 8 * rotRow];
                    }
                }
            }

            return squares;
        }

        public override UInt64 takes(Board thisBoard)
        {
            UInt64 squares = 0;


            for (byte rotNum = 0; rotNum < 2; rotNum++)
            {
                int rotRow = Constants.rot[rotNum, index] / 8;
                int rotCol = Constants.rot[rotNum, index] % 8;

                byte lineOcc = BitConverter.GetBytes(thisBoard.occ[rotNum])[rotRow];
                byte[] lineTakes = new byte[1] { Constants.lineTake[rotCol, lineOcc] };
                BitArray bitlineTakes = new BitArray(lineTakes);
                for (byte i = 0; i < 8; i++)
                {
                    if (bitlineTakes[i])
                    {
                        squares |= (UInt64)1 << Constants.irot[rotNum, i + 8 * rotRow];
                    }
                }
            }

            if (this.colour == wB.white)
            { squares &= thisBoard.bw[0, (int)wB.black]; }
            else
            { squares &= thisBoard.bw[0, (int)wB.white]; }
            return squares;
        }
    }






    public class Knight : Piece
    {
        public Knight(wB colour, int index)
        {
            base.rank = piece.knight;
            base.colour = colour;
            base.index = index;
        }
        public override UInt64 moves(Board thisBoard)
        {
            UInt64 squares = Constants.knightMove[index] & ~thisBoard.occ[0];
            return squares;
        }
        public override UInt64 takes(Board thisBoard)
        {
            UInt64 squares = Constants.knightMove[index];
            if (this.colour == wB.white)
            { squares &= thisBoard.bw[0, (int)wB.black]; }
            else
            { squares &= thisBoard.bw[0, (int)wB.white]; }
            return squares;
        }
    }

    public class Bishop : Piece
    {
        public Bishop(wB colour, int index)
        {
            base.rank = piece.bishop;
            base.colour = colour;
            base.index = index;
        }
        public override UInt64 moves(Board thisBoard)
        {
            UInt64 squares = 0;
            for (byte rotNum = 2; rotNum < 4; rotNum++)
            {
                byte rotIndex = Constants.rot[rotNum, index];
                byte diag = Constants.whichDiag[rotIndex];
                byte diagPos = Constants.diagPos[rotIndex];
                byte diagShift = Constants.diagShift[diag];
                byte diagOcc = (byte)((thisBoard.occ[rotNum] >> diagShift) & 255);
                byte[] diagMoves = new byte[1] { (byte)(Constants.lineAvail[diagPos, diagOcc] & Constants.diagMask[diag]) };
                BitArray bitDiagMoves = new BitArray(diagMoves);
                for (byte i = 0; i < 8; i++)
                {
                    if (bitDiagMoves[i])
                    {
                        squares |= (UInt64)1 << Constants.irot[rotNum, (i + diagShift)];
                    }
                }
            }

            return squares;
        }

        public override UInt64 takes(Board thisBoard)
        {
            UInt64 squares = 0;
            for (byte rotNum = 2; rotNum < 4; rotNum++)
            {
                byte rotIndex = Constants.rot[rotNum, index];
                byte diag = Constants.whichDiag[rotIndex];
                byte diagPos = Constants.diagPos[rotIndex];
                byte diagShift = Constants.diagShift[diag];
                byte diagOcc = (byte)((thisBoard.occ[rotNum] >> diagShift) & 255);
                byte[] diagTakes = new byte[1] { (byte)(Constants.lineTake[diagPos, diagOcc] & Constants.diagMask[diag]) };
                BitArray bitDiagTakes = new BitArray(diagTakes);
                for (byte i = 0; i < 8; i++)
                {
                    if (bitDiagTakes[i])
                    {
                        squares |= (UInt64)1 << Constants.irot[rotNum, (i + diagShift)];
                    }
                }
            }
            if (this.colour == wB.white)
            { squares &= thisBoard.bw[0, (int)wB.black]; }
            else
            { squares &= thisBoard.bw[0, (int)wB.white]; }
            return squares;
        }
    }

    public class Queen : Piece
    {
        public Queen(wB colour, int index)
        {
            base.rank = piece.queen;
            base.colour = colour;
            base.index = index;
        }
        public override UInt64 moves(Board thisBoard)
        {
            UInt64 squares = 0;


            // first adds the linewise moves.
            for (byte rotNum = 0; rotNum < 2; rotNum++)
            {
                int rotRow = Constants.rot[rotNum, index] / 8;
                int rotCol = Constants.rot[rotNum, index] % 8;

                byte lineOcc = BitConverter.GetBytes(thisBoard.occ[rotNum])[rotRow];
                byte[] lineMoves = new byte[1] { Constants.lineAvail[rotCol, lineOcc] };
                BitArray bitVertMoves = new BitArray(lineMoves);
                for (byte i = 0; i < 8; i++)
                {
                    if (bitVertMoves[i])
                    {
                        squares |= (UInt64)1 << Constants.irot[rotNum, i + 8 * rotRow];
                    }
                }
            }

            // then the diagonal moves
            for (byte rotNum = 2; rotNum < 4; rotNum++)
            {
                byte rotIndex = Constants.rot[rotNum, index];
                byte diag = Constants.whichDiag[rotIndex];
                byte diagPos = Constants.diagPos[rotIndex];
                byte diagShift = Constants.diagShift[diag];
                byte diagOcc = (byte)((thisBoard.occ[rotNum] >> diagShift) & 255);
                byte[] diagMoves = new byte[1] { (byte)(Constants.lineAvail[diagPos, diagOcc] & Constants.diagMask[diag]) };
                BitArray bitDiagMoves = new BitArray(diagMoves);
                for (byte i = 0; i < 8; i++)
                {
                    if (bitDiagMoves[i])
                    {
                        squares |= (UInt64)1 << Constants.irot[rotNum, (i + diagShift)];
                    }
                }
            }

            return squares;
        }

        public override UInt64 takes(Board thisBoard)
        {
            UInt64 squares = 0;



            for (byte rotNum = 0; rotNum < 2; rotNum++)
            {
                int rotRow = Constants.rot[rotNum, index] / 8;
                int rotCol = Constants.rot[rotNum, index] % 8;

                byte lineOcc = BitConverter.GetBytes(thisBoard.occ[rotNum])[rotRow];
                byte[] lineTakes = new byte[1] { Constants.lineTake[rotCol, lineOcc] };
                BitArray bitVertTakes = new BitArray(lineTakes);
                for (byte i = 0; i < 8; i++)
                {
                    if (bitVertTakes[i])
                    {
                        squares |= (UInt64)1 << Constants.irot[rotNum, i + 8 * rotRow];
                    }
                }
            }


            for (byte rotNum = 2; rotNum < 4; rotNum++)
            {
                byte rotIndex = Constants.rot[rotNum, index];
                byte diag = Constants.whichDiag[rotIndex];
                byte diagPos = Constants.diagPos[rotIndex];
                byte diagShift = Constants.diagShift[diag];
                byte diagOcc = (byte)((thisBoard.occ[rotNum] >> diagShift) & 255);
                byte[] diagTakes = new byte[1] { (byte)(Constants.lineTake[diagPos, diagOcc] & Constants.diagMask[diag]) };
                BitArray bitDiagTakes = new BitArray(diagTakes);
                for (byte i = 0; i < 8; i++)
                {
                    if (bitDiagTakes[i])
                    {
                        squares |= (UInt64)1 << Constants.irot[rotNum, (i + diagShift)];
                    }
                }
            }
            if (this.colour == wB.white)
            { squares &= thisBoard.bw[0, (int)wB.black]; }
            else
            { squares &= thisBoard.bw[0, (int)wB.white]; }
            return squares;
        }

    }

    public class King : Piece // finally the king's moves and takes
    {
        public King(wB colour, int index)
        {
            base.rank = piece.king;
            base.colour = colour;
            base.index = index;
        }

        public override UInt64 moves(Board thisBoard)
        {
            UInt64 squares = Constants.kingMove[index] & ~thisBoard.occ[0];
            return squares;
        }


        public override UInt64 takes(Board thisBoard)
        {
            UInt64 squares = Constants.kingMove[index];
            if (this.colour == wB.white)
            { squares &= thisBoard.bw[0, (int)wB.black]; }
            else
            { squares &= thisBoard.bw[0, (int)wB.white]; }
            return squares;

        }


    }
}



