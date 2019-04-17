using GameLauncher.Properties;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GameLauncher.Views
{
    /// <summary>
    /// Interaction logic for Loading.xaml
    /// </summary>
    public partial class LoadingProgress : Page
    {
        public static LoadingProgress ld;
        public bool isLoading;
        public LoadingProgress()
        {
            var converter = new BrushConverter();
            var white = (Brush)converter.ConvertFromString("#FFFFFF");
            var black = (Brush)converter.ConvertFromString("#000000");
            InitializeComponent();
            if (Settings.Default.theme.ToString() == "Dark")
            {
                ThemeAssist.SetTheme(this, BaseTheme.Dark);
                NumberLeft.Foreground = white;
            }
            else if (Settings.Default.theme.ToString() == "Light")
            {
                ThemeAssist.SetTheme(this, BaseTheme.Light);
                NumberLeft.Foreground = black;
            }
            try
            {
                ld.ProgressGrid.Height = (Application.Current.MainWindow).ActualHeight * 0.9;
                ld.ProgressGrid.Width = (Application.Current.MainWindow).ActualWidth * 0.9;
                ld.ProgressBar.Height = (Application.Current.MainWindow).ActualHeight * 0.6;
                ld.ProgressBar.Width = (Application.Current.MainWindow).ActualWidth * 0.6;
            }
            catch { }
        }
        public static void ChangeWindowSize(double height, double width)
        {
            ld.ProgressGrid.Height = (Application.Current.MainWindow).ActualHeight * 0.9;
            ld.ProgressGrid.Width = (Application.Current.MainWindow).ActualWidth * 0.9;
            ld.ProgressBar.Height = (Application.Current.MainWindow).ActualHeight * 0.6;
            ld.ProgressBar.Width = (Application.Current.MainWindow).ActualWidth * 0.6;
        }
    }
}
