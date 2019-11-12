using System.ComponentModel;

namespace GameLauncher.Models
{
    public class SearchModel { }

    public class SearchResults : INotifyPropertyChanged
    {
        private string title;
        private string directlink;
        private string directlinkqwant;
        private string thumbnaillink;
        private string height;
        private string width;

        public string Title { get { return title; } set { title = value; OnPropertyChanged("Title"); } }
        public string DirectLink { get { return directlink; } set { directlink = value; OnPropertyChanged("DirectLink"); } }
        public string DirectLinkQwant { get { return directlinkqwant; } set { directlinkqwant = value; OnPropertyChanged("DirectLinkQwant"); } }
        public string ThumbnailLink { get { return thumbnaillink; } set { thumbnaillink = value; OnPropertyChanged("ThumbnailLink"); } }
        public string Height { get { return height; } set { height = value; OnPropertyChanged("Height"); } }
        public string Width { get { return width; } set { width = value; OnPropertyChanged("Width"); } }


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