using GameLauncher.ViewModels;
using GameLauncher.Views;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using GameLauncher.Properties;
using MaterialDesignThemes.Wpf;
using System.Windows.Data;

namespace GameLauncher
{
    public partial class MainWindow : Window
    {
        public static AddGames DialogAddGames = new AddGames();
        public static EditGames DialogEditGames = new EditGames();
        private BannerViewModel bannerViewModel;
        private ListViewModel listViewModel;
        private PosterViewModel posterViewModel;
        private SettingsViewModel settingsViewModel;
        private PosterView pv = new PosterView();
        private BannerView bv = new BannerView();
        private Views.ListView lv = new Views.ListView();
        public CollectionViewSource cvs;

        public MainWindow()
        {
            LoadSettings();
            InitializeComponent();
            posterViewModel = new PosterViewModel();
            posterViewModel.LoadGames();
            posterViewModel.LoadGenres();
            DataContext = posterViewModel;

            //If games list doesn't exist, create directory and open ag dialog
            if (!File.Exists("./Resources/GamesList.txt"))
            {
                Directory.CreateDirectory("./Resources/");
                OpenAddGameDialog();
                RefreshGames();
            }
        }

        public void OpenAddGameDialog()
        {
            DialogFrame.Visibility = Visibility.Visible;
            DialogFrame.Content = DialogAddGames;
            DialogAddGames.AddGameDialog.IsOpen = true;
        }

        public void OpenEditGameDialog(string guid)
        {
            DialogFrame.Visibility = Visibility.Visible;
            DialogFrame.Content = DialogEditGames;
            DialogEditGames.currentGuid(guid);
            DialogEditGames.EditGameDialog.IsOpen = true;
        }

        private void ApplyGenreFilter_OnClick(object sender, RoutedEventArgs e)
        { 
            string genreToFilter = ((Button)sender).Tag.ToString();
            pv.GenreToFilter(genreToFilter);
            pv.RefreshList2(cvs);
            MenuToggleButton.IsChecked = false; //hide hamburger
        }

        private void BannerButton_OnClick(object sender, RoutedEventArgs e)
        {
            BannerViewActive();
        }

        private void BannerViewActive()
        {
            bannerViewModel = new BannerViewModel();
            bannerViewModel.LoadGames();
            bannerViewModel.LoadGenres();
            DataContext = bannerViewModel;
        }

        private void ListButton_OnClick(object sender, RoutedEventArgs e)
        {
            ListViewActive();
        }

        private void ListViewActive()
        {
            listViewModel = new ListViewModel();
            listViewModel.LoadGames();
            listViewModel.LoadGenres();
            DataContext = listViewModel;
        }

        private void PosterButton_OnClick(object sender, RoutedEventArgs e)
        {
            PosterViewActive();
        }

        private void PosterViewActive()
        {
            //posterViewModel = new PosterViewModel();
            posterViewModel.LoadGames();
            posterViewModel.LoadGenres();
            DataContext = posterViewModel;
        }

        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            SettingsViewActive();
        }

        private void SettingsViewActive()
        {
            settingsViewModel = new SettingsViewModel();
            settingsViewModel.LoadGenres();
            DataContext = settingsViewModel;
        }

        private void OpenAddGameWindow_OnClick(object sender, RoutedEventArgs e)
        {
            OpenAddGameDialog();
            RefreshGames();
        }

        private void RefreshGames_OnClick(object sender, RoutedEventArgs e)
        {
            RefreshGames();
        }

        public void RefreshGames()
        {
            if (DataContext == listViewModel)
            {
                Console.WriteLine("List");
                ListViewActive();
            }
            else if (DataContext == posterViewModel)
            {
                Console.WriteLine("Poster");
                PosterViewActive();
            }
            else if (DataContext == bannerViewModel)
            {
                Console.WriteLine("Banner");
                BannerViewActive();
            }
            else if (DataContext == settingsViewModel)
            {
                Console.WriteLine("Settings");
                SettingsViewActive();
            }
            else
            {
                Console.WriteLine("Nothing");
            }
        }

        public void LoadSettings()
        {
            //Theme Light or Dark
            if (Settings.Default.theme.ToString() == "Dark") { ThemeAssist.SetTheme(Application.Current.MainWindow, BaseTheme.Dark); }
            else if (Settings.Default.theme.ToString() == "Light") { ThemeAssist.SetTheme(Application.Current.MainWindow, BaseTheme.Light); }
        }
    }
}