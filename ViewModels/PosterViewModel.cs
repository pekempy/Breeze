using GameLauncher.Models;
using System;
using System.Collections.ObjectModel;

namespace GameLauncher.ViewModels
{
    internal class PosterViewModel
    {
        private LoadAllGames lag = new LoadAllGames();
        public static ObservableCollection<GameList> PosterViewOC { get; set; }
        public static ObservableCollection<GenreList> GenreListOC { get; set; }
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
            PosterViewOC = lag.Games;
        }

        public void LoadGenres()
        {
            lag.LoadGenres();
            GenreListOC = lag.Genres;
        }

        public void LoadList()
        {
            for (int i = 0; i < PosterViewOC.Count; i++)
            {
                Console.WriteLine(string.Concat(PosterViewOC[i].Title, " | ", PosterViewOC[i].Genre, "|", PosterViewOC[i].Path));
            }
        }
    }
}