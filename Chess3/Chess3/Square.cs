using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chess3
{

    public class Square : Grid // the class for square inherits from the c# class Grid and adds a few proprties and methods. 
                               // This is handy as event handlers will return sender as square so you can see which piece has been clicked.
    {
        private byte _index { get; set; } // so a square has an index.
        public byte index 
        { 
            get 
            { return _index; } 
            set 
            { _index = value; } 
        }
        public wB colour // it has a colour
        {
            get;
            set;

        }
        private Piece _piece; // it contains a piece (or null if it is empty)
        public Piece piece
        {
            get
            { return _piece; }
            set
            { 
                _piece = value;
                this.Children.Clear(); if (value!=null){this.Children.Add(value.drawMe());} }
       }
        public override string ToString()
        {
            return index.ToString();
        }
        public void highlight()
        {
            if (colour == wB.black) { this.Background = bruHighlight.black; }
            else { this.Background = bruHighlight.white; }
        }
        public void highlightRed()
        {
            if (colour == wB.black) { this.Background = bruHighlightRed.black; }
            else { this.Background = bruHighlightRed.white; }
        }
        public void unHighlight()
        {
            if (colour == wB.black) { this.Background = Brushes.Black; }
            else { this.Background = Brushes.White; }
        }
        public Square(byte i)
            : base()
        {
            index = i;
            colour = (wB)((index + (index / 8) + 1) % 2);
            this.Width = 80;
            this.Height = 80;
            this.unHighlight();
        }

    }
}
