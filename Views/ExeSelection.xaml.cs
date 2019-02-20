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
}
