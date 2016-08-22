using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Chess3
{
    public class Game // the Game class has the kings as properties so you can test if they are in check
    {
        public King whiteKing = new King(wB.white, 4);
        public King blackKing = new King(wB.black, 60);
        
        public bool[] check
        {
            get
            {
                bool[] _check = new bool[2];
                _check[0]=(whiteKing.AttackedBy(_board)!=0);
                _check[1]=(blackKing.AttackedBy(_board)!=0);
                return _check;
            }

        }
        
        public bool won { get; set; }
        private Player[] _player = new Player[2];
        private Board _board = new Board();
        private Board _lastBoard = new Board();
        public Board board
        {
            get { return _board; }
            set { _board = value; }
        }
        public Board lastBoard
        {
            get { return _lastBoard; }
            set { _lastBoard = value; }
        }

        public void retract()
        {
            board = lastBoard;
        }
        public void advance()
        { lastBoard = board; }
        public Player[] player
        {
            get { return _player; }
            set { _player = value; }
        }
        public Game() // the constructor of Game adds all the pieces to the board (but not to the UI) and finally adds the public kings.
        {
            won = false;
            Pawn pawnToAdd;
            for (int i = 0; i < 8; i++)
            {
                pawnToAdd = new Pawn(wB.white, (i + 8));
                board.add(pawnToAdd);
                pawnToAdd = new Pawn(wB.black, (i + 48));
                board.add(pawnToAdd);
            }

            Rook rookToAdd;
            rookToAdd= new Rook(wB.white, 0);
            board.add(rookToAdd);
            rookToAdd= new Rook(wB.white, 7);
            board.add(rookToAdd);

            rookToAdd= new Rook(wB.black, 56);
            board.add(rookToAdd);
            rookToAdd= new Rook(wB.black, 63);
            board.add(rookToAdd);

            Knight knightToAdd;
            knightToAdd = new Knight(wB.white, 1);
            board.add(knightToAdd);
            knightToAdd = new Knight(wB.white,6);
            board.add(knightToAdd);
            knightToAdd = new Knight(wB.black, 57);
            board.add(knightToAdd);
            knightToAdd = new Knight(wB.black, 62);
            board.add(knightToAdd);

            Bishop bishopToAdd;
            bishopToAdd = new Bishop(wB.white, 2);
            board.add(bishopToAdd);
            bishopToAdd = new Bishop(wB.white, 5);
            board.add(bishopToAdd);
            bishopToAdd = new Bishop(wB.black, 58);
            board.add(bishopToAdd);
            bishopToAdd = new Bishop(wB.black, 61);
            board.add(bishopToAdd);

            Queen queenToAdd;
            queenToAdd = new Queen(wB.white,3);
            board.add(queenToAdd);
            queenToAdd = new Queen(wB.black, 59);
            board.add(queenToAdd);


            board.add(whiteKing);
            board.add(blackKing);

            lastBoard = board;



            }


        }

    }

