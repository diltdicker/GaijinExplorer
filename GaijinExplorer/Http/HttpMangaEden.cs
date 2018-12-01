using Newtonsoft.Json;
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
    class HttpMangaEden
    {
        const string IMAGE_URL = "https://cdn.mangaeden.com/mangasimg/";
        const string MANGA_URL = "https://www.mangaeden.com/api";

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
        public static async Task GetIndividualMangaTitlesAsync(Func<Manga.Manga, bool> callback)
        {
            //List<Manga.Manga> mangas = new List<Manga.Manga>();
            Uri uri = new Uri(MANGA_URL + "/list/0/");
            HttpClient client = new HttpClient();
            dynamic json = null;
            try
            {
                string jsonString = await client.GetStringAsync(uri);
                json = JsonConvert.DeserializeObject(jsonString);
            } catch (Exception e)
            {
                Debug.WriteLine(e.TargetSite);
                Debug.WriteLine(e.StackTrace);
            }
            if (json != null)
            {
                for( int i = 0; i < json.manga.Count; i++)
                {
                    try
                    {
                        Manga.Manga manga = new Manga.Manga
                        {
                            Id = json.manga[i].i,
                            Title = json.manga[i].t,
                            ImageString = json.manga[i].im,
                            Hits = json.manga[i].h,
                            Categories = new List<string>()
                        };
                        manga.SetStatus((int) json.manga[i].s);
                        if (manga.ImageString != null)
                        {
                            //Debug.WriteLine("valid image [ " + manga.ImageString + " ]");
                            //manga.Image = new BitmapImage(new Uri(IMAGE_URL + manga.ImageString));
                            manga.ImageString = IMAGE_URL + manga.ImageString;
                        }
                        else
                        {
                            manga.ImageString = IMAGE_URL;
                            //manga.Image = new BitmapImage();
                        }
                        if (json.manga[i].ld != null)
                        {
                            manga.LastDate = (long)json.manga[i].ld;
                        }
                        for (int k = 0; k < json.manga[i].c.Count; k++)
                        {
                            string category = json.manga[i].c[k];
                            //Debug.WriteLine("category [ " + category + " ]");
                            manga.Categories.Add(category);
                        }
                        //mangas.Add(manga);
                        if (!callback.Invoke(manga))
                        {
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.TargetSite);
                        Debug.WriteLine(e.StackTrace);
                    }
                }
            }
            Debug.WriteLine("Done Getting Manga 1");
            //callback.Invoke(mangas);
        }

        public static async Task GetAllMangaTitlesAsync(Func<List<Manga.Manga>, bool> callback)
        {
            List<Manga.Manga> mangas = new List<Manga.Manga>();
            Uri uri = new Uri(MANGA_URL + "/list/0/");
            HttpClient client = new HttpClient();
            dynamic json = null;
            try
            {
                string jsonString = await client.GetStringAsync(uri);
                json = JsonConvert.DeserializeObject(jsonString);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.TargetSite);
                Debug.WriteLine(e.StackTrace);
            }
            if (json != null)
            {
                for (int i = 0; i < json.manga.Count; i++)
                {
                    try
                    {
                        Manga.Manga manga = new Manga.Manga
                        {
                            Id = json.manga[i].i,
                            Title = json.manga[i].t,
                            ImageString = json.manga[i].im,
                            Hits = json.manga[i].h,
                            Categories = new List<string>()
                        };
                        manga.SetStatus((int) json.manga[i].s);
                        if (manga.ImageString != null)
                        {
                            //Debug.WriteLine("valid image [ " + manga.ImageString + " ]");
                            //manga.Image = new BitmapImage(new Uri(IMAGE_URL + manga.ImageString));
                            manga.ImageString = IMAGE_URL + manga.ImageString;
                        }
                        else
                        {
                            manga.ImageString = IMAGE_URL;
                            //manga.Image = new BitmapImage();
                        }
                        if (json.manga[i].ld != null)
                        {
                            manga.LastDate = (long)json.manga[i].ld;
                        }
                        for (int k = 0; k < json.manga[i].c.Count; k++)
                        {
                            string category = json.manga[i].c[k];
                            //Debug.WriteLine("category [ " + category + " ]");
                            manga.Categories.Add(category);
                        }
                        mangas.Add(manga);
                        //if (!callback.Invoke(manga))
                        //{
                        //    break;
                        //}
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.TargetSite);
                        Debug.WriteLine(e.StackTrace);
                    }
                }
            }
            Debug.WriteLine("Done Getting Manga 1");
            callback.Invoke(mangas);
        }
    }
}
