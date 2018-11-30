using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace GaijinExplorer.Manga
{
    class Chapter
    {
        public string Id { get; set; }
        public List<string> ImageStrings { get; set; }

        //

        public List<BitmapImage> Images { get; set; }
        public List<byte[]> RawImages { get; set; }
    }
}
