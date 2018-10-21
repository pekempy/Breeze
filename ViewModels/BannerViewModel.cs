using GameLauncher.Models;
using System.Collections.ObjectModel;

namespace GameLauncher.ViewModels
{
    internal class BannerViewModel
    {
        public ObservableCollection<GameList> BannerView { get; set; }

        public void LoadGames()
        {
            LoadAllGames lag = new LoadAllGames();
            lag.LoadGames();
            BannerView = lag.Games;
        }
    }
}