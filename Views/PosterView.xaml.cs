using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GameLauncher.Models;
using GameLauncher.ViewModels;

namespace GameLauncher.Views
{
    public partial class PosterView : UserControl
    {
        private MainWindow MainWindow = ((MainWindow)Application.Current.MainWindow);

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

        public static void SearchGames(string searchString)
        {
            Console.WriteLine(searchString);
            //This writes whatever is in the search box... but... cannot get games to filter by Title on this
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