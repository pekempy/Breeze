using GameLauncher.Models;
using System.Collections.ObjectModel;
using System.IO;

namespace GameLauncher.ViewModels
{
    internal class GridViewModel
    {
        public ObservableCollection<GameList> GridView { get; set; }

        public void LoadGames()
        {
            LoadAllGames lag = new LoadAllGames();
            lag.LoadGames();
            GridView = lag.Games;
        }
    }
}