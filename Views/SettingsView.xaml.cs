using MaterialDesignThemes.Wpf;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Configuration;

namespace GameLauncher.Views
{
    public partial class SettingsView : UserControl
    {
        string theme = "";
        public SettingsView()
        {
            InitializeComponent();
        }

        private void DarkModeToggle_Checked(object sender, RoutedEventArgs e)
        {
            ThemeAssist.SetTheme(Application.Current.MainWindow, BaseTheme.Dark);
            theme = ConfigurationManager.AppSettings["Theme"];
        }

        private void DarkModeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            ThemeAssist.SetTheme(Application.Current.MainWindow, BaseTheme.Light);
        }
    }
}