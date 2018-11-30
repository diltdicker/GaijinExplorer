using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GaijinExplorer.Http
{
    class HttpHelper
    {
        const string IMAGE_URL = "";
        const string MANGA_URL = "";

        static void TestFunc_1()
        {
            Debug.WriteLine("test func 1");
        }

        public static async void TestFunc_2()
        {
            await Task.Delay(1000);
            Debug.WriteLine("test func 2");
        }

        public static async Task<BitmapImage> TestGetImage()
        {
            Uri uri = new Uri("https://cdn.mangaeden.com/mangasimg/b4/b437ee9d784776b41b3b9dc3c8992d4ae8ccec2ea7548cdcdb1b9f72.png");
            HttpClient client = new HttpClient();
            byte[] imageArray = await client.GetByteArrayAsync(uri);
            BitmapImage image = new BitmapImage();
            MemoryStream stream = new MemoryStream(imageArray);
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

        /// <summary>
        /// Used for the Home Page, Gets All Manga Titles.
        /// </summary>
        /// <returns>List of minimally filled Manga Objects (Id, Title, ImageString, Hits, LastDate)</returns>
        public static async Task<List<Manga.Manga>> GetMangaTitlesAsync()
        {
            List<Manga.Manga> mangas = new List<Manga.Manga>();
            await Task.Delay(1000);
            return mangas;
        }
    }
}
