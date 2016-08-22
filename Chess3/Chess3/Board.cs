using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chess3
{



    



        public enum boardOrder
        {
            up, r90, l45, r45
        }



        public class Board
        {
            
     
            public Piece selectedPiece { get; set; } // used to denoted which piece is selected when the user clicks a square
            public wB toPlay { get; set; } // which person is to play
            private UInt64[,] _bitBoard = new UInt64[4, 14]; // 4 rotations 14 different kinds of pieces : 7 pieces enumerated in Enum.cs + 7*0 for white, 7*1 for black.
            private UInt64[,] _bw = new UInt64[4, 2];        // 4 rotations white 0 black 1 is the square occupied by a piece of the appropriate colour?
            private UInt64[] _occ = new UInt64[4];           // 4 rotations: is the square occupied?
            public UInt64[,] bitBoard
            {
                get{return _bitBoard;} set {_bitBoard=value;}
            }
            public UInt64[,] bw
            {
                get { return _bw; }
                set { _bw = value; }
            }
            public UInt64[] occ
            {
                get { return _occ; }
                set { _occ = value; }
            }
            public List<Piece>[] bwList = new List<Piece>[2]; // this is a duplication of effort but very useful. a list of pieces for each colour.
            public int move { get; set; } // this holds the number of moves played so far (initial = 0) not yet implemented.

            public Board() // the constructor sets up the two lists.
            {
                bwList[0] = new List<Piece>();
                bwList[1] = new List<Piece>();
            }

            //  here are the methods for the class board


            //  xor simply adds the piece to the various boards if it not there. or, if it is there removes it. the operator in c# is ^ and can be used like += thus ^=
            public void xor(Piece pieceToXor)
            {
                for (byte i = 0; i < 4; i++)
                {
                    _bitBoard[i, pieceToXor.colRank] ^= (UInt64)1 << Constants.rot[i, pieceToXor.index];
                    _bw[i, (int)pieceToXor.colour] ^= (UInt64)1 << Constants.rot[i, pieceToXor.index];
                    _occ[i] ^= (UInt64)1 << Constants.rot[i, pieceToXor.index];
                }
            }

            // this is an overload method for xor which takes piece attributes, it wasn't eventually used. not the differene between Piece (class) and piece (enum)
            public void xor(wB colour, piece rank, byte index)
            {
                int colRank = 7 * (int)colour + (int)rank;
                for (byte i = 0; i < 4; i++)
                {
                    _bitBoard[i, colRank] ^= (UInt64)1 << Constants.rot[i, index];
                    _bw[i, (int)colour] ^= (UInt64)1 << Constants.rot[i, index];
                    _occ[i] ^= (UInt64)1 << Constants.rot[i, index];
                }
            }

            // when a piece is added to the board, it is added to the requisite list (depending on its colour) and then added to the bitboards using the xor method.
            public void add(Piece pieceToAdd)
            {
                bwList[(int)pieceToAdd.colour].Add(pieceToAdd);
                xor(pieceToAdd);
                
                
            }

            // the remove method removes the piece from the list and the requisite bitboards
            public void remove(Piece pieceToRemove)
            {
                bwList[(int)pieceToRemove.colour].Remove(pieceToRemove);
                xor(pieceToRemove);
            }




        }

    }



       
