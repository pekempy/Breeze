using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using GameLauncher.Views;
using GameLauncher.Models;

namespace GameLauncher.ViewModels
{
    public class ListViewModel
    {



        public ObservableCollection<GameList> ListView { get; set; }
        public void LoadGames()
        {
            LoadAllGames lag = new LoadAllGames();
            lag.LoadGames();
            ListView = lag.Games;

        }

    }

}