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
        public string ImageType;
        public ImageDownload(string gametitle, string searchstring, string imagetype)
        {
            int offset = 0;
            ImageType = imagetype;
            PosterViewModel pvm = new PosterViewModel();
            BannerViewModel bvm = new BannerViewModel();
            ListViewModel lvm = new ListViewModel();
            pvm.LoadSearch(gametitle, imagetype,searchstring, offset);
            bvm.LoadSearch(gametitle, imagetype, searchstring, offset);
            lvm.LoadSearch(gametitle, imagetype, searchstring, offset);
            InitializeComponent();
            DownloadGrid.Height = ((MainWindow)Application.Current.MainWindow).ActualHeight * 0.8;
            DownloadGrid.Width = ((MainWindow)Application.Current.MainWindow).ActualWidth * 0.8;
            windowTitle.Text = searchstring.ToUpperInvariant();

        }
        protected void ImageDownload_Loaded(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < SearchView.Items.Count; i++)
            {
                ContentPresenter c = (ContentPresenter)SearchView.ItemContainerGenerator.ContainerFromItem(SearchView.Items[i]);
                if (c != null)
                {
                    try
                    {
                        Card card = c.ContentTemplate.FindName("DTCard", c) as Card;
                        Button button = c.ContentTemplate.FindName("DTButton", c) as Button;
                        if (ImageType == "icon")
                        {
                            card.Width = 80;
                            card.Height = 80;
                            button.Width = 80;
                            button.Height = 80;
                        }
                        if (ImageType == "poster")
                        {
                            card.Width = 180;
                            card.Height = 220;
                            button.Width = 180;
                            button.Height = 220;
                        }
                        if (ImageType == "banner")
                        {
                            card.Width = 290;
                            card.Height = 100;
                            button.Width = 290;
                            button.Height = 100;
                        }
                    }
                    catch (Exception ex) { Console.WriteLine("Error: " + ex); }
                }
            }
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
