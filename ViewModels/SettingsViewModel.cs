using GameLauncher.Models;
using System.Collections.ObjectModel;

namespace GameLauncher.ViewModels
{
    internal class SettingsViewModel
    {
        private LoadAllGames lag = new LoadAllGames();
        public static ObservableCollection<GenreList> GenreListOC { get; set; }

        public void LoadGenres()
        {
            lag.LoadGenres();
            GenreListOC = lag.Genres;
        }
    }
}