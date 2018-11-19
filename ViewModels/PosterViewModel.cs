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