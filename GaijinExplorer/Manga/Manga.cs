using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaijinExplorer.Manga
{
    class Manga
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string ImageString { get; set; }
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
    }
}
