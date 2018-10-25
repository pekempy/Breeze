using GameLauncher.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;

namespace GameLauncher.ViewModels
{
    internal class PosterViewModel
    {
        public static ObservableCollection<GameList> PosterViewOC { get; set; }

        public void LoadGames()
        {
            LoadAllGames lag = new LoadAllGames();
            lag.LoadGames();
            PosterViewOC = lag.Games;
        }
    }
}