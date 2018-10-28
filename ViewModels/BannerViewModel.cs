using GameLauncher.Models;
using System.Collections.ObjectModel;

namespace GameLauncher.ViewModels
{
    internal class BannerViewModel
    {
        public ObservableCollection<GameList> BannerViewOC { get; set; }

        public ObservableCollection<GenreList> BannerGenre { get; set; }

        public void LoadGames()
        {
            LoadAllGames lag = new LoadAllGames();
            lag.LoadGames();
            BannerViewOC = lag.Games;
        }
    }
}