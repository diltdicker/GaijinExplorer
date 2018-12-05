using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for SearchPage.xaml
    /// </summary>
    public partial class SearchPage : Page
    {
        public enum SearchType { Title, Category, Artist, Author }
        ObservableCollection<Manga.Manga> ObservableResults = new ObservableCollection<Manga.Manga>();

        public SearchPage()
        {
            InitializeComponent();
        }

        public SearchPage(SearchType type, string query)
        {
            InitializeComponent();
            if (type == SearchType.Artist)
            {

            }
            else if (type == SearchType.Author)
            {

            }
            else if (type == SearchType.Category)
            {

            }
            else
            {

            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void QueryBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
