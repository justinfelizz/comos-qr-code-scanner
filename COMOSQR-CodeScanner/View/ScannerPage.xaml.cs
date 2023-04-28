﻿using System;
using System.Collections.Generic;
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

namespace COMOSQR_CodeScanner.View
{
    /// <summary>
    /// Interaktionslogik für ScannerPage.xaml
    /// </summary>
    public partial class ScannerPage : Page
    {
        public ScannerPage()
        {
            var vm = new ViewModel.ScannerViewModel();
            vm.Page = this;
            this.DataContext = vm;
            InitializeComponent();
        }
    }
}
