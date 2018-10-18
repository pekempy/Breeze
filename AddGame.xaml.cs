using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;
using GameLauncher.ViewModels;

namespace GameLauncher
{
    public partial class AddGame : Window
    {
        public AddGame()
        {
            InitializeComponent();
        }

        private ListViewModel listViewModel = new ListViewModel();

        #region ADD GAME button

        private void AddGame_OnClick(object sender, RoutedEventArgs e)
        {
            //This part repairs the link so it launches properly
            string ngl = NewGameLink.Text.ToString();
            if (!ngl.Contains("http") && (ngl != ""))
            {
                UriBuilder uriBuilder = new UriBuilder();
                uriBuilder.Scheme = "http";
                uriBuilder.Host = NewGameLink.Text;
                Uri uri = uriBuilder.Uri;
                NewGameLink.Text = uri.ToString();
            }
            //Write all the fields to the text file
            try
            {
                TextWriter tsw = new StreamWriter(@"./Resources/GamesList.txt", true);
                tsw.WriteLine(NewGameTitle.Text + " | " +
                              NewGameGenre.Text + " | " +
                              NewGamePath.Text + " | " +
                              NewGameLink.Text + " | " +
                              NewGameIcon.Text + " | " +
                              NewGamePoster.Text + " | " +
                              NewGameBanner.Text);
                tsw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            this.Hide();
            clearFields();
        }

        #endregion ADD GAME button

        #region CANCEL button

        private void CancelAddGame_OnClick(object sender, RoutedEventArgs e)
        {
            this.Hide();
            clearFields();
        }

        #endregion CANCEL button

        #region clearFields

        private void clearFields()
        {
            NewGameTitle.Text = "";
            NewGamePath.Text = "";
            NewGameGenre.Text = "";
            NewGameLink.Text = "";
            NewGameIcon.Text = "";
            NewGamePoster.Text = "";
            NewGameBanner.Text = "";
        }

        #endregion clearFields

        #region PickGenre button - unknown

        private void PickGenre_OnClick(object sender, RoutedEventArgs e)
        {
            return; //??
        }

        #endregion PickGenre button - unknown

        #region GenreDialog close - unknown

        public void GenreDialog_OnDialogClosing(object sender, RoutedEventArgs e)
        {
            return; //??
        }

        #endregion GenreDialog close - unknown

        #region AddGenre OnClick

        private void AddGenre_OnClick(object sender, RoutedEventArgs e)
        {
            string genrePlaceHolder = null;
            if (Action.IsChecked == true) { genrePlaceHolder += "Action "; }
            if (Adventure.IsChecked == true) { genrePlaceHolder += "Adventure "; }
            if (Fantasy.IsChecked == true) { genrePlaceHolder += "Fantasy "; }
            if (FPS.IsChecked == true) { genrePlaceHolder += "FPS "; }
            if (Horror.IsChecked == true) { genrePlaceHolder += "Horror "; }
            if (OpenWorld.IsChecked == true) { genrePlaceHolder += "Open-World "; }
            if (Platform.IsChecked == true) { genrePlaceHolder += "Platform "; }
            if (RolePlaying.IsChecked == true) { genrePlaceHolder += "Role-Playing "; }
            if (Shooter.IsChecked == true) { genrePlaceHolder += "Shooter "; }
            if (Simulation.IsChecked == true) { genrePlaceHolder += "Simulation "; }
            if (Thriller.IsChecked == true) { genrePlaceHolder += "Thriller "; }
            if (genrePlaceHolder != null)
            {
                genrePlaceHolder = genrePlaceHolder.Replace(" ", "; ");
                genrePlaceHolder = genrePlaceHolder.TrimEnd(' ');
                genrePlaceHolder = genrePlaceHolder.TrimEnd(';');
            }
            NewGameGenre.Text = genrePlaceHolder;
            ClearGenreBoxes();
            return;
        }

        #endregion AddGenre OnClick

        #region ClearGenreSelection

        private void ClearGenreSelection_OnClick(object sender, RoutedEventArgs e)
        {
            ClearGenreBoxes();
        }

        #endregion ClearGenreSelection

        #region Clear Genre Boxes

        private void ClearGenreBoxes()
        {
            Action.IsChecked = false;
            Adventure.IsChecked = false;
            Fantasy.IsChecked = false;
            FPS.IsChecked = false;
            Horror.IsChecked = false;
            OpenWorld.IsChecked = false;
            Platform.IsChecked = false;
            RolePlaying.IsChecked = false;
            Shooter.IsChecked = false;
            Simulation.IsChecked = false;
            Thriller.IsChecked = false;
        }

        #endregion Clear Genre Boxes

        #region Attach Launcher popup

        private void AttachLauncher_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.InitialDirectory = "C:\\";
            fileDialog.Filter = "Executable Files (*.exe) | *.exe;*.lnk";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true)
            {
                string ngLauncherFile = fileDialog.FileName;
                NewGamePath.Text = ngLauncherFile;
            }
        }

        #endregion Attach Launcher popup

        #region Attach Icon popup

        private void AttachIcon_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.InitialDirectory = "C:\\";
            fileDialog.Filter = "Images (*.jpg;*.png;*.bmp | *.jpg;*.png;*.bmp";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true)
            {
                string ngIconFile = fileDialog.FileName;
                NewGameIcon.Text = ngIconFile;
            }
        }

        #endregion Attach Icon popup

        #region Attach Poster popup

        private void AttachPoster_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.InitialDirectory = "C:\\";
            fileDialog.Filter = "Images (*.jpg;*.png;*.bmp | *.jpg;*.png;*.bmp";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true)
            {
                string ngPosterFile = fileDialog.FileName;
                NewGamePoster.Text = ngPosterFile;
            }
        }

        #endregion Attach Poster popup

        #region Attach Banner popup

        private void AttachBanner_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.InitialDirectory = "C:\\";
            fileDialog.Filter = "Images (*.jpg;*.png;*.bmp | *.jpg;*.png;*.bmp";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true)
            {
                string ngBannerFile = fileDialog.FileName;
                NewGameBanner.Text = ngBannerFile;
            }
        }

        #endregion Attach Banner popup
    }
}