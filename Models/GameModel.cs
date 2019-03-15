using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GameLauncher.Models
{
    public class GameModel { }

    public class GameList : INotifyPropertyChanged
    {
        private string _title;
        private string _genre;
        private string _path;
        private string _link;
        private BitmapImage _icon;
        private BitmapImage _poster;
        private BitmapImage _banner;
        private string _guid;

        public string Title { get { return _title; } set { _title = value; OnPropertyChanged("Title"); } }

        public string Genre { get { return _genre; } set { _genre = value; OnPropertyChanged("Genre"); } }

        public string Path { get { return _path; } set { _path = value; OnPropertyChanged("Path"); } }

        public string Link { get { return _link; } set { _link = value; OnPropertyChanged("Link"); } }

        public BitmapImage Icon { get { return _icon; } set { _icon = value; OnPropertyChanged("Icon"); } }

        public BitmapImage Poster { get { return _poster; } set { _poster = value; OnPropertyChanged("Poster"); } }

        public BitmapImage Banner { get { return _banner; } set { _banner = value; OnPropertyChanged("Banner"); } }

        public string Guid { get { return _guid; } set { _guid = value; OnPropertyChanged("Guid"); } }

        public event PropertyChangedEventHandler PropertyChanged;


        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {

                 PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
    }
}