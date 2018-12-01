using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Explorer.xaml
    /// </summary>
    public partial class Explorer : Page
    {
        ObservableCollection<Manga.Manga> ObservableMangas = new ObservableCollection<Manga.Manga>();
        List<Manga.Manga> backupMangaList = new List<Manga.Manga>();
        List<Manga.Manga> displayedMangaList = new List<Manga.Manga>();

        public Explorer()
        {
            InitializeComponent();
            MangaListBox.ItemsSource = ObservableMangas;
            DataContext = this;


            //SetImage();
            
            Debug.WriteLine("Explorer");
            
            if (App.FIRST_MANGA_GRAB)
            {
                //UpdateMangaDB();
                App.FIRST_MANGA_GRAB = false;
            }
            GetAllMangaTitles();
        }


        public void UpdateMangaDB()
        {
            Task.Run(async () => await Http.HttpMangaEden.GetAllMangaTitlesAsync((List<Manga.Manga> mangas) =>
            {
                Task.Run(() =>
                {
                    // Send Manga to DB
                });
                return true;
            }));
        }

        /// <summary>
        /// 
        /// </summary>
        public void GetAllMangaTitles()
        {
            //App.CancellationToken = new CancellationTokenSource();
            //Task.Run(async () => await Http.HttpMangaEden.GetIndividualMangaTitlesAsync((Manga.Manga manga) =>
            //{
            //    if (!App.CancellationToken.IsCancellationRequested)
            //    {
            //        try
            //        {
            //            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
            //            {
            //                // UI Thread Execution
            //                ObservableMangas.Add(manga);
            //            }));
            //        }
            //        catch (Exception e)
            //        {
            //            Debug.WriteLine(e.TargetSite);
            //            Debug.WriteLine(e.StackTrace);
            //        }
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }
            //}), App.CancellationToken.Token);
            App.CancellationToken = new CancellationTokenSource();
            Task.Run(async () => await Http.HttpMangaEden.GetAllMangaTitlesAsync((List<Manga.Manga> mangas) =>
            {
                if (!App.CancellationToken.IsCancellationRequested)
                {
                    Random random = new Random();
                    List<Manga.Manga> displayedMangas = new List<Manga.Manga>();
                    while (displayedMangas.Count < 102)
                    {
                        int i = random.Next(0, mangas.Count);
                        displayedMangas.Add(mangas[i]);
                        mangas.RemoveAt(i);
                    }
                    foreach (Manga.Manga manga in displayedMangas)
                    {
                        try
                        {
                            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                            {
                                // UI Thread Execution
                                ObservableMangas.Add(manga);
                            }));
                        }
                        catch (Exception e)
                        {
                            Debug.WriteLine(e.TargetSite);
                            Debug.WriteLine(e.StackTrace);
                        }
                    }
                    backupMangaList = mangas;
                    displayedMangaList = displayedMangas;
                }
                return true;
            }), App.CancellationToken.Token);
        }

        private void MangaListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.CancellationToken.Cancel();
            Manga.Manga manga = (sender as ListBox).SelectedItem as Manga.Manga;
            if (manga != null)
            {
                string id = String.Copy(manga.Id);
                MangaListBox.SelectedIndex = -1;
                ObservableMangas.Clear();
                MainWindow.NavigationService.Navigate(new Uri("MangaPage.xaml", UriKind.RelativeOrAbsolute), id);
            }
        }

        private void DiscoverButton_Click(object sender, RoutedEventArgs e)
        {
            foreach(Manga.Manga manga in displayedMangaList)
            {
                backupMangaList.Add(manga);
            }
            displayedMangaList.Clear();
            Random random = new Random();
            while (displayedMangaList.Count < 102)
            {
                int i = random.Next(0, backupMangaList.Count);
                displayedMangaList.Add(backupMangaList[i]);
                backupMangaList.RemoveAt(i);
            }
            ObservableMangas.Clear();
            foreach(Manga.Manga manga in displayedMangaList)
            {
                ObservableMangas.Add(manga);
            }
        }

        private void PopularButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Manga.Manga manga in displayedMangaList)
            {
                backupMangaList.Add(manga);
            }
            displayedMangaList.Clear();
            backupMangaList = backupMangaList.OrderByDescending(manga => manga.Hits).ToList();
            int i = 0;
            while (displayedMangaList.Count < 102)
            {
                displayedMangaList.Add(backupMangaList[i]);
                backupMangaList.RemoveAt(i);
            }
            ObservableMangas.Clear();
            foreach (Manga.Manga manga in displayedMangaList)
            {
                ObservableMangas.Add(manga);
            }
        }

        private void UpdatedButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (Manga.Manga manga in displayedMangaList)
            {
                backupMangaList.Add(manga);
            }
            displayedMangaList.Clear();
            backupMangaList = backupMangaList.OrderByDescending(manga => manga.LastDate).ToList();
            int i = 0;
            while (displayedMangaList.Count < 102)
            {
                Debug.WriteLine("last date: " + backupMangaList[i].LastDate);
                displayedMangaList.Add(backupMangaList[i]);
                backupMangaList.RemoveAt(i);
            }
            ObservableMangas.Clear();
            foreach (Manga.Manga manga in displayedMangaList)
            {
                ObservableMangas.Add(manga);
            }
        }
    }
}
