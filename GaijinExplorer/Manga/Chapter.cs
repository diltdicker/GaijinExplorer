using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GaijinExplorer.Manga
{
    public class Chapter
    {
        public string Id { get; set; }
        public List<string> ImageStrings { get; set; }
        public string Title { get; set; }
        public double Number { get; set; }
        public long Date { get; set; }
        public enum HasViewed { Viewed, New }
        public HasViewed ViewedStatus { get; set; }

        //

        public List<BitmapImage> Images { get; set; }
        public List<byte[]> RawImages { get; set; }

        public Chapter()
        {
            ViewedStatus = HasViewed.New;
        }

        public void setHasViewed(string hasViewed)
        {
            if (hasViewed.Equals("New"))
            {
                ViewedStatus = HasViewed.New;
            }
            else if (hasViewed.Equals("Viewed"))
            {
                ViewedStatus = HasViewed.Viewed;
            }
            else
            {
                ViewedStatus = HasViewed.New;
            }
        }
    }
}
