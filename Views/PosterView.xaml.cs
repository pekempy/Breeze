using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace GameLauncher.Views
{
    public partial class PosterView : UserControl
    {
        public PosterView()
        {
            InitializeComponent();
        }

        private void GameLink_OnClick(object sender, RoutedEventArgs e)
        {
            object link = ((Button)sender).Tag;
            string linkstring = link.ToString().Trim();

            if (linkstring != "")
            {
                Process.Start(new ProcessStartInfo(linkstring));
            }
        }

        private void LaunchButton_OnClick(object sender, RoutedEventArgs e)
        {
            object link = ((Button)sender).Tag;
            string linkString = link.ToString().Trim();
            if (linkString != "")
            {
                Process.Start(new ProcessStartInfo(linkString));
            }
        }

        private void EditGame_OnClick(object sender, RoutedEventArgs e)
        {
            object title = ((Button)sender).Tag;
            Console.WriteLine(title);
            return;
        }

        private void DeleteGame_OnClick(object sender, RoutedEventArgs e)
        {
            object title = ((Button)sender).Tag;
            Console.WriteLine(title);
            return;
        }
    }
}