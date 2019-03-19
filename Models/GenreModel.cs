using System.ComponentModel;

namespace GameLauncher.Models
{
    public class GenreModel { }

    public class GenreList : INotifyPropertyChanged
    {
        private string _name;
        private string _guid;

        public string Name { get { return _name; } set { _name = value; OnPropertyChanged("Name"); } }

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