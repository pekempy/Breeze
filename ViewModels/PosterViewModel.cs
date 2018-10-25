using GameLauncher.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace GameLauncher.ViewModels
{
    internal class PosterViewModel
    {
        private LoadAllGames lag = new LoadAllGames();
        public static ObservableCollection<GameList> PosterViewOC { get; set; }

        public void LoadGames()
        {
            lag.LoadGames();
            PosterViewOC = lag.Games;
        }
    }
}