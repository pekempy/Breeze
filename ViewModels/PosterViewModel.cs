using GameLauncher.Models;
using System.Collections.ObjectModel;

namespace GameLauncher.ViewModels
{
    internal class PosterViewModel
    {
        public ObservableCollection<GameList> PosterView { get; set; }

        public void LoadGames()
        {
            LoadAllGames lag = new LoadAllGames();
            lag.LoadGames();
            PosterView = lag.Games;
        }
    }
}