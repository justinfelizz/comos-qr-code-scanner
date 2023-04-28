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
    /// Interaktionslogik für ProjectPage.xaml
    /// </summary>
    public partial class ProjectPage : Page
    {
        public ProjectPage()
        {
            var vm = new ViewModel.ProjectViewModel();
            vm.Page = this;
            this.DataContext = vm;  
            InitializeComponent();
        }
    }
}
