using GameLauncher.Models;
using GameLauncher.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace GameLauncher.Views
{
    public partial class PosterView : UserControl
    {
        public static string FilterGenreName;
        public string installPath = AppDomain.CurrentDomain.BaseDirectory;
        private MainWindow MainWindow = ((MainWindow)Application.Current.MainWindow);
        public CollectionViewSource GameListCVS;

        public PosterView()
        {
            InitializeComponent();
        }
        private void DeleteGame_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyFile.RemoveGameFromFile(((Button)sender).Tag);
            try
            {
                ModifyFile.DeleteGameImages(((Button)sender).CommandParameter.ToString());
            }
            catch (Exception exc) { Trace.WriteLine("Failed to delete images for game: " + exc); }
            string removeguid = ((Button)sender).Tag.ToString();
            foreach (var item in PosterViewModel.PosterViewOC.ToList())
            {
                if (removeguid == item.Guid)
                {
                    Trace.WriteLine(DateTime.Now + ": Removed Game: " + item.Title);
                    PosterViewModel.PosterViewOC.Remove(item);
                }
            }
        }

        private void EditGame_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyFile.EditGameInfile(((Button)sender).Tag);
        }

        private void GameLink_OnClick(object sender, RoutedEventArgs e)
        {
            object link = ((Button)sender).Tag;
            string linkstring = link.ToString().Trim();

            if (linkstring != string.Empty)
            {
                Process.Start(new ProcessStartInfo(linkstring));
            }
        }

        private void LaunchButton_OnClick(object sender, RoutedEventArgs e)
        {
            object link = ((Button)sender).Tag;
            string linkString = link.ToString().Trim();
            if (linkString != string.Empty)
            {
                Process.Start(new ProcessStartInfo(installPath + "Resources/shortcuts/" + linkString));
            }
        }

        private void SearchString_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshList();
        }

        private void EnableFilteringCheat(object sender, RoutedEventArgs e)
        {
            GameListCVS = ((CollectionViewSource)(FindResource("GameListCVS")));
            MainWindow.cvs = GameListCVS;
            MainWindow.MenuToggleButton.IsChecked = true;
        }

        private void GameSearch(object sender, FilterEventArgs e)
        {
            GameList gl = e.Item as GameList;
            e.Accepted &= gl.Title.ToUpper().Contains(GameSearchBar.Text.ToUpper());
        }

        public void GenreFilter(object sender, FilterEventArgs e)
        {
            GameList gl = e.Item as GameList;
            e.Accepted &= gl.Genre.ToUpper().Contains(FilterGenreName.ToUpper());
        }

        public void GenreToFilter(string filtergenrename)
        {
            FilterGenreName = filtergenrename;
        }

        public void RefreshList()
        {
            GameListCVS = ((CollectionViewSource)(FindResource("GameListCVS")));
            MainWindow.cvs = GameListCVS;
            if (FilterGenreName != null) { GameListCVS.Filter += new FilterEventHandler(GenreFilter); }
            if (GameSearchBar.Text != null) { GameListCVS.Filter += new FilterEventHandler(GameSearch); }
            if (GameListCVS.View != null)
                GameListCVS.View.Refresh();
        }

        public void RefreshList2(CollectionViewSource cvscvs)
        {
            if (cvscvs != null)
            {
                GameListCVS = cvscvs;
                if (FilterGenreName != null || FilterGenreName != "") { GameListCVS.Filter += new FilterEventHandler(GenreFilter); }
                if (GameSearchBar.Text != null) { GameListCVS.Filter += new FilterEventHandler(GameSearch); }
                if (GameListCVS.View != null)
                    GameListCVS.View.Refresh();
            }
        }
    }
}