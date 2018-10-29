using GameLauncher.Properties;
using GameLauncher.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace GameLauncher.Views
{
    public partial class SettingsView : UserControl
    {
        private LoadAllGames lag = new LoadAllGames();

        public SettingsView()
        {
            lag.LoadGenres();
            InitializeComponent();
            if (Settings.Default.theme == "Dark") { themeToggle.IsChecked = true; }
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

        private void AddNewGenre_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                TextWriter tsw = new StreamWriter(@"./Resources/GenreList.txt", true);
                tsw.WriteLine(NewGenreName.Text + "|False|" + Guid.NewGuid());
                tsw.Close();
                NewGenreName.Text = "";
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            lag.LoadGenres();
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }
    }
}