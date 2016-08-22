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
using System.Windows.Shapes;

namespace Chess3
{
    /// <summary>
    /// Interaction logic for promote.xaml
    /// </summary>
    public partial class wndPromote : Window // new window wndPromote contains four grids with the pieces in
    {
        public Rook nomRook;
        public Knight nomKnight;
        public Bishop nomBishop;
        public Queen nomQueen;
        public Piece promoteTo
        { get; set; }
        public wndPromote(Piece thisPiece) // the constructor takes the pawn to be promoted as its argument
        {
            InitializeComponent();
            // the constructor creates 4 pieces the same colour as the pawn and puts them in the grids.
            nomRook = new Rook(thisPiece.colour, thisPiece.index);
            nomKnight = new Knight(thisPiece.colour, thisPiece.index);
            nomBishop = new Bishop(thisPiece.colour, thisPiece.index);
            nomQueen = new Queen(thisPiece.colour, thisPiece.index);

            grdRook.Children.Add(nomRook.drawMe());
            grdKnight.Children.Add(nomKnight.drawMe());
            grdBishop.Children.Add(nomBishop.drawMe());
            grdQueen.Children.Add(nomQueen.drawMe());
            


        }

        // the 4 event handlers put the chosen piece into a property called promoteTo and close the window. Note the object still exists and promoteTo can be accessed by the main program.
        private void grdRook_MouseDown(object sender, MouseButtonEventArgs e)
        {
            promoteTo = nomRook;
            this.Close();

        }

        private void grdKnight_MouseDown(object sender, MouseButtonEventArgs e)
        {
            promoteTo = nomKnight;
            
            this.Close();

        }

        private void grdBishop_MouseDown(object sender, MouseButtonEventArgs e)
        {
            promoteTo = nomBishop;
            this.Close();

        }

        private void grdQueen_MouseDown(object sender, MouseButtonEventArgs e)
        {
            promoteTo = nomQueen;
            this.Close();

        }


    }
}
