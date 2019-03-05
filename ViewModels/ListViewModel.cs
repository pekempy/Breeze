using GameLauncher.Models;
using System;
using System.Collections.ObjectModel;

namespace GameLauncher.ViewModels
{
    public class ListViewModel
    {
        public ObservableCollection<GameList> ListViewOC { get; set; }
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
            ListViewOC = MainWindow.GameListMW;
            GenreListOC = MainWindow.GenreListMW;
        }
        

    }
}