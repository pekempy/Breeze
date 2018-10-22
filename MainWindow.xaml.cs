using GameLauncher.ViewModels;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using GameLauncher.Views;

namespace GameLauncher
{
    public partial class MainWindow : Window
    {
        #region ViewModels are at class level to be reused

        private SettingsViewModel settingsViewModel;
        private PosterViewModel posterViewModel;
        private ListViewModel listViewModel;
        private BannerViewModel bannerViewModel;
        private AddGame ag;
        public string theme;

        #endregion ViewModels are at class level to be reused

        public MainWindow()
        {
            InitializeComponent();

            #region Instantiate new displays only ONCE

            //lag = new LoadAllGames();
            ag = new AddGame();

            #endregion Instantiate new displays only ONCE

            #region default view

            posterViewModel = new PosterViewModel();
            posterViewModel.LoadGames();
            DataContext = posterViewModel;
            ThemeAssist.SetTheme(this, BaseTheme.Light);
            theme = "dark";

            #endregion default view

            #region if game file doesn't exist, create dir + open ag dialog

            //If games list doesn't exist, create directory and open ag dialog
            if (!File.Exists("./Resources/GamesList.txt"))
            {
                Directory.CreateDirectory("./Resources/");
                ag.Show();
                RefreshGames();
            }

            #endregion if game file doesn't exist, create dir + open ag dialog
        }

        private void ThemeSwitch_OnClick(object sender, RoutedEventArgs e)
        {
            if (theme == "dark")
            {
                SwitchThemeLight();
            }
            else if (theme == "light")
            {
                SwitchThemeDark();
            }
        }

        public void SwitchThemeLight()
        {
            ThemeAssist.SetTheme(this, BaseTheme.Light);
            theme = "light";
        }

        public void SwitchThemeDark()
        {
            ThemeAssist.SetTheme(this, BaseTheme.Dark);
            theme = "dark";
        }

        #region Until we add StayOpen glag to drawer, this helps w/ scrollbar

        private void UIElement_OnPreviewLeftMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }
            MenuToggleButton.IsChecked = false;
        }

        #endregion Until we add StayOpen glag to drawer, this helps w/ scrollbar

        #region Refresh Games method

        public void RefreshGames()
        {
            if (DataContext == settingsViewModel)
            {
                Console.WriteLine("Grid");
                GridViewActive();
            }
            else if (DataContext == listViewModel)
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
            else
            {
                Console.WriteLine("Nothing");
            }
        }

        #endregion Refresh Games method

        #region Open AddGameWindow with FAB

        private void openAddGameWindow_OnClick(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.5;
            ag = new AddGame();
            ag.Owner = this;
            ag.ShowDialog();
            RefreshGames();
            this.Opacity = 100;
        }

        #endregion Open AddGameWindow with FAB

        #region Change DataContext with buttons in overflow

        private void GridButton_OnClick(object sender, RoutedEventArgs e)
        {
            GridViewActive();
        }

        private void PosterButton_OnClick(object sender, RoutedEventArgs e)
        {
            PosterViewActive();
        }

        private void BannerButton_OnClick(object sender, RoutedEventArgs e)
        {
            BannerViewActive();
        }

        private void ListButton_OnClick(object sender, RoutedEventArgs e)
        {
            ListViewActive();
        }

        #endregion Change DataContext with buttons in overflow

        private void ListViewActive()
        {
            listViewModel = new ListViewModel();
            listViewModel.LoadGames();
            DataContext = listViewModel;
        }

        private void GridViewActive()
        {
            settingsViewModel = new SettingsViewModel();
            DataContext = settingsViewModel;
        }

        private void PosterViewActive()
        {
            posterViewModel = new PosterViewModel();
            posterViewModel.LoadGames();
            DataContext = posterViewModel;
        }

        private void BannerViewActive()
        {
            bannerViewModel = new BannerViewModel();
            bannerViewModel.LoadGames();
            DataContext = bannerViewModel;
        }

        #region Settings button

        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            settingsViewModel = new SettingsViewModel();
            DataContext = settingsViewModel;
        }

        #endregion Settings button

        #region When program closed

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        #endregion When program closed
    }
}