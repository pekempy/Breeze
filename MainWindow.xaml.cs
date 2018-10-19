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
using GameLauncher.Models;
using System.IO;
using GameLauncher.ViewModels;

namespace GameLauncher
{
    public partial class MainWindow : Window
    {
        #region ViewModels are at class level to be reused

        //private LoadAllGames lag;
        private GridViewModel gridViewModel;

        private PosterViewModel posterViewModel;
        private ListViewModel listViewModel;
        private BannerViewModel bannerViewModel;
        private AddGame ag;

        #endregion ViewModels are at class level to be reused

        public MainWindow()
        {
            InitializeComponent();

            #region Instantiate new displays only ONCE

            //lag = new LoadAllGames();
            ag = new AddGame();

            #endregion Instantiate new displays only ONCE

            #region default view

            listViewModel = new ListViewModel();
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
            if (DataContext == gridViewModel)
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
            gridViewModel = new GridViewModel();
            gridViewModel.LoadGames();
            DataContext = gridViewModel;
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
            //code to be put in for settings form
            return; //to prevent errors
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