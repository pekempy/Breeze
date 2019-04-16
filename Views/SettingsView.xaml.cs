using Dsafa.WpfColorPicker;
using GameLauncher.Models;
using GameLauncher.Properties;
using GameLauncher.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace GameLauncher.Views
{
    public partial class SettingsView : UserControl
    {
        string installPath = AppDomain.CurrentDomain.BaseDirectory;
        private ExeSearch es = new ExeSearch();
        private MainWindow MainWindow = ((MainWindow)Application.Current.MainWindow);
        private LoadAllGames lag = new LoadAllGames();
        public string DeletedGenre;

        public void SearchForGames(object sender, RoutedEventArgs e)
        {
            MainWindow.OpenExeSearchDialog();
        }

        public void CleanLibrary(object sender, RoutedEventArgs e)
        {
            Directory.Delete(@"Resources/img/", true);
            File.Delete(@"Resources/GamesList.txt");
            Directory.CreateDirectory(@"/Resources/img/");
            ((MainWindow)Application.Current.MainWindow).RefreshGames();
            ((MainWindow)Application.Current.MainWindow).SettingsViewActive();
        }

        public SettingsView()
        {
            InitializeComponent();
            var converter = new BrushConverter();
            if (Settings.Default.theme == "Dark")
            {
                themeToggle.IsChecked = true;
            }
            if (Settings.Default.gametitles != "")
            {
                TitlesIndicator.Fill = (Brush)converter.ConvertFromString(Settings.Default.gametitles);
            }
            if (Settings.Default.genrecolour != "")
            {
                GenresIndicator.Fill = (Brush)converter.ConvertFromString(Settings.Default.genrecolour);
            }
            if (Settings.Default.launchercolour != "")
            {
                LauncherIndicator.Fill = (Brush)converter.ConvertFromString(Settings.Default.launchercolour);
            }
            if (Settings.Default.fabcolour == "primary")
            {
                FABToggle.IsChecked = true;
            }
            SelectedThemeColour();
        }
        private void DarkModeToggle_Checked(object sender, RoutedEventArgs e)
        {
            ThemeAssist.SetTheme(Application.Current.MainWindow, BaseTheme.Dark);
            Settings.Default.theme = "Dark";
            SaveSettings();
        }
        private void DarkModeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            ThemeAssist.SetTheme(Application.Current.MainWindow, BaseTheme.Light);
            Settings.Default.theme = "Light";
            SaveSettings();
        }
        private void ChangePrimary_OnClick(object sender, RoutedEventArgs e)
        {
            string newPrimaryColour = ((Button)sender).Tag.ToString();
            new PaletteHelper().ReplacePrimaryColor(newPrimaryColour);
            Settings.Default.primary = newPrimaryColour;
            SaveSettings();
            SelectedThemeColour();
        }
        private void ChangeAccent_OnClick(object sender, RoutedEventArgs e)
        {
            string newAccentColour = ((Button)sender).Tag.ToString();
            new PaletteHelper().ReplaceAccentColor(newAccentColour);
            Settings.Default.accent = newAccentColour;
            SaveSettings();
            SelectedThemeColour();
        }
        private void SelectedThemeColour()
        {
            string currentPrimary = Settings.Default.primary.ToString();
            string currentAccent = Settings.Default.accent.ToString();
            DeselectColours();
            TickColour(currentPrimary, currentAccent);
        }
        private void TickColour(string primary, string accent)
        {
            if (primary == "Pink")
            {
                PPinkIcon.Opacity = 100;
            }
            if (primary == "Red")
            {
                PRedIcon.Opacity = 100;
            }
            if (primary == "DeepOrange")
            {
                PDeepOrangeIcon.Opacity = 100;
            }
            if (primary == "Orange")
            {
                POrangeIcon.Opacity = 100;
            }
            if (primary == "Yellow")
            {
                PYellowIcon.Opacity = 100;
            }
            if (primary == "Lime")
            {
                PLimeIcon.Opacity = 100;
            }
            if (primary == "Green")
            {
                PGreenIcon.Opacity = 100;
            }
            if (primary == "Teal")
            {
                PTealIcon.Opacity = 100;
            }
            if (primary == "Cyan")
            {
                PCyanIcon.Opacity = 100;
            }
            if (primary == "Blue")
            {
                PBlueIcon.Opacity = 100;
            }
            if (primary == "Indigo")
            {
                PIndigoIcon.Opacity = 100;
            }
            if (primary == "DeepPurple")
            {
                PDeepPurpleIcon.Opacity = 100;
            }
            if (primary == "Purple")
            {
                PPurpleIcon.Opacity = 100;
            }
            if (primary == "BlueGrey")
            {
                PBlueGreyIcon.Opacity = 100;
            }

            if (accent == "Pink")
            {
                APinkIcon.Opacity = 100;
            }
            if (accent == "Red")
            {
                ARedIcon.Opacity = 100;
            }
            if (accent == "DeepOrange")
            {
                ADeepOrangeIcon.Opacity = 100;
            }
            if (accent == "Orange")
            {
                AOrangeIcon.Opacity = 100;
            }
            if (accent == "Yellow")
            {
                AYellowIcon.Opacity = 100;
            }
            if (accent == "Lime")
            {
                ALimeIcon.Opacity = 100;
            }
            if (accent == "Green")
            {
                AGreenIcon.Opacity = 100;
            }
            if (accent == "Teal")
            {
                ATealIcon.Opacity = 100;
            }
            if (accent == "Cyan")
            {
                ACyanIcon.Opacity = 100;
            }
            if (accent == "Blue")
            {
                ABlueIcon.Opacity = 100;
            }
            if (accent == "Indigo")
            {
                AIndigoIcon.Opacity = 100;
            }
            if (accent == "DeepPurple")
            {
                ADeepPurpleIcon.Opacity = 100;
            }
            if (accent == "Purple")
            {
                APurpleIcon.Opacity = 100;
            }
        }
        private void DeselectColours()
        {
            PPinkIcon.Opacity = 0;
            PRedIcon.Opacity = 0;
            PDeepOrangeIcon.Opacity = 0;
            POrangeIcon.Opacity = 0;
            PYellowIcon.Opacity = 0;
            PLimeIcon.Opacity = 0;
            PGreenIcon.Opacity = 0;
            PTealIcon.Opacity = 0;
            PCyanIcon.Opacity = 0;
            PBlueIcon.Opacity = 0;
            PIndigoIcon.Opacity = 0;
            PDeepPurpleIcon.Opacity = 0;
            PPurpleIcon.Opacity = 0;
            PBlueGreyIcon.Opacity = 0;

            APinkIcon.Opacity = 0;
            ARedIcon.Opacity = 0;
            ADeepOrangeIcon.Opacity = 0;
            AOrangeIcon.Opacity = 0;
            AYellowIcon.Opacity = 0;
            ALimeIcon.Opacity = 0;
            AGreenIcon.Opacity = 0;
            ATealIcon.Opacity = 0;
            ACyanIcon.Opacity = 0;
            ABlueIcon.Opacity = 0;
            AIndigoIcon.Opacity = 0;
            ADeepPurpleIcon.Opacity = 0;
            APurpleIcon.Opacity = 0;
            APurpleIcon.Opacity = 0;
        }

        private void FabColour_Checked(object sender, RoutedEventArgs e)
        {
            Settings.Default.fabcolour = "primary";
            Settings.Default.Save();
            MainWindow.AddGameButton.Style = Application.Current.Resources["MaterialDesignFloatingActionButton"] as Style;
        }
        private void FabColour_Unchecked(object sender, RoutedEventArgs e)
        {
            Settings.Default.fabcolour = "accent";
            Settings.Default.Save();
            MainWindow.AddGameButton.Style = Application.Current.Resources["MaterialDesignFloatingActionAccentButton"] as Style;
        }
        private void ChangeColours(object sender, RoutedEventArgs e)
        {
            string type = ((Button)sender).Tag.ToString();

            var initialColor = Colors.Blue;
            var dialog = new ColorPickerDialog(initialColor);
            var result = dialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                var converter = new BrushConverter();
                var dialogResult = dialog.Color;
                string chosenColour = dialogResult.ToString();
                Brush colour = (Brush)converter.ConvertFromString(chosenColour);
                if (type == "Titles")
                {
                    TitlesIndicator.Fill = colour;
                    Settings.Default.gametitles = chosenColour;
                }
                if (type == "Genres")
                {
                    GenresIndicator.Fill = colour;
                    Settings.Default.genrecolour = chosenColour;
                }
                if (type == "Launchers")
                {
                    LauncherIndicator.Fill = colour;
                    Settings.Default.launchercolour = chosenColour;
                }
                Settings.Default.Save();
                UpdateAllColours(colour, type);
            }
        }
        private void UpdateAllColours(Brush colour, string type)
        {
            if (type == "Launchers")
                MainWindow.UpdateLauncherButtons();
            if (type == "Genres")
                MainWindow.UpdateGenreColours(colour);
            //Poster titles auto-colour when loaded
        }
        private void AddNewGenre_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                TextWriter tsw = new StreamWriter(@"./Resources/GenreList.txt", true);
                tsw.WriteLine(NewGenreName.Text + "|" + Guid.NewGuid());
                tsw.Close();
                NewGenreName.Text = "";
            }
            catch (Exception ex)
            {
                Trace.WriteLine(DateTime.Now + ": AddNewGenre: " + ex.Message);
            }
            lag.LoadGenres();
            MainWindow.RefreshGames();
            CollectionViewSource GenreListCVS = (CollectionViewSource)FindResource("GenreListCVS");
            if (GenreListCVS != null)
                GenreListCVS.View.Refresh();
        }
        private void DeleteGenre_OnClick(object sender, RoutedEventArgs e)
        {
            string genreGuid = ((Button)sender).Tag.ToString();
            var genretext = File.ReadAllLines("./Resources/GenreList.txt", Encoding.UTF8);
            for (int i = 0; i < genretext.Length; i++)
            {
                if (genretext[i].Contains($"{genreGuid}"))
                {
                    try
                    {
                        string[] column = genretext[i].Split('|');
                        DeletedGenre = column[0];
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(DateTime.Now + ": DeleteGenre: " + ex.ToString());
                    }
                }
            }
            if (!File.Exists("./Resources/GamesList.txt"))
            {
            }
            else
            {
                var gametext = File.ReadAllLines("./Resources/GamesList.txt", Encoding.UTF8);
                for (int i2 = 0; i2 < gametext.Length; i2++)
                {
                    if (gametext[i2].Contains(DeletedGenre))
                    {
                        try
                        {
                            string[] column = gametext[i2].Split('|');
                            string genretoedit = column[1];
                            genretoedit = genretoedit.Replace(DeletedGenre + ";", "");
                            string gametoeditguid = column[7];
                            string newGenre = genretoedit.Trim();
                            string NewGameInfo = column[0] + "|" + newGenre + "|" + column[2] + "|" + column[3] + "|" + column[4] + "|" + column[5] + "|" + column[6] + "|" + Guid.NewGuid();
                            ModifyFile.RemoveGameFromFile(gametoeditguid);
                            TextWriter tw = new StreamWriter(@"./Resources/GamesList.txt", true);
                            tw.WriteLine(NewGameInfo);
                            tw.Close();
                        }
                        catch (Exception ex2)
                        {
                            Trace.WriteLine(DateTime.Now + ": DeleteGenre2: " + ex2.ToString());
                        }
                    }
                }
            }
            ModifyFile.RemoveGenreFromFile(((Button)sender).Tag.ToString());
            MainWindow.RefreshGames();
        }
        public void BackupLibrary(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine(DateTime.Now + ": Created backup file");
            File.Delete(@"backup.zip");
            ZipFile.CreateFromDirectory(@"Resources/", installPath + "backup.zip");
        }
        public void RestoreLibrary(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine(DateTime.Now + ": Restored backup file");
            Directory.Delete(@"Resources", true);
            ZipFile.ExtractToDirectory(installPath + "backup.zip", @"Resources/");
            ((MainWindow)Application.Current.MainWindow).RefreshGames();
        }
        public void SaveSettings()
        {
            Settings.Default.Save();
        }

    }

}