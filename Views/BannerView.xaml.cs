using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;
using GameLauncher.ViewModels;

namespace GameLauncher.Views
{
    public partial class BannerView : UserControl
    {
        public BannerView()
        {
            InitializeComponent();
        }

        #region Run the hyperlink

        private void Hyperlink_Link(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
        }

        #endregion Run the hyperlink

        #region Run the game

        private void LaunchButton_OnClick(object sender, RoutedEventArgs e)
        {
            object link = ((Button)sender).Tag;
            string linkString = link.ToString().Trim();
            Process.Start(new ProcessStartInfo(linkString));
        }

        #endregion Run the game
    }
}