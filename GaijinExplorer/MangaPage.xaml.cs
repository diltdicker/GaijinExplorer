using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
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
    /// Interaction logic for MangaPage.xaml
    /// </summary>
    public partial class MangaPage : Page
    {
        ObservableCollection<Manga.Chapter> ObservableChapters = new ObservableCollection<Manga.Chapter>();
        ObservableCollection<string> ObservableCategories = new ObservableCollection<string>();
        Manga.Manga Manga { get; set; }
        bool favoriteStatus = false;

        public MangaPage()
        {
            InitializeComponent();
            Debug.WriteLine("MangaPage");
        }

        public MangaPage(string id)
        {
            InitializeComponent();
            Debug.WriteLine("MangaPage id: " + id);
            //ChapterList
            //MainWindow.AddToFrameHistory(MainWindow.ExplorerPage.MangaPage);
            CategoriesList.ItemsSource = ObservableCategories;
            ChapterList.ItemsSource = ObservableChapters;
            Task.Run(async () => await Http.HttpMangaEden.GetManga(id, (Manga.Manga manga) =>
            {
                this.Manga = manga;
                //Debug.WriteLine("image string: " + manga.ImageString);
                Task.Run(()=> Database.MangaDAO.CreateMangaAsyncLite(Manga));
                //if (MainWindow.ObservableFavoriteMangas.Where())
                //if (MainWindow.ObservableFavoriteMangas.Where((mangaTmp) => mangaTmp.Id == Manga.Id)
                //MainWindow.ObservableFavoriteMangas.Contains(MainWindow.ObservableFavoriteMangas.Where((mangaTmp) => mangaTmp.Id == Manga.Id).Cast<Manga.Manga>);
                //MainWindow.ObservableFavoriteMangas.Contains(((mangaQuery) => mangaQuery.Id == Manga.Id).Fr);//
                //Manga.Manga tmpManga = MainWindow.ObservableFavoriteMangas.Where(mangaQuery => mangaQuery.Id == Manga.Id).FirstOrDefault();
                if (MainWindow.ObservableFavoriteMangas.Where(mangaQuery => mangaQuery.Id == Manga.Id).FirstOrDefault() is Manga.Manga)
                {
                    Debug.WriteLine("found favorite");
                    favoriteStatus = true;
                }
                else
                {
                    Debug.WriteLine("not found favorite");
                    favoriteStatus = false;
                }
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
                {

                    MangaTitle.Text = manga.Title;
                    MangaImage.Source = new BitmapImage(new Uri(manga.ImageString));
                    MangaArtist.Inlines.Add(manga.Artist);
                    MangaAuthor.Inlines.Add(manga.Author);
                    MangaDescription.Text = manga.Description;
                    MangaStatus.Text = manga.Status.ToString();
                    if (favoriteStatus)
                    {
                        UnFavoriteButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        FavoriteButton.Visibility = Visibility.Visible;
                    }
                }));
                foreach (string category in manga.Categories)
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
                    {
                        ObservableCategories.Add(category);
                    }));
                }
                foreach (Manga.Chapter chapter in manga.Chapters)
                {
                    Manga.Chapter modifiedChapter = Database.ChapterDAO.CreateAndGetChapterLite(chapter, Manga.Id);               // checks if chapter has already been viewed
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new ThreadStart(delegate
                    {
                        ObservableChapters.Add(modifiedChapter);
                    }));
                }
                return true;
            }));
        }

        private void MangaArtist_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Artist: " + Manga.Artist);
        }

        private void MangaAuthor_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Author: " + Manga.Author);
        }

        private void CategoriesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("Category: " + (sender as ListBox).SelectedItem as string);
            if ((sender as ListBox).SelectedItem is string category)
            {
                (sender as ListBox).SelectedIndex = -1;
            }

        }

        private void ChapterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ListView).SelectedItem is Manga.Chapter chapter)
            {
                Debug.WriteLine("Chapter: " + chapter.Title);
                (sender as ListView).SelectedIndex = -1;
                MainWindow.NavigationFrame.Navigate(new ChapterPage(Manga.Title, chapter, Manga.Chapters, 0));
            }
        }


        protected override void OnPreviewMouseWheel(MouseWheelEventArgs e)
        {
            //Debug.WriteLine("P-Wheel");
            if (e.Source.Equals(ScrollParent))
            {
                //Debug.WriteLine("Correct Scroll");
                //base.OnPreviewMouseWheel(e);
            }
            else
            {
                e.Handled = true;
                //Debug.WriteLine("Bad Scroll");
                HandleMouseScroll(ScrollParent, e);
            }
        }

        protected static void HandleMouseScroll(object sender, MouseWheelEventArgs e)
        {
            MouseWheelEventArgs eventArgs = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
            eventArgs.RoutedEvent = UIElement.MouseWheelEvent;
            eventArgs.Source = sender;
            UIElement uIElement = (UIElement)sender;
            uIElement.RaiseEvent(eventArgs);
        }

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            FavoriteButton.Visibility = Visibility.Collapsed;
            UnFavoriteButton.Visibility = Visibility.Visible;
            favoriteStatus = !favoriteStatus;
            MainWindow.AddToFavorites(Manga);
        }

        private void UnFavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            UnFavoriteButton.Visibility = Visibility.Collapsed;
            FavoriteButton.Visibility = Visibility.Visible;
            favoriteStatus = !favoriteStatus;
            MainWindow.RemoveFromFavorites(Manga);
        }
    }
}
