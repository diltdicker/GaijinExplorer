using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace GaijinExplorer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool FIRST_MANGA_GRAB = true;
        public static CancellationTokenSource CancellationToken { get; set; }

        public App()
        {
            Debug.WriteLine("start of program");
        }
    }
}
