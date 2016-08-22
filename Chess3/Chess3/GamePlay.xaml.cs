using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;

namespace Chess3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class GamePlay : Window
    {
        // the gamePlay window has a thisGame attribute, 64 Squares and 3 event handlers. 
        private Game thisGame;
        private Square[] square = new Square[64];
        private MouseButtonEventHandler firstClick;
        private MouseButtonEventHandler moveTo;
        private MouseButtonEventHandler moveToAndTake;


        public GamePlay(Game game) // this is the constructor for the main game board
        {
            InitializeComponent();

            thisGame = game; // MainWindow has initialized the players names and handicaps and creates a game with these attributes. it is passed to the GamPlay window.

            for (byte i = 0; i < 64; i++)
            {
                
                square[i] = new Square(i); // the squares are initialised
                cnvBoard.Children.Add(square[i]);
                Canvas.SetLeft(square[i], (i % 8) * 80);
                Canvas.SetTop(square[i], 560 - (i / 8) * 80);
                // Label lblNumber = new Label(); // 
                // lblNumber.Content = i.ToString(); // used to get a visual ID of each square during development.

                //if (square[i].colour == wB.black) { lblNumber.Foreground = Brushes.White; } else { lblNumber.Foreground = Brushes.Black; }
                //square[i].Children.Add(lblNumber);
                firstClick = new MouseButtonEventHandler(square_firstClick);
                moveTo = new MouseButtonEventHandler(square_moveTo);
                moveToAndTake = new MouseButtonEventHandler(square_moveToAndTake);

            }

            foreach (Piece thisPiece in thisGame.board.bwList[0]) // adds the white pieces
            {

                square[thisPiece.index].piece = thisPiece;
            }
            foreach (Piece thisPiece in thisGame.board.bwList[1]) // adds the black pieces
            {

                square[thisPiece.index].piece = thisPiece;
            }


            // sets up the labels for the player names.
            if (thisGame.player[0].colour == wB.white)
            {
                lblWhitePlayer.Content = thisGame.player[0].name;
                lblBlackPlayer.Content = thisGame.player[1].name;

            }
            else
            {
                lblWhitePlayer.Content = thisGame.player[1].name;
                lblBlackPlayer.Content = thisGame.player[0].name;
            }


            // sets up some of the handicap information
            thisGame.board.toPlay = wB.white;
            if (
                (thisGame.player[0].handicap == handicap.theMove)
                || (thisGame.player[0].handicap == handicap.theKnightAndMove)
                || (thisGame.player[0].handicap == handicap.theRookAndMove)
                || (thisGame.player[1].handicap == handicap.pawnAgainstTheMove)
                )
            { thisGame.board.toPlay = thisGame.player[1].colour; }
            if (
                (thisGame.player[1].handicap == handicap.theMove)
                || (thisGame.player[1].handicap == handicap.theKnightAndMove)
                || (thisGame.player[1].handicap == handicap.theRookAndMove)
                || (thisGame.player[0].handicap == handicap.pawnAgainstTheMove)
                )
            { thisGame.board.toPlay = thisGame.player[0].colour; }

            play(); // calls the play method

        }


        private void play()
        {
            // displays message box if anyone is in check - but does not prevent you from moving into check.(to fix ***)
            if (thisGame.check[0]) { MessageBox.Show("White is in Check"); }
            if (thisGame.check[1]) { MessageBox.Show("Black is in Check"); }

            lblToPlay.Content = thisGame.board.toPlay.ToString() + " to Play";
            foreach (Piece thisPiece in thisGame.board.bwList[(int)thisGame.board.toPlay])
            { square[thisPiece.index].MouseDown += firstClick; } // adds the firstClick event handler to each square occupied by a piece of the toPlay colour
        }

        private void square_firstClick(object sender, MouseButtonEventArgs e)
        {
            // This is the event handler used when someone selects a piece to move.

            Square thisSquare = sender as Square; // this is so cool. sender as Object before I had used tag as a property in VB which you could access the numbers you wanted but now I can get the whole thing.
            thisGame.board.selectedPiece = thisSquare.piece; // sets the public property of Board to the selected piece



            UInt64 moves = thisSquare.piece.moves(thisGame.board);
            UInt64 takes = thisSquare.piece.takes(thisGame.board);
            if (!((moves == 0) && (takes == 0))) // if the piece has some valid moves put in the event handlers for them and highlight the squares appropriately.
            {
                BitArray bitMoves = new BitArray(BitConverter.GetBytes(moves)); // BitConverter converts the UInt64 into an array of 8 bytes. This array is converted into a 64 bit BitArray
                BitArray bitTakes = new BitArray(BitConverter.GetBytes(takes));
                for (byte i = 0; i < 64; i++)
                {
                    square[i].MouseDown -= firstClick; // stop them from changing their mind (touch and move)
                    if (bitMoves[i]) // if its a legal move highlight it and add the moveTo event handler
                    {
                        square[i].highlight();
                        square[i].MouseDown += moveTo;
                    }
                    else if (bitTakes[i]) // if its a legal take highlight in red and add moveToAndTake event handler
                    {
                        square[i].highlightRed();
                        square[i].MouseDown += moveToAndTake;
                    }
                    else
                    {
                        square[i].unHighlight();
                        square[i].MouseDown -= moveToAndTake;
                        square[i].MouseDown -= moveTo;
                    }
                }
            }
            else // if the square has no legal moves tell the player and allow another selection
            {
                MessageBox.Show("You cannot move this piece");
            }

        }

        private void square_moveTo(object sender, MouseButtonEventArgs e) // this is the event handler for thw square a pieve wants to move to
        {
            Square thisSquare = sender as Square; // gets the destination square from sender


            thisGame.board.remove(thisGame.board.selectedPiece); // 'picks up' the source piece from the board
            square[thisGame.board.selectedPiece.index].piece = null; // empties the source square in the UI
            thisGame.board.selectedPiece.index = thisSquare.index; // changes the index number
            thisGame.board.add(thisGame.board.selectedPiece); // adds the piece back to the board
            thisSquare.piece = thisGame.board.selectedPiece; // adds the piece to the UI

            for (byte i = 0; i < 64; i++) // removes all event handlers and returns squares to their original colour.
            {
                square[i].MouseDown -= moveTo;
                square[i].MouseDown -= moveToAndTake;
                
                square[i].unHighlight();
            }

            if  // this part handles the pawn promotion - if it is reached the last rank it can become any other piece. 
                // I have recently learnt that it only gets promoted on the next turn and may not be taken in between. *** to Fix
                (
                ((thisSquare.piece.GetType() == typeof(Pawn)) && (thisSquare.piece.colour == wB.white) && (thisSquare.index / 8 == 7))
          ||
              ((thisSquare.piece.GetType() == typeof(Pawn)) && (thisSquare.piece.colour == wB.black) && (thisSquare.index / 8 == 0))
                )
            {
                wndPromote thisWndPromote = new wndPromote(thisSquare.piece); // instantiates the new Window and passes the Pawn to be promoted to the constructor
                thisWndPromote.ShowDialog(); // ShowDialog() halts the current thread until the window is closed.


                thisGame.board.remove(thisSquare.piece);// when the window is closed the piece is wiped from the board
                thisSquare.piece = null;
                thisSquare.piece = thisWndPromote.promoteTo; // sets the square's piece to the new piece selected in the dialog for the benefit of the UI
                thisGame.board.add(thisWndPromote.promoteTo); // adds the piece to the board
                
            }

            // changes the colour of toPlay - I would like to write a method or property of the enum for toggling colour. 
            // Ideally a method called opposite and another method of board to change the colour of toPlay
            
            // Not sure if enums support other attributes or methods.

                if (thisGame.board.toPlay == wB.black)
                { thisGame.board.toPlay = wB.white; }
                else
                { thisGame.board.toPlay = wB.black; }


                play();

            


        }

        private void square_moveToAndTake(object sender, MouseButtonEventArgs e)
        {
            Square thisSquare = sender as Square;
            if (thisSquare.piece.GetType() == typeof(King))
            {
                MessageBox.Show("Game Over " + thisGame.board.toPlay.ToString() + " has won"); // if the king is taken the game is won. I need to make a proper test for checkmate *** to fix
                MainWindow thisMainWindow = new MainWindow();
                thisMainWindow.Show();
                this.Close();
            }
            else
            {
                thisGame.board.remove(thisGame.board.selectedPiece);
                square[thisGame.board.selectedPiece.index].piece = null;

                thisGame.board.remove(thisSquare.piece);
                thisSquare.piece = null;
                thisGame.board.selectedPiece.index = thisSquare.index;
                thisGame.board.add(thisGame.board.selectedPiece);
                thisSquare.piece = thisGame.board.selectedPiece;

                for (byte i = 0; i < 64; i++)
                {
                    square[i].MouseDown -= moveTo;
                    square[i].MouseDown -= moveToAndTake;
                    square[i].unHighlight();
                }

                if  // this part handles the pawn promotion - if it is reached the last rank it can become any other piece. 
                    // I have recently learnt that it only gets promoted on the next turn and may not be taken in between. *** to Fix 
                    (
                    ((thisSquare.piece.GetType() == typeof(Pawn)) && (thisSquare.piece.colour == wB.white) && (thisSquare.index / 8 == 7))
              ||
                  ((thisSquare.piece.GetType() == typeof(Pawn)) && (thisSquare.piece.colour == wB.black) && (thisSquare.index / 8 == 0))
                    )
                {
                    wndPromote thisWndPromote = new wndPromote(thisSquare.piece); // instantiates the new Window and passes the Pawn to be promoted to the constructor
                    thisWndPromote.ShowDialog(); // ShowDialog() halts the current thread until the window is closed.


                    thisGame.board.remove(thisSquare.piece);// when the window is closed the piece is wiped from the board
                    thisSquare.piece = null;
                    thisSquare.piece = thisWndPromote.promoteTo; // sets the square's piece to the new piece selected in the dialog for the benefit of the UI
                    thisGame.board.add(thisWndPromote.promoteTo); // adds the piece to the board

                }


                if (thisGame.board.toPlay == wB.black)
                { thisGame.board.toPlay = wB.white; }
                else
                { thisGame.board.toPlay = wB.black; }


                play();


            }

        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e) // new game
        {
            if (MessageBox.Show("Really Quit this game?", "Chess3", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                MainWindow newMainWindow = new MainWindow();
                newMainWindow.Show();
                this.Close();
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
