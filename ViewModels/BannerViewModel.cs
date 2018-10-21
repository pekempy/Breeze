using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using GameLauncher.Models;
using GameLauncher.Views;

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