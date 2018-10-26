using GameLauncher.Properties;
using MaterialDesignThemes.Wpf;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;

namespace GameLauncher.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
            if (Settings.Default.theme == "Dark")
            {
                themeToggle.IsChecked = true;
            }
        }

        private void DarkModeToggle_Checked(object sender, RoutedEventArgs e)
        {
            ThemeAssist.SetTheme(Application.Current.MainWindow, BaseTheme.Dark);
            Properties.Settings.Default.theme = "Dark";
            SaveSettings();
        }

        private void DarkModeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            ThemeAssist.SetTheme(Application.Current.MainWindow, BaseTheme.Light);
            Properties.Settings.Default.theme = "Light";
            SaveSettings();
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }
    }
}