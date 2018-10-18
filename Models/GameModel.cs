using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLauncher.ViewModels;

namespace GameLauncher.Models
{
    public class GameModel { }

    public class GameList : INotifyPropertyChanged
    {
        private string _title;
        private string _genre;
        private string _path;
        private string _link;
        private string _icon;
        private string _poster;
        private string _banner;

        public string Title { get { return _title; } set { _title = value; OnPropertyChanged(Title); } }
        public string Genre { get { return _genre; } set { _genre = value; OnPropertyChanged(Genre); } }
        public string Path { get { return _path; } set { _path = value; OnPropertyChanged(Path); } }
        public string Link { get { return _link; } set { _link = value; OnPropertyChanged(Link); } }
        public string Icon { get { return _icon; } set { _icon = value; OnPropertyChanged(Icon); } }
        public string Poster { get { return _poster; } set { _poster = value; OnPropertyChanged(Poster); } }
        public string Banner { get { return _banner; } set { _banner = value; OnPropertyChanged(Banner); } }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged
    }
}