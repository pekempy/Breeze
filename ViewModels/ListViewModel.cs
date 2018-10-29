using GameLauncher.Models;
using System.Collections.ObjectModel;

namespace GameLauncher.ViewModels
{
    public class ListViewModel
    {
        public ObservableCollection<GameList> ListViewOC { get; set; }
        public static ObservableCollection<GenreList> GenreListOC { get; set; }
        private LoadAllGames lag = new LoadAllGames();

        public void LoadGames()
        {
            lag.LoadGames();
            ListViewOC = lag.Games;
        }

        public void LoadGenres()
        {
            lag.LoadGenres();
            GenreListOC = lag.Genres;
        }
    }
}