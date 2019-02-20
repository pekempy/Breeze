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

namespace GameLauncher.Views
{
    public partial class ExeSelection : Page
    {
        public static ExeSelection es;
        public List<string> ExeList = new List<string>();
        private MainWindow MainWindow = ((MainWindow)Application.Current.MainWindow);
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
            bool matchFound = false;

            Console.WriteLine("Title: " + title);
            Console.WriteLine("Exe selected: " + selectedExe);
            
            for (int i = 0; i < ExeList.Count; i++)
            {
                if (ExeList[i].Contains(title + ";"))
                {
                    matchFound = true;
                    ExeList[i] = title + ";" + selectedExe;
                }
            }
            if (matchFound == false)
            {
                ExeList.Add(title + ";" + selectedExe);
            }
        }
        private void AcceptExeSelection_OnClick(object sender, RoutedEventArgs e)
        {
            //Save contents of ExeList and add to launcher
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
