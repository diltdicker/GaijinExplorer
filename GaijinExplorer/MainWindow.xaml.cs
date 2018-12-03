using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

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
            if (App.FIRST_MANGA_GRAB)
            {
                App.FIRST_MANGA_GRAB = !App.FIRST_MANGA_GRAB;
                Task.Run(() => UpdateMangaDB());
            }
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

    //    Debug.WriteLine("Updating DB");
    //            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
    //            {
    //                DataBaseProgress.Maximum = mangas.Count;
    //                DataBaseProgress.Value = 0;
    //            }));
    //            // Quick DB Update
    //            for (int i = 0; i< 5; i++)
    //            {
    //                Manga.Manga manga = mangas[i];
    //Task.Run(async () => 
    //                {
                        
    //                    Application.Current.Dispatcher.Invoke(DispatcherPriority.SystemIdle, new ThreadStart(delegate
    //                    {
    //                        DataBaseProgress.Value = i;
    //                    }));
    //                });
    //            }
                //Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                //{
                //    DataBaseProgress.Maximum = mangas.Count;
                //    DataBaseProgress.Value = 0;
                //}));
                //// Long DB Update
                //for (int i = 0; i < mangas.Count; i++)
                //{
                //    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                //    {
                //        DataBaseProgress.Value = i;
                //    }));
                //}
                //Application.Current.Dispatcher.Invoke(DispatcherPriority., new ThreadStart(delegate
                //{
                //    DataBaseProgress.Visibility = Visibility.Collapsed;
                //}));

        public async Task UpdateMangaDB()
        {
            Debug.WriteLine("getting info for DB");
            List<Manga.Manga> mangaList = null;
            await Http.HttpMangaEden.GetAllMangaTitlesAsync((List<Manga.Manga> mangas) =>
            {
                mangaList = mangas;
                return true;
            });
            int i = 0;
            Application.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new ThreadStart(delegate {
                DataBaseProgress.Value = 0;
                DataBaseProgress.Maximum = mangaList.Count;
            }));
            foreach (Manga.Manga manga in mangaList)
            {
                i++;
                await Database.MangaDAO.CreateMangaAsyncLite(manga);
                if (i%100 == 0)
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new ThreadStart(delegate {
                        DataBaseProgress.Value = i;
                    }));
                }
            }
            Application.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new ThreadStart(delegate {
                DataBaseProgress.Visibility = Visibility.Collapsed;
            }));
        }
    }
}
