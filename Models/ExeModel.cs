using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GameLauncher.Models
{
    public class ExeModel { }

    public class GameExecutables : INotifyPropertyChanged
    {
        private string _title;
        private string _link1;
        private string _link2;
        private string _link3;
        private string _link4;
        private string _link5;
        private string _link6;
        private string _link7;
        private string _link8;
        private string _link9;
        private string _link10;


        public string Title { get { return _title; } set { _title = value; OnPropertyChanged("Title"); } }

        public string Link1 { get { return _link1; } set { _link1 = value; OnPropertyChanged("Link1"); } }

        public string Link2 { get { return _link2; } set { _link2 = value; OnPropertyChanged("Link2"); } }

        public string Link3 { get { return _link3; } set { _link3 = value; OnPropertyChanged("Link3"); } }
        public string Link4 { get { return _link4; } set { _link4 = value; OnPropertyChanged("Link4"); } }
        public string Link5 { get { return _link5; } set { _link5 = value; OnPropertyChanged("Link5"); } }
        public string Link6 { get { return _link6; } set { _link6 = value; OnPropertyChanged("Link6"); } }
        public string Link7 { get { return _link7; } set { _link7 = value; OnPropertyChanged("Link7"); } }
        public string Link8 { get { return _link8; } set { _link8 = value; OnPropertyChanged("Link8"); } }
        public string Link9 { get { return _link9; } set { _link9 = value; OnPropertyChanged("Link9"); } }
        public string Link10 { get { return _link10; } set { _link10 = value; OnPropertyChanged("Link10"); } }

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