﻿using System;
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
    /// Interaction logic for TestPage2.xaml
    /// </summary>
    public partial class TestPage2 : Page
    {
        public TestPage2()
        {
            InitializeComponent();

            Debug.WriteLine("page 2");
        }
    }
}
