using System.ComponentModel;

namespace GameLauncher.Models
{
    public class SearchModel { }

    public class SearchResults : INotifyPropertyChanged
    {
        private string thumbnail;
        private string image;

        public string Thumbnail { get { return thumbnail; } set { thumbnail = value; OnPropertyChanged("Thumbnail"); } }

        public string Image { get { return image; } set { image = value; OnPropertyChanged("Image"); } }

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