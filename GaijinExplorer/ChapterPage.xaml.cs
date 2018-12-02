using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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
    /// Interaction logic for ChapterPage.xaml
    /// </summary>
    public partial class ChapterPage : Page
    {
        Manga.Chapter Chapter { get; set; }
        List<Manga.Chapter> Chapters { get; set; }
        ObservableCollection<ImageSource> ObservableImages = new ObservableCollection<ImageSource>();
        

        public ChapterPage(Manga.Chapter chapter, List<Manga.Chapter> chapters)
        {
            InitializeComponent();
            MainWindow.AddToFrameHistory(MainWindow.ExplorerPage.ChapterPage);
            Chapter = chapter;
            Chapters = chapters;
            ImageList.ItemsSource = ObservableImages;
            ChapterNumber.Text = Chapter.Number.ToString();

            GetImages();
        }

        public void GetImages()
        {
            Task.Run(async () => await Http.HttpMangaEden.GetChapterImagesIndividually(Chapter.Id, (BitmapImage image) =>
            {
                try
                {
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new ThreadStart(delegate
                    {
                        ObservableImages.Add(image);
                    }));
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.TargetSite);
                    Debug.WriteLine(e.StackTrace);
                }
                return true;
            }));
        }

        private void PreviousChapter_Click(object sender, RoutedEventArgs e)
        {
            // Reverse Order
            int index = Chapters.IndexOf(Chapters.Where(chapter => chapter.Id == Chapter.Id).FirstOrDefault());
            if (index < Chapters.Count - 1)
            {
                MainWindow.Frame.Navigate(new ChapterPage(Chapters[index + 1], Chapters));
            }
        }

        private void NextChapter_Click(object sender, RoutedEventArgs e)
        {
            // Reverse Order
            int index = Chapters.IndexOf(Chapters.Where(chapter => chapter.Id == Chapter.Id).FirstOrDefault());
            if (index > 0)
            {
                MainWindow.Frame.Navigate(new ChapterPage(Chapters[index - 1], Chapters));
            }
        }
    }
}
