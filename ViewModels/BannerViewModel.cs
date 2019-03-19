using GameLauncher.Models;
using System.Collections.ObjectModel;

namespace GameLauncher.ViewModels
{
    internal class BannerViewModel
    {
        public static ObservableCollection<GameList> BannerViewOC { get; set; }
        public static ObservableCollection<GenreList> GenreListOC { get; set; }
        private LoadSearch ls = new LoadSearch();
        public static ObservableCollection<SearchResults> SearchList { get; set; }

        public void LoadSearch(string gametitle, string imagetype, string searchstring, int offset)
        {
            ls.Search(gametitle, imagetype, searchstring, offset);
            SearchList = ls.SearchList;
        }
        public void LoadGames()
        {
            BannerViewOC = MainWindow.GameListMW;
            GenreListOC = MainWindow.GenreListMW;
        }

    }
}