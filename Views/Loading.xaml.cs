using GameLauncher.Properties;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

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
