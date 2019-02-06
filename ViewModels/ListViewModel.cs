﻿using GameLauncher.Models;
using System;
using System.Collections.ObjectModel;

namespace GameLauncher.ViewModels
{
    public class ListViewModel
    {
        public ObservableCollection<GameList> ListViewOC { get; set; }
        public static ObservableCollection<GenreList> GenreListOC { get; set; }
        private LoadAllGames lag = new LoadAllGames();
        private LoadSearch ls = new LoadSearch();
        public static ObservableCollection<SearchResults> SearchList { get; set; }

        public void LoadSearch(string gametitle, string imagetype, string searchstring, int offset)
        {
            ls.Search(gametitle, imagetype, searchstring, offset);
            SearchList = ls.SearchList;
        }
        public void LoadGames()
        {
            lag.LoadGames();
            ListViewOC = lag.Games;
        }

        public void LoadGenres()
        {
            lag.LoadGenres();
            GenreListOC = lag.Genres;
        }

        public void LoadList()
        {
            for (int i = 0; i < ListViewOC.Count; i++)
            {
                Console.WriteLine(string.Concat(ListViewOC[i].Title, " | ", ListViewOC[i].Genre, "|", ListViewOC[i].Path));
            }
        }
    }
}