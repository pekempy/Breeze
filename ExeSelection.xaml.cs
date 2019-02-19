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
using GameLauncher.Models;

namespace GameLauncher
{
    public partial class ExeSelection : Page
    {
        private MainWindow MainWindow = ((MainWindow)Application.Current.MainWindow);
        private ExeSearch es = new Models.ExeSearch();
        public ExeSelection()
        {
            this.DataContext = es;
            InitializeComponent();
        }

        public void CloseExeSelection(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow)?.CloseExeSearchDialog();
        }
    }
}
