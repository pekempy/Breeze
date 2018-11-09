using GameLauncher.Properties;
using GameLauncher.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace GameLauncher.Views
{
    public partial class SettingsView : UserControl
    {
        private MainWindow MainWindow = ((MainWindow)Application.Current.MainWindow);
        private LoadAllGames lag = new LoadAllGames();
        public string DeletedGenre;

        public SettingsView()
        {
            InitializeComponent();
            var converter = new System.Windows.Media.BrushConverter();
            if (Settings.Default.theme == "Dark") { themeToggle.IsChecked = true; }
            if (Settings.Default.primarylight != null)
            {
                ColorPrimaryLight.Text = Settings.Default.primarylight;
                ColorPrimaryLightRect.Fill = (Brush)converter.ConvertFromString("#" + ColorPrimaryLight.Text);
            }
            if (Settings.Default.accentlight != null)
            {
                ColorAccentLight.Text = Settings.Default.accentlight;
                ColorAccentLightRect.Fill = (Brush)converter.ConvertFromString("#" + ColorAccentLight.Text);
            }
            if (Settings.Default.primarydark != null)
            {
                ColorPrimaryDark.Text = Settings.Default.primarydark;
                ColorPrimaryDarkRect.Fill = (Brush)converter.ConvertFromString("#" + ColorPrimaryDark.Text);
            }
            if (Settings.Default.accentdark != null)
            {
                ColorAccentDark.Text = Settings.Default.accentdark;
                ColorAccentDarkRect.Fill = (Brush)converter.ConvertFromString("#" + ColorAccentDark.Text);
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

        private void ChangePrimaryLight(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString("#" + ColorPrimaryLight.Text);
                Properties.Settings.Default.primarylight = ColorPrimaryLight.Text;
                SaveSettings();
                ColorPrimaryLightRect.Fill = brush;
            }
        }

        private void ChangeAccentLight(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString("#" + ColorAccentLight.Text);
                Properties.Settings.Default.accentlight = ColorAccentLight.Text;
                SaveSettings();
                ColorAccentLightRect.Fill = brush;
            }
        }

        private void ChangePrimaryDark(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString("#" + ColorPrimaryDark.Text);
                Properties.Settings.Default.primarydark = ColorPrimaryDark.Text;
                SaveSettings();
                ColorPrimaryDarkRect.Fill = brush;
            }
        }

        private void ChangeAccentDark(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var converter = new System.Windows.Media.BrushConverter();
                var brush = (Brush)converter.ConvertFromString("#" + ColorAccentDark.Text);
                Properties.Settings.Default.accentdark = ColorAccentDark.Text;
                SaveSettings();
                ColorAccentDarkRect.Fill = brush;
            }
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
            MainWindow.RefreshGames();
            CollectionViewSource GenreListCVS = (CollectionViewSource)FindResource("GenreListCVS");
            if (GenreListCVS != null)
                GenreListCVS.View.Refresh();
        }

        private void DeleteGenre_OnClick(object sender, RoutedEventArgs e)
        {  //Check Genre file for the name of the genre to remove
            string genreGuid = ((Button)sender).Tag.ToString();
            var genretext = File.ReadAllLines("./Resources/GenreList.txt", Encoding.UTF8);
            for (int i = 0; i < genretext.Length; i++)
            {
                if (genretext[i].Contains($"{genreGuid}"))
                {
                    try
                    {
                        Console.WriteLine(genretext[i]);
                        string[] column = genretext[i].Split('|');
                        DeletedGenre = column[0];
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
            //Check gameslist, and remove that genre from any listings
            var gametext = File.ReadAllLines("./Resources/GamesList.txt", Encoding.UTF8);
            for (int i2 = 0; i2 < gametext.Length; i2++)
            {
                if (gametext[i2].Contains(DeletedGenre))
                {
                    try
                    {
                        Console.WriteLine(gametext[i2]);
                        string[] column = gametext[i2].Split('|');
                        string genretoedit = column[1];
                        genretoedit = genretoedit.Replace(DeletedGenre + ";", "");
                        string gametoeditguid = column[7];
                        string newGenre = genretoedit.Trim();
                        string NewGameInfo = column[0] + "|" + newGenre + "|" + column[2] + "|" + column[3] + "|" + column[4] + "|" + column[5] + "|" + column[6] + "|" + Guid.NewGuid();
                        Console.WriteLine(NewGameInfo);
                        ModifyFile.RemoveGameFromFile(gametoeditguid);
                        TextWriter tw = new StreamWriter(@"./Resources/GamesList.txt", true);
                        tw.WriteLine(NewGameInfo);
                        tw.Close();
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine(ex2.ToString());
                    }
                }
            }
            ModifyFile.RemoveGenreFromFile(((Button)sender).Tag.ToString());
            MainWindow.RefreshGames();
        }

        public void SaveSettings()
        {
            Properties.Settings.Default.Save();
        }
    }
}