using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace GameLauncher.Models
{
    public class ExeModel { }

    public class GameExecutables : INotifyPropertyChanged
    {
        private string _title;
        private string _exe1;
        private string _exe2;
        private string _exe3;


        public string Title { get { return _title; } set { _title = value; OnPropertyChanged("Title"); } }

        public string Exe1 { get { return _exe1; } set { _exe1 = value; OnPropertyChanged("Exe1"); } }

        public string Exe2 { get { return _exe2; } set { _exe2 = value; OnPropertyChanged("Exe2"); } }

        public string Exe3 { get { return _exe3; } set { _exe3 = value; OnPropertyChanged("Exe3"); } }

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