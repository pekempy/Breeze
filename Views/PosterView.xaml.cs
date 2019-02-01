using GameLauncher.Models;
using GameLauncher.ViewModels;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml;

namespace GameLauncher.Views
{
    public partial class PosterView : UserControl
    {
        public static string FilterGenreName;
        private MainWindow MainWindow = ((MainWindow)Application.Current.MainWindow);
        public CollectionViewSource GameListCVS;

        public PosterView()
        {
            InitializeComponent();
        }

        public List<string> ThumbList;
        public List<string> LinkList;
        private void DEBUG_OnClick(object sender, RoutedEventArgs e)
        {
            string gametitle = ((Button)sender).Tag.ToString();
            var url = "https://www.qwant.com/?q="+ gametitle +"%20poster&t=images";
            HtmlAgilityPack.HtmlWeb hw = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = hw.Load(url);
            List<string> ThumbList = new List<string>();
            List<string> LinkList = new List<string>();
            foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//img"))
            {
                string imgValue = link.GetAttributeValue("src", string.Empty);
                ThumbList.Add(imgValue);
                string[] imgLink = imgValue.Split('=');
                string imglink = imgLink[1].Replace("%3A", ":");
                imglink = imglink.Replace("%2F", "/");
                imglink = imglink.Remove(imglink.Length - 2);
                imgValue = "http:" + imgValue;
                Console.WriteLine("----------");
                Console.WriteLine("Thumbnail: "+imgValue);
                Console.WriteLine("Link: " + imglink);
            }
        }
        private void DeleteGame_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyFile.RemoveGameFromFile(((Button)sender).Tag);
            MainWindow.RefreshGames();
        }

        private void EditGame_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyFile.EditGameInfile(((Button)sender).Tag);
            MainWindow.RefreshGames();
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
                Process.Start(new ProcessStartInfo(linkString));
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
            if (FilterGenreName != null) { GameListCVS.Filter += new FilterEventHandler(GenreFilter); }
            if (GameSearchBar.Text != null) { GameListCVS.Filter += new FilterEventHandler(GameSearch); }
            if (GameListCVS.View != null)
                GameListCVS.View.Refresh();
        }

        //REFRESHES LIST AFTER GENRE SELECTED
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