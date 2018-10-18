using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameLauncher.Views;
using System.IO;
using GameLauncher.ViewModels;

namespace GameLauncher
{
    public partial class MainWindow : Window
    {
        #region ViewModels are at class level to be reused

        private GridViewModel gridViewModel;
        private PosterViewModel posterViewModel;
        private ListViewModel listViewModel;
        private BannerViewModel bannerViewModel;
        private ListView lv;
        private GridView gv;
        private PosterView pv;
        private BannerView bv;
        private AddGame ag;

        #endregion ViewModels are at class level to be reused

        public MainWindow()
        {
            InitializeComponent();

            #region Instantiate new displays only ONCE

            gridViewModel = new GridViewModel();
            posterViewModel = new PosterViewModel();
            bannerViewModel = new BannerViewModel();
            listViewModel = new ListViewModel();
            gv = new GridView();
            pv = new PosterView();
            bv = new BannerView();
            lv = new ListView();
            ag = new AddGame();

            #endregion Instantiate new displays only ONCE

            #region default view

            listViewModel.LoadGames();
            DataContext = listViewModel;

            #endregion default view

            #region if game file doesn't exist, create dir + open ag dialog

            //If games list doesn't exist, create directory and open ag dialog
            if (!File.Exists("./Resources/GamesList.txt"))
            {
                Directory.CreateDirectory("./Resources/");
                ag.Show();
            }

            #endregion if game file doesn't exist, create dir + open ag dialog
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
            DataContext = null;
            listViewModel.LoadGames();
            //bannerViewModel.LoadGames();
            //posterViewModel.LoadGames();
            //gridViewModel.LoadGames();
        }

        public void RefreshGames_OnClick(object sender, RoutedEventArgs e)
        {
            RefreshGames();
        }

        #endregion Refresh Games method

        #region Open AddGameWindow with FAB

        private void openAddGameWindow_OnClick(object sender, RoutedEventArgs e)
        {
            this.Opacity = 0.5;
            ag.ShowDialog();
            this.Opacity = 100;
        }

        #endregion Open AddGameWindow with FAB

        #region Change DataContext with buttons in overflow

        private void GridButton_OnClick(object sender, RoutedEventArgs e)
        {
            DataContext = gridViewModel;
        }

        private void PosterButton_OnClick(object sender, RoutedEventArgs e)
        {
            DataContext = posterViewModel;
        }

        private void BannerButton_OnClick(object sender, RoutedEventArgs e)
        {
            DataContext = bannerViewModel;
        }

        private void ListButton_OnClick(object sender, RoutedEventArgs e)
        {
            DataContext = listViewModel;
        }

        #endregion Change DataContext with buttons in overflow

        #region Refresh & Settings button

        private void RefreshButton_OnClick(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            //code to be put in for settings form
            return; //to prevent errors
        }

        #endregion Refresh & Settings button

        #region When program closed

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Application.Current.Shutdown();
        }

        #endregion When program closed
    }
}