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
    public struct Bitboard
    {
        private UInt64[] _bitBoard = new UInt64[4];
        public UInt64 up
        {
            get { return _bitBoard[0]; }
            set { _bitBoard[0] = value; }
        }
        public UInt64 r90
        {
            get { return _bitBoard[1]; }
            set { _bitBoard[1] = value; }
        }
        public UInt64 l45
        {
            get { return _bitBoard[2]; }
            set { _bitBoard[2] = value; }
        }
        public UInt64 r45
        {
            get { return _bitBoard[3]; }
            set { _bitBoard[3] = value; }
        }
        public UInt64[] bitBoard
        {
            get { return _bitBoard; }
            set { _bitBoard = value; }
        }
        public static Bitboard operator ^(Bitboard a,Bitboard b)
        {
            Bitboard c = new Bitboard();
            for(byte i=0;i<4;i++)
            {c.bitBoard[i] = a.bitBoard[i]^b.bitBoard[i];}
            return c;
        }
        
    }


    



        public enum boardOrder
        {
            up, r90, l45, r45
        }



        public class Board
        {
            public Board()
            {
                bwList[0] = new List<Piece>();
                bwList[1] = new List<Piece>();
            }
            public Piece selectedPiece { get; set; }
            public wB toPlay { get; set; }
            private UInt64[,] _bitBoard = new UInt64[4, 14];
            private UInt64[,] _bw = new UInt64[4, 2];
            private UInt64[] _occ = new UInt64[4];
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


            
            public List<Piece>[] bwList = new List<Piece>[2];
            
            public int move { get; set; }

            public void xor(Piece pieceToXor)
            {
                for (byte i = 0; i < 4; i++)
                {
                    _bitBoard[i, pieceToXor.colRank] ^= (UInt64)1 << Constants.rot[i, pieceToXor.index];
                    _bw[i, (int)pieceToXor.colour] ^= (UInt64)1 << Constants.rot[i, pieceToXor.index];
                    _occ[i] ^= (UInt64)1 << Constants.rot[i, pieceToXor.index];
                }
            }
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
            public void add(Piece pieceToAdd)
            {
                bwList[(int)pieceToAdd.colour].Add(pieceToAdd);
                xor(pieceToAdd);
                
                
            }
            public void remove(Piece pieceToRemove)
            {
                bwList[(int)pieceToRemove.colour].Remove(pieceToRemove);
                xor(pieceToRemove);
            }




        }

    }



       
