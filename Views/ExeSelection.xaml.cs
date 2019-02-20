using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GameLauncher.Models;
using GameLauncher.ViewModels;
using System.Globalization;
using Microsoft.Win32;

namespace GameLauncher.Views
{
    public partial class ExeSelection : Page
    {
        public static ExeSelection es;
        public List<string> ExeList = new List<string>();
        private MainWindow MainWindow = ((MainWindow)Application.Current.MainWindow);
        private bool matchFound = false;
        public ExeSelection()
        {
            es = this;
            InitializeComponent();
        }

        public void CloseExeSelection(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow)?.CloseExeSearchDialog();
        }
        public static void ChangeWindowSize(double height, double width)
        {
            es.ExeGrid.Height = ((MainWindow)Application.Current.MainWindow).ActualHeight * 0.9;
            es.ExeGrid.Width = ((MainWindow)Application.Current.MainWindow).ActualWidth * 0.9;
        }
        protected void UILoaded(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Resized();
        }
        private void RadioButton_Selected(object sender, RoutedEventArgs e)
        {
            string selectedExe = ((RadioButton)sender).Tag.ToString();
            string title = selectedExe.Substring(selectedExe.IndexOf("\\common\\"));
            string[] titlex = title.Split('\\');
            title = titlex[2].ToString();
            for (int i = 0; i < ExeList.Count; i++)
            {
                if (ExeList[i].Contains(title + ";"))
                {
                    matchFound = true;
                    ExeList[i] = title + ";" + selectedExe;
                    matchFound = false;
                }
            }
            if (matchFound == false)
            {
                ExeList.Add(title + ";" + selectedExe);
            }
        }
        private void ManualLauncher(object sender, RoutedEventArgs e)
        {
            string title = ((Button)sender).Tag.ToString();
            string exe;
            string newgame;
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.RestoreDirectory = true;
            fileDialog.Filter = "Executable Files (*.exe) | *.exe;*.lnk;*.url";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true)
            {
                exe = fileDialog.FileName;
                newgame = title + ";" + exe;
                for (int i = 0; i<ExeList.Count; i++)
                {
                    if (ExeList[i].Contains(title + ";")){
                        matchFound = true;
                        ExeList[i] = newgame;
                        matchFound = false;
                    }
                }
                if (matchFound == false)
                {
                    ExeList.Add(newgame);
                }
            }
        }
        private void AcceptExeSelection_OnClick(object sender, RoutedEventArgs e)
        {
            foreach (var item in ExeList)
            {
                Console.WriteLine(item);
            }
        }
    }
    public sealed class Null2Visibility : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public sealed class Null2VisibilityInvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
