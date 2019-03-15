using GameLauncher.Properties;
using MaterialDesignThemes.Wpf;
using System;
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

namespace GameLauncher.Views
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class Loading : Page
    {
        public static Loading ld;
        public bool isLoading;
        public Loading()
        {
            InitializeComponent();
            if (Settings.Default.theme.ToString() == "Dark")
            {
                ThemeAssist.SetTheme(this, BaseTheme.Dark);
            }
            else if (Settings.Default.theme.ToString() == "Light")
            {
                ThemeAssist.SetTheme(this, BaseTheme.Light);
            }
            try
            {
                ld.ProgressGrid.Height = (Application.Current.MainWindow).ActualHeight * 0.9;
                ld.ProgressGrid.Width = (Application.Current.MainWindow).ActualWidth * 0.9;
                ld.LoadingSpinner.Height = (Application.Current.MainWindow).ActualHeight * 0.6;
                ld.LoadingSpinner.Width = (Application.Current.MainWindow).ActualWidth * 0.6;
            }
            catch { }
        }
        public static void ChangeWindowSize(double height, double width)
        {
            ld.ProgressGrid.Height = (Application.Current.MainWindow).ActualHeight * 0.9;
            ld.ProgressGrid.Width = (Application.Current.MainWindow).ActualWidth * 0.9;
            ld.LoadingSpinner.Height = (Application.Current.MainWindow).ActualHeight * 0.6;
            ld.LoadingSpinner.Width = (Application.Current.MainWindow).ActualWidth * 0.6;
        }
    }
}
