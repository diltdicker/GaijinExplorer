using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GaijinExplorer.Manga
{
    class Manga
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string ImageString { get; set; }
        //public BitmapImage Image { get; set; }
        public long Hits { get; set; }
        public long LastDate { get; set; }
        public enum MangaStatus { Suspended, Ongoing, Completed };
        public MangaStatus Status { get; set; }
        public List<string> Categories { get; set; }

        //

        public string Description { get; set; }
        public string Artist { get; set; }
        public string Author { get; set; }
        public List<string> ChapterIds { get; set; }

        public void SetStatus(int status)
        {
            switch (status)
            {
                case 0:
                    Status = MangaStatus.Suspended;
                    break;
                case 1:
                    Status = MangaStatus.Ongoing;
                    break;
                case 2:
                    Status = MangaStatus.Completed;
                    break;
                default:
                    Status = MangaStatus.Ongoing;
                    break;
            }
        }
    }
}
