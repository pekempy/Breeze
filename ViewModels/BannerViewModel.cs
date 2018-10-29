using GameLauncher.Models;
using System.Collections.ObjectModel;

namespace GameLauncher.ViewModels
{
    internal class BannerViewModel
    {
        public static ObservableCollection<GenreList> GenreListOC { get; set; }
        public ObservableCollection<GameList> BannerViewOC { get; set; }
        private LoadAllGames lag = new LoadAllGames();

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
    }
}