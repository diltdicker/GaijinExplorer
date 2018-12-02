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
    /// Interaction logic for MangaPage.xaml
    /// </summary>
    public partial class MangaPage : Page
    {
        ObservableCollection<Manga.Chapter> ObservableChapters = new ObservableCollection<Manga.Chapter>();
        ObservableCollection<string> ObservableCategories = new ObservableCollection<string>();
        Manga.Manga Manga { get; set; }

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
            MainWindow.AddToFrameHistory(MainWindow.ExplorerPage.MangaPage);
            CategoriesList.ItemsSource = ObservableCategories;
            ChapterList.ItemsSource = ObservableChapters;
            Task.Run(async () => await Http.HttpMangaEden.GetManga(id, (Manga.Manga manga) =>
            {
                this.Manga = manga;
                Debug.WriteLine("image string: " + manga.ImageString);
                
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                {

                    MangaTitle.Text = manga.Title;
                    MangaImage.Source = new BitmapImage(new Uri(manga.ImageString));
                    MangaArtist.Inlines.Add(manga.Artist);
                    MangaAuthor.Inlines.Add(manga.Author);
                    MangaDescription.Text = manga.Description;
                    
                }));
                foreach (string category in manga.Categories)
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                    {
                        ObservableCategories.Add(category);
                    }));
                }
                foreach (Manga.Chapter chapter in manga.Chapters)
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                    {
                        ObservableChapters.Add(chapter);
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
        }

        private void ChapterList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("Chapter: " + ((sender as ListView).SelectedItem as Manga.Chapter).Title);
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

    }
}
