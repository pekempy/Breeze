using GameLauncher.ViewModels;
using MaterialDesignThemes.Wpf;
using System;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace GameLauncher.Views
{
    /// <summary>
    /// Interaction logic for ImageDownload.xaml
    /// </summary>
    public partial class ImageDownload : Page
    {
        public ImageDownload(string gametitle, string searchstring, string imagetype)
        {
            int offset = 0;
            PosterViewModel pvm = new PosterViewModel();
            BannerViewModel bvm = new BannerViewModel();
            ListViewModel lvm = new ListViewModel();
            pvm.LoadSearch(gametitle, imagetype,searchstring, offset);
            bvm.LoadSearch(gametitle, imagetype, searchstring, offset);
            lvm.LoadSearch(gametitle, imagetype, searchstring, offset);
            InitializeComponent();
            windowTitle.Text = searchstring.ToUpperInvariant();

        }

        private void qwantlink()
        {
            System.Diagnostics.Process.Start("https://www.qwant.com/?q=test");
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
