using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GaijinExplorer.Database
{
    class Sanitize
    {
        public static string SanitizeString(string suspect, string backup)
        {
            return suspect;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="suspect"></param>
        /// <returns>Returns Null if suspect is dangerous</returns>
        public static string SanitizeString(string suspect)
        {
            return suspect;
        }
    }
}
