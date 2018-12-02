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
    /// Interaction logic for ChapterPage.xaml
    /// </summary>
    public partial class ChapterPage : Page
    {
        Manga.Chapter Chapter { get; set; }
        List<Manga.Chapter> Chapters { get; set; }

        public ChapterPage(string id, List<Manga.Chapter> chapters)
        {
            InitializeComponent();
            MainWindow.AddToFrameHistory(MainWindow.ExplorerPage.ChapterPage);
            Chapters = chapters;
            Debug.WriteLine("chapter id = " + id);
            Debug.WriteLine("chapters size = " + Chapters.Count);
        }
    }
}
