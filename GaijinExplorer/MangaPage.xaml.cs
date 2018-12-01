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
    /// Interaction logic for MangaPage.xaml
    /// </summary>
    public partial class MangaPage : Page
    {
        public MangaPage()
        {
            InitializeComponent();
            Debug.WriteLine("MangaPage");
        }

        public MangaPage(string id)
        {
            InitializeComponent();
            Debug.WriteLine("MangaPage id: " + id);
        }
    }
}
