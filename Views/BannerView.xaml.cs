using GameLauncher.Models;
using GameLauncher.Properties;
using GameLauncher.ViewModels;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace GameLauncher.Views
{
    public partial class BannerView : UserControl
    {
        public static string FilterGenreName;
        public string installPath = AppDomain.CurrentDomain.BaseDirectory;
        private MainWindow MainWindow = ((MainWindow)Application.Current.MainWindow);
        public CollectionViewSource GameListCVS;

        public BannerView()
        {
            InitializeComponent();
        }

        public void BannerBackground_Loaded(object sender, RoutedEventArgs e)
        {
            var converter = new BrushConverter();
            if (gameListView.Items.Count != 0)
            {
                for (int i = 0; i < gameListView.Items.Count; i++)
                {
                    ContentPresenter c = (ContentPresenter)gameListView.ItemContainerGenerator.ContainerFromItem(gameListView.Items[i]);
                    try
                    {
                        TextBlock tb = c.ContentTemplate.FindName("BannerBackground", c) as TextBlock;
                        tb.Background = (Brush)converter.ConvertFromString(Settings.Default.bannertitles);
                    }
                    catch (Exception br) { Trace.WriteLine("Break: " + br); break; }
                }
            }
        }
        private void GameLink_OnClick(object sender, RoutedEventArgs e)
        {
            object link = ((Button)sender).Tag;
            string linkstring = link.ToString().Trim();

            if (linkstring != "")
            {
                Process.Start(new ProcessStartInfo(linkstring));
            }
        }

        private void LaunchButton_OnClick(object sender, RoutedEventArgs e)
        {
            object link = ((Button)sender).Tag;
            string linkString = link.ToString().Trim();
            if (linkString != "")
            {
                Process.Start(new ProcessStartInfo(installPath + "Resources/shortcuts/" + linkString));
            }
        }

        private void EditGame_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyFile.EditGameInfile(((Button)sender).Tag);
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
            foreach (var item in BannerViewModel.BannerViewOC.ToList())
            {
                if (removeguid == item.Guid)
                {
                    Trace.WriteLine(DateTime.Now + ": Removed Game: " + item.Title);
                    BannerViewModel.BannerViewOC.Remove(item);
                }
            }
        }

        //When text is changed in searchbar, apply filter
        private void SearchString_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshList();
        }

        //Hacky method to set cvs in mainwindow on a hidden button
        private void EnableFilteringCheat(object sender, RoutedEventArgs e)
        {
            GameListCVS = ((CollectionViewSource)(FindResource("GameListCVS")));
            MainWindow.cvs = GameListCVS;
            MainWindow.MenuToggleButton.IsChecked = true;
        }

        //FILTERS GAMES BASED ON THE TITLE SEARCHED
        private void GameSearch(object sender, FilterEventArgs e)
        {
            GameList gl = e.Item as GameList;
            e.Accepted &= gl.Title.ToUpper().Contains(GameSearchBar.Text.ToUpper());
        }

        //FILTERS GAMES BASED ON THE GENRE SELECTED
        public void GenreFilter(object sender, FilterEventArgs e)
        {
            GameList gl = e.Item as GameList;
            e.Accepted &= gl.Genre.ToUpper().Contains(FilterGenreName.ToUpper());
        }

        //PULLS GENRENAME FROM MAINWINDOW
        public void GenreToFilter(string filtergenrename)
        {
            //Set public variable for use in GenreFilter
            FilterGenreName = filtergenrename;
        }

        //REFRESHES LIST AFTER SEARCH TEXT
        public void RefreshList()
        {
            GameListCVS = ((CollectionViewSource)(FindResource("GameListCVS")));
            MainWindow.cvs = GameListCVS;
            if (FilterGenreName != null)
            {
                GameListCVS.Filter += new FilterEventHandler(GenreFilter);
            }
            if (GameSearchBar.Text != null)
            {
                GameListCVS.Filter += new FilterEventHandler(GameSearch);
            }
            if (GameListCVS.View != null)
                GameListCVS.View.Refresh();
        }

        //REFRESHES LIST AFTER GENRE SELECTED
        public void RefreshList2(CollectionViewSource cvscvs)
        {
            if (cvscvs != null)
            {
                GameListCVS = cvscvs;
                if (FilterGenreName != null)
                {
                    GameListCVS.Filter += new FilterEventHandler(GenreFilter);
                }
                if (GameSearchBar.Text != null)
                {
                    GameListCVS.Filter += new FilterEventHandler(GameSearch);
                }
                if (GameListCVS.View != null)
                    GameListCVS.View.Refresh();
            }
        }
    }
}