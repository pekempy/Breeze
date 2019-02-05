using GameLauncher.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
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
    public partial class ImageDownload : Page
    {
        public ImageDownload(string gametitle, string searchstring, string imagetype)
        {
            PosterViewModel pvm = new PosterViewModel();
            BannerViewModel bvm = new BannerViewModel();
            ListViewModel lvm = new ListViewModel();
            pvm.LoadSearch(gametitle, imagetype,searchstring);
            bvm.LoadSearch(gametitle, imagetype, searchstring);
            lvm.LoadSearch(gametitle, imagetype, searchstring);
            InitializeComponent();
            windowTitle.Text = "Breeze Image Search: " + searchstring;

        }
        private void closeImageDLButton(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow)?.OpenImageDL("string", "string", "string");
        }
        private void DownloadImage_OnClick(object sender, RoutedEventArgs e)
        {
            string url = ((Button)sender).Tag.ToString();
            ((MainWindow)Application.Current.MainWindow)?.DownloadImage(url);
        }
    }

    internal class Bitmap
    {
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
