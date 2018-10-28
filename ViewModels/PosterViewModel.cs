using GameLauncher.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace GameLauncher.ViewModels
{
    internal class PosterViewModel
    {
        public ObservableCollection<GameList> PosterViewOC { get; set; }

        public ObservableCollection<GenreList> PosterGenre { get; set; }

        private LoadAllGames lag = new LoadAllGames();

        public void LoadGames()
        {
            lag.LoadGames();
            PosterViewOC = lag.Games;
            PosterGenre = lag.Genres;
        }
    }
}