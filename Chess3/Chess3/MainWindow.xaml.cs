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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window // This is the first window a player sees. It sets up the two players, their colour, the handicap and - eventually - the possibility of computer players.
    {
        public MainWindow()
        {
            
            InitializeComponent();

            cmbPlayer1Handicap.ItemsSource = Enum.GetValues(typeof(handicap));
            cmbPlayer1Handicap.SelectionChanged -= cmbPlayer1Handicap_SelectionChanged;
            cmbPlayer1Handicap.SelectedValue = handicap.none;
            cmbPlayer1Handicap.SelectionChanged += cmbPlayer1Handicap_SelectionChanged;         
            cmbPlayer2Handicap.ItemsSource = Enum.GetValues(typeof(handicap));
            cmbPlayer2Handicap.SelectionChanged -= cmbPlayer2Handicap_SelectionChanged;
            cmbPlayer2Handicap.SelectedValue = handicap.none;
            cmbPlayer2Handicap.SelectionChanged += cmbPlayer2Handicap_SelectionChanged;
            cmbPlayer1Colour.ItemsSource = Enum.GetValues(typeof(wB));
            cmbPlayer2Colour.ItemsSource = Enum.GetValues(typeof(wB));
            cmbPlayer1HuAi.ItemsSource = Enum.GetValues(typeof(huAi));
            cmbPlayer2HuAi.ItemsSource = Enum.GetValues(typeof(huAi));
            

        }

        private void cmbPlayer1Handicap_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((handicap)cmbPlayer1Handicap.SelectedValue != handicap.none)
            {
                cmbPlayer2Handicap.SelectionChanged -= cmbPlayer2Handicap_SelectionChanged;
                cmbPlayer2Handicap.SelectedValue = handicap.none;
                cmbPlayer2Handicap.SelectionChanged += cmbPlayer2Handicap_SelectionChanged;

            }
            checkFields();

        }

        private void cmbPlayer2Handicap_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((handicap)cmbPlayer2Handicap.SelectedValue != handicap.none)
            {
                cmbPlayer1Handicap.SelectionChanged -= cmbPlayer1Handicap_SelectionChanged;
                cmbPlayer1Handicap.SelectedValue = handicap.none;
                cmbPlayer1Handicap.SelectionChanged += cmbPlayer1Handicap_SelectionChanged;
                
            }
            checkFields();

        }

        private void cmbPlayer1Colour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            cmbPlayer2Colour.SelectionChanged -= cmbPlayer2Colour_SelectionChanged;
            if ((wB)cmbPlayer1Colour.SelectedValue == (wB)wB.black)
            { cmbPlayer2Colour.SelectedValue = wB.white; }
            else { cmbPlayer2Colour.SelectedValue = wB.black; }
            cmbPlayer2Colour.SelectionChanged += cmbPlayer2Colour_SelectionChanged;
            checkFields();
        }

        private void cmbPlayer2Colour_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cmbPlayer1Colour.SelectionChanged -= cmbPlayer1Colour_SelectionChanged;
            if ((wB)cmbPlayer2Colour.SelectedValue == wB.black) 
            { cmbPlayer1Colour.SelectedValue = wB.white; }
            else { cmbPlayer1Colour.SelectedValue = wB.black; }
            cmbPlayer1Colour.SelectionChanged += cmbPlayer1Colour_SelectionChanged;
            checkFields();
        }

        private void checkFields()
        {
            if (
                (txtPlayer1Name.Text != "")
                && (txtPlayer2Name.Text != "")
                && (cmbPlayer1HuAi.SelectedValue!=null)
                && (cmbPlayer2HuAi.SelectedValue != null)
                && (cmbPlayer1Colour.SelectedValue!=null)
                )

            { btnPlay.IsEnabled = true; }
            else
            { btnPlay.IsEnabled = false; }

        }

        private void txtPlayer1Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkFields();
        }

        private void txtPlayer2Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            checkFields();
        }

        private void cmbPlayer1HuAi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkFields();
        }

        private void cmbPlayer2HuAi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            checkFields();
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {

            Game thisGame = new Game();
            thisGame.player[0].name = txtPlayer1Name.Text;
            thisGame.player[1].name = txtPlayer2Name.Text;
            thisGame.player[0].colour = (wB)cmbPlayer1Colour.SelectedValue;
            thisGame.player[1].colour = (wB)cmbPlayer2Colour.SelectedValue;
            thisGame.player[0].handicap = (handicap)cmbPlayer1Handicap.SelectedValue;
            thisGame.player[1].handicap = (handicap)cmbPlayer2Handicap.SelectedValue;
            thisGame.player[0].huai = (huAi)cmbPlayer1HuAi.SelectedValue;
            thisGame.player[1].huai = (huAi)cmbPlayer2HuAi.SelectedValue;
            GamePlay thisGamePlay = new GamePlay(thisGame);
            




            thisGamePlay.Show();
            this.Close();


        }

    }
}
