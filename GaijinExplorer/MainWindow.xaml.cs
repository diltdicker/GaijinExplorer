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
using System.Collections;

namespace GaijinExplorer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Frame NavigationFrame { get; set; }
        public static NavigationService navigationService;
        public enum ExplorerPage { ExploreMangaPage, MangaPage, ChapterPage}
        private static int FrameHistoryIndex { get; set; }
        public static List<ExplorerPage> FrameHistory { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            NavigationFrame = ExplorerFrame;
            FrameHistoryIndex = -1;
            FrameHistory = new List<ExplorerPage>();
            //Frame.CacheMode = null;
            navigationService = ExplorerFrame.NavigationService;

            //ExplorerFrame.Source = new Uri("Explorer.xaml");
            //Debug.WriteLine("Frame source: " + ExplorerFrame.Source);
            if (App.FIRST_MANGA_GRAB)
            {
                App.FIRST_MANGA_GRAB = !App.FIRST_MANGA_GRAB;
                //Task.Run(() => UpdateMangaDB());
                Task.Factory.StartNew(() => UpdateMangaDB(), TaskCreationOptions.LongRunning);          // Better way of doing it for long running task
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

        // Don't use
        public static void AddToFrameHistory(ExplorerPage page)
        {
            IEnumerable enumerable = NavigationFrame.BackStack;
            if (NavigationFrame.CanGoBack)
            {
                List<object> list = NavigationFrame.BackStack.Cast<object>().ToList();
                Debug.WriteLine("backstack length: " + list.Count);
                if (list.Count > 1)
                {
                    IEnumerator enumerator = NavigationFrame.BackStack.GetEnumerator();
                    enumerator.MoveNext();
                    //var q = (enumerator.Current.GetType());
                    Debug.WriteLine(enumerator.Current.GetType());
                }
            }
        }

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
                await Database.MangaDAO.CreateMangaAsyncLite(manga);                                    // Puts in only Title and ID
                if (i%100 == 0)
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new ThreadStart(delegate {
                        DataBaseProgress.Value = i;
                    }));
                }
            }

            // TODO
            // Update All Manga Here

            Application.Current.Dispatcher.Invoke(DispatcherPriority.ContextIdle, new ThreadStart(delegate {
                DataBaseProgress.Visibility = Visibility.Collapsed;
            }));
        }
    }
}
