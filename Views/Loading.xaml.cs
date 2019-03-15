using System;
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

namespace GameLauncher.Views
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : UserControl
    {
        public static Loading ld;
        public bool isLoading;
        public Loading()
        {
            InitializeComponent();
            try
            {
                ld.ProgressGrid.Height = (Application.Current.MainWindow).ActualHeight * 0.5;
                ld.ProgressGrid.Width = (Application.Current.MainWindow).ActualWidth * 0.5;
            }
            catch { }
        }
        public static void ChangeWindowSize(double height, double width)
        {
            ld.ProgressGrid.Height = ((MainWindow)Application.Current.MainWindow).ActualHeight * 0.8;
            ld.ProgressGrid.Width = ((MainWindow)Application.Current.MainWindow).ActualWidth * 0.8;
        }
    }
}
