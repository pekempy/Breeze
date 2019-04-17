using GameLauncher.Models;
using System.Collections.ObjectModel;

namespace GameLauncher.ViewModels
{
    internal class SettingsViewModel
    {
        public static ObservableCollection<GenreList> GenreListOC { get; set; }

        public void LoadGenres()
        {
            GenreListOC = MainWindow.GenreListMW;
        }
    }
}