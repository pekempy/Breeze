using GameLauncher.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GameLauncher.ViewModels
{
    public class ExesViewModel
    {
        private ExeSearch es = new ExeSearch();
        public static ObservableCollection<GameExecutables> ExesOC { get; set; }

        public void SearchExe()
        {
            es.SearchForShortcuts(); 
            ExesOC = es.Exes;
        }
        public void UpdateObsCol(string title, string exe)
        {
            es.UpdateObsCol(title, exe);
        }
        public bool CheckBinding(string title)
        {
            bool result = es.CheckBinding(title);
            if (result == true) { return true; }
            else { return false; }
        }
    }
}