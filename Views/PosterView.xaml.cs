using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GameLauncher.Models;
using GameLauncher.ViewModels;

namespace GameLauncher.Views
{
    public partial class PosterView : UserControl
    {
        private MainWindow MainWindow = ((MainWindow)Application.Current.MainWindow);

        public PosterView()
        {
            InitializeComponent();
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
                Process.Start(new ProcessStartInfo(linkString));
            }
        }

        private void EditGame_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyFile.EditGameInfile(((Button)sender).Tag);
            MainWindow.RefreshGames();
        }

        private void DeleteGame_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyFile.RemoveGameFromFile(((Button)sender).Tag);
            MainWindow.RefreshGames();
        }

        private void SearchString_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshList();
        }

        public void RefreshList()
        {
            CollectionViewSource GameListCVS = (CollectionViewSource)FindResource("GameListCVS");
            GameListCVS.Filter += new FilterEventHandler(GameSearch);
            GameListCVS.Filter += new FilterEventHandler(GenreFilter);
            if (GameListCVS != null)
                GameListCVS.View.Refresh(); //This is getting a null "GameListCVS.View"
        }

        private void GameSearch(object sender, FilterEventArgs e)

        {
            GameList gl = e.Item as GameList;
            e.Accepted &= gl.Title.ToUpper().Contains(GameSearchBar.Text.ToUpper());
        }

        public static string GTF;

        public void GenreToFilter(string gtf)
        {
            GTF = gtf;
            Console.WriteLine(GTF);
            //somehow need to apply the filter from here
        }

        public void GenreFilter(object sender, FilterEventArgs e)
        {
            GameList gl = e.Item as GameList;
            if (GTF != null)
            {
                e.Accepted &= gl.Genre.ToUpper().Contains(GTF.ToUpper());
                Console.WriteLine(sender);
            }
            return;
        }
    }
}