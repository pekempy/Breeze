using GameLauncher.Models;
using System.Collections.ObjectModel;

namespace GameLauncher.ViewModels
{
    internal class SettingsViewModel
    {
        private LoadAllGames lag = new LoadAllGames();
        private ExeSearch gs = new ExeSearch();
        public static ObservableCollection<GenreList> GenreListOC { get; set; }

        public void LoadGenres()
        {
            lag.LoadGenres();
            GenreListOC = lag.Genres;
        }
    }
}