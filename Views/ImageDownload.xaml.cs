using GameLauncher.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
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
using System.Windows.Shapes;

namespace GameLauncher.Views
{
    /// <summary>
    /// Interaction logic for ImageDownload.xaml
    /// </summary>
    public partial class ImageDownload : Window
    {
        public ImageDownload(string gametitle, string searchstring, string imagetype)
        {
            SearchViewModel svm = new SearchViewModel();
            svm.LoadSearch(gametitle, imagetype,searchstring);
            InitializeComponent();
            windowTitle.Text = "Breeze Image Search: " + searchstring;

        }
    }
    public class StringToImage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
                return new BitmapImage(new Uri((string)value, UriKind.RelativeOrAbsolute));

            if (value is Uri)
                return new BitmapImage((Uri)value);

            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
