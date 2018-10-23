using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using GameLauncher.ViewModels;

namespace GameLauncher.Views
{
    public partial class ListView : UserControl
    {
        private MainWindow MainWindow = ((MainWindow)Application.Current.MainWindow);

        public ListView()
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
            ModifyFile.EditGameInfile(((Button)sender).Tag);
            MainWindow.RefreshGames();
        }

        private void DeleteGame_OnClick(object sender, RoutedEventArgs e)
        {
            ModifyFile.RemoveGameFromFile(((Button)sender).Tag);
            MainWindow.RefreshGames();
        }
    }
}