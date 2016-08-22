using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Chess3
{
    public static class imgPiece // this is another static class of the BitmapImages of the pieces that will be used many times as Source for the images in the game.
    {
        public static BitmapImage[] bmiPiece = new BitmapImage[14];
        static imgPiece()
        {
            string[] src = new string[14] { "wp.png", "wpe.png", "wr.png", "wn.png", "wb.png", "wq.png", "wk.png", "bp.png", "bpe.png", "br.png", "bn.png", "bb.png", "bq.png", "bk.png" };
            for (byte i = 0; i < 14; i++)
            {

                bmiPiece[i] = new BitmapImage();
                bmiPiece[i].BeginInit();
                bmiPiece[i].UriSource = new Uri(src[i], UriKind.Relative);
                bmiPiece[i].EndInit();

            }
        }
    }

    public static class bruHighlight // brushes for highlighting the squares - must be public to allow other classes to use them.
    {
        public static SolidColorBrush black;
        public static SolidColorBrush white;
        static bruHighlight()
        {
            black = new SolidColorBrush(Color.FromRgb(44,44,22));
            white = new SolidColorBrush(Color.FromRgb(215,215,179));
        }
    }

    public static class bruHighlightRed
    {
        public static SolidColorBrush black;
        public static SolidColorBrush white;
        static bruHighlightRed()
        {
            black = new SolidColorBrush(Color.FromRgb(104, 44, 22));
            white = new SolidColorBrush(Color.FromRgb(255, 74, 69));
        }
    }
}
