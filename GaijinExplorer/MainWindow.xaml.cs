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
using System.Collections.ObjectModel;

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

        public static ObservableCollection<Manga.Manga> ObservableFavoriteMangas = new ObservableCollection<Manga.Manga>();

        public MainWindow()
        {
            InitializeComponent();
            FavoriteMangaList.ItemsSource = ObservableFavoriteMangas;
            

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
            Task.Run(() => RefreshFavorites());
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

        private void FavoriteMangaList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListView).SelectedItem is Manga.Manga manga)
            {
                NavigationFrame.Navigate(new MangaPage(manga.Id));
            }
        }

        public static void AddToFavorites(Manga.Manga manga)
        {
            Task.Run(async () =>
            {
                Debug.WriteLine("add start");
                await Database.MangaDAO.AddMangaToFavoritesAsync(manga.Id);
                Debug.WriteLine("after add");
                await RefreshFavorites();
                Debug.WriteLine("add finish");
            });
        }

        public static void RemoveFromFavorites(Manga.Manga manga)
        {
            Task.Run(async () =>
            {
                await Database.MangaDAO.RemoveMangaFromFavoritesAsync(manga.Id);
                await RefreshFavorites();
            });
        }

        private static async Task RefreshFavorites()
        {
            List<Manga.Manga> mangas = await Database.MangaDAO.GetFavoriteMangasAsyncLite();
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
            {
                ObservableFavoriteMangas.Clear();
            }));
            foreach (Manga.Manga manga in mangas)
            {
                Debug.WriteLine("adding manga: " + manga.Title);
                if (manga.Title.Length > 30)
                {
                    manga.Title = manga.Title.Substring(0, 30) + "...";
                }
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
                {
                    ObservableFavoriteMangas.Add(manga);
                }));
            }
        }
    }
}
