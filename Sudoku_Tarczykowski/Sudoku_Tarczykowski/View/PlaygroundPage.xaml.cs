using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Sudoku_Tarczykowski.View
{
    /// <summary>
    /// Interaction logic for PlaygroundPage.xaml
    /// </summary>
    public partial class PlaygroundPage : Page
    {
        public PlaygroundPage()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(Regex.IsMatch(((TextBox)sender).Text, "([^1-9])"))
            {
                ((TextBox)sender).Text = "";
            }
        }

        private void TextBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            chosenTextBoxToHelp.Text = Regex.Match(((TextBox)sender).Name, @"([0-9]+)").Value;
        }
    }
}
