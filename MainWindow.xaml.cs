using GameLauncher.ViewModels;
using GameLauncher.Views;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

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

        public MainWindow()
        {
            InitializeComponent();
            posterViewModel = new PosterViewModel();
            posterViewModel.LoadGames();
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

        public void OpenEditGameDialog(String guid)
        {
            DialogFrame.Visibility = Visibility.Visible;
            DialogFrame.Content = DialogEditGames;
            DialogEditGames.currentGuid(guid);
            DialogEditGames.EditGameDialog.IsOpen = true;
        }

        private void BannerButton_OnClick(object sender, RoutedEventArgs e)
        {
            BannerViewActive();
        }

        private void BannerViewActive()
        {
            bannerViewModel = new BannerViewModel();
            bannerViewModel.LoadGames();
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
            DataContext = listViewModel;
        }

        private void PosterButton_OnClick(object sender, RoutedEventArgs e)
        {
            PosterViewActive();
        }

        private void PosterViewActive()
        {
            posterViewModel = new PosterViewModel();
            posterViewModel.LoadGames();
            DataContext = posterViewModel;
        }

        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            settingsViewModel = new SettingsViewModel();
            DataContext = settingsViewModel;
        }

        private void OpenAddGameWindow_OnClick(object sender, RoutedEventArgs e)
        {
            OpenAddGameDialog();
            RefreshGames();
        }

        private void SearchChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            string searchString = GameSearchBar.Text;
            PosterView.SearchGames(searchString);
        }

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
            else
            {
                Console.WriteLine("Nothing");
            }
        }
    }
}