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
        public static Frame Frame { get; set; }
        public static NavigationService navigationService;
        public enum ExplorerPage { ExploreMangaPage, MangaPage, ChapterPage}
        private static int FrameHistoryIndex { get; set; }
        public static List<ExplorerPage> FrameHistory { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Frame = ExplorerFrame;
            FrameHistoryIndex = -1;
            FrameHistory = new List<ExplorerPage>();
            //Frame.CacheMode = null;
            navigationService = ExplorerFrame.NavigationService;

            //ExplorerFrame.Source = new Uri("Explorer.xaml");
            //Debug.WriteLine("Frame source: " + ExplorerFrame.Source);
            ExplorerFrame.Navigate(new MangaExplorerPage());
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            //if (FrameHistory[FrameHistoryIndex] != ExplorerPage.ExploreMangaPage)
            //{
                App.CancellationToken.Cancel();
                ExplorerFrame.Navigate(new MangaExplorerPage());
            //}
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (ExplorerFrame.CanGoBack)
            {
                App.CancellationToken.Cancel();
                ExplorerFrame.GoBack();
                FrameHistoryIndex--;
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (ExplorerFrame.CanGoForward)
            {
                App.CancellationToken.Cancel();
                ExplorerFrame.GoForward();
                FrameHistoryIndex++;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        public static void AddToFrameHistory(ExplorerPage page)
        {
            while (FrameHistoryIndex < FrameHistory.Count - 1)
            {
                FrameHistory.RemoveAt(FrameHistory.Count - 1);
            }
            FrameHistory.Add(page);
            FrameHistoryIndex++;
            if (FrameHistory.Count > 5)
            {
                navigationService.RemoveBackEntry();
                Debug.WriteLine("removed entry");
                FrameHistory.RemoveAt(0);
                FrameHistoryIndex--;
            }
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
