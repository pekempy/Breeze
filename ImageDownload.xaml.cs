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
using System.Windows.Shapes;

namespace GameLauncher
{
    /// <summary>
    /// Interaction logic for ImageDownload.xaml
    /// </summary>
    public partial class ImageDownload : Window
    {
        public ImageDownload(string gametitle, string searchstring, string imagetype)
        {
            InitializeComponent();
            windowTitle.Text = "Breeze Image Search: " + searchstring;
        }
    }
}
