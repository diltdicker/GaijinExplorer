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
    /// Interaction logic for SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        public enum SearchType { Title, Category, Artist, Author }
        ObservableCollection<Manga.Manga> ObservableResults = new ObservableCollection<Manga.Manga>();

        public SearchPage()
        {
            InitializeComponent();
            ResultList.ItemsSource = ObservableResults;
            
        }

        public SearchPage(SearchType type, string query)
        {
            InitializeComponent();
            ResultList.ItemsSource = ObservableResults;
            if (type == SearchType.Artist)
            {
                Task.Run(async () =>
                {
                    List<Manga.Manga> mangas = await Database.MangaDAO.GetMangasFromArtistAsync(query);
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
                    {
                        ObservableResults.Clear();
                    }));
                    foreach (Manga.Manga manga in mangas)
                    {
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
                        {
                            ObservableResults.Add(manga);
                        }));
                    }
                });
            }
            else if (type == SearchType.Author)
            {
                Task.Run(async () =>
                {
                    List<Manga.Manga> mangas = await Database.MangaDAO.GetMangasFromAuthorAsync(query);
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
                    {
                        ObservableResults.Clear();
                    }));
                    foreach (Manga.Manga manga in mangas)
                    {
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
                        {
                            ObservableResults.Add(manga);
                        }));
                    }
                });
            }
            else if (type == SearchType.Category)
            {
                Task.Run(async () =>
                {
                    List<Manga.Manga> mangas = await Database.MangaDAO.GetMangasFromCategoryAsync(query);
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
                    {
                        ObservableResults.Clear();
                    }));
                    foreach(Manga.Manga manga in mangas)
                    {
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
                        {
                            ObservableResults.Add(manga);
                        }));
                    }
                });
            }
            else
            {
                Task.Run(async () =>
                {
                    List<Manga.Manga> mangas = await Database.MangaDAO.GetMangasFromTitleAsync(query);
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
                    {
                        ObservableResults.Clear();
                    }));
                    foreach (Manga.Manga manga in mangas)
                    {
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
                        {
                            ObservableResults.Add(manga);
                        }));
                    }
                });
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void QueryBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((sender as TextBox).Text is string titleString)
            {
                if (titleString.Length > 2)
                {
                    //ObservableResults.Clear();
                    //Debug.WriteLine("start search");
                    Task.Run(async () =>
                    {
                        List<Manga.Manga> mangas = await Database.MangaDAO.GetMangasFromTitleAsync(titleString);
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
                        {
                            ObservableResults.Clear();
                        }));
                        foreach (Manga.Manga manga in mangas)
                        {
                            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
                            {
                                ObservableResults.Add(manga);
                            }));
                        }
                    });
                }
                else
                {
                    ObservableResults.Clear();
                }
            }
        }

        private void ResultList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListBox).SelectedItem is Manga.Manga manga)
            {
                (sender as ListBox).SelectedItem = -1;
                MainWindow.navigationService.Navigate(new MangaPage(manga.Id));
            }
        }
    }
}
