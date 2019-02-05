using GameLauncher.Models;
using System;
using System.Collections.ObjectModel;

namespace GameLauncher.ViewModels
{
    internal class BannerViewModel
    {
        private LoadAllGames lag = new LoadAllGames();
        public static ObservableCollection<GameList> BannerViewOC { get; set; }
        public static ObservableCollection<GenreList> GenreListOC { get; set; }
        private LoadSearch ls = new LoadSearch();
        public static ObservableCollection<SearchResults> SearchList { get; set; }

        public void LoadSearch(string gametitle, string imagetype, string searchstring)
        {
            ls.Search(gametitle, imagetype, searchstring);
            SearchList = ls.SearchList;
        }
        public void LoadGames()
        {
            lag.LoadGames();
            BannerViewOC = lag.Games;
        }

        public void LoadGenres()
        {
            lag.LoadGenres();
            GenreListOC = lag.Genres;
        }

        public void LoadList()
        {
            for (int i = 0; i < BannerViewOC.Count; i++)
            {
                Console.WriteLine(string.Concat(BannerViewOC[i].Title, " | ", BannerViewOC[i].Genre, "|", BannerViewOC[i].Path));
            }
        }
    }
}