using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace GaijinExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //ExplorerFrame.Source = new Uri("Explorer.xaml");
            //Debug.WriteLine("Frame source: " + ExplorerFrame.Source);
            ExplorerFrame.Navigate(new Uri("Explorer.xaml", UriKind.RelativeOrAbsolute));
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ExplorerFrame.Navigate(new Uri("Explorer.xaml", UriKind.RelativeOrAbsolute));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExplorerFrame.CanGoBack)
            {
                ExplorerFrame.GoBack();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExplorerFrame.CanGoForward)
            {
                ExplorerFrame.GoForward();
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{
        //    ExplorerFrame.Navigate(new System.Uri("TestPage2.xaml", UriKind.RelativeOrAbsolute));
        //}

        //private void MangaModeButton_Click(object sender, RoutedEventArgs e)
        //{
        //    ModeButton.Header = "Manga";
        //}

        //private void AnimeModeButton_Click(object sender, RoutedEventArgs e)
        //{
        //    ModeButton.Header = "Anime";
        //}
    }
}
