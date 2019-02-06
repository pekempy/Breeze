using GameLauncher.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GameLauncher.ViewModels
{
    public class SearchViewModel
    {
        private LoadSearch ls = new LoadSearch();
        public static ObservableCollection<SearchResults> SearchList { get; set; }

        public void LoadSearch(string gametitle, string imagetype, string searchstring, int offset)
        {
            ls.Search(gametitle, imagetype, searchstring, offset);
            SearchList = ls.SearchList;
        }
    }
}