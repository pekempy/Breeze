using GameLauncher.ViewModels;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using IWshRuntimeLibrary;

namespace GameLauncher
{
    public partial class EditGames : Page
    {
        private string guid;

        public EditGames()
        {
            InitializeComponent();
        }

        private void EditGame_OnClick(object sender, RoutedEventArgs e)
        {
            //This part repairs the link so it launches properly
            string ngl = EditLink.Text;
            if (!ngl.Contains("http") && (ngl != ""))
            {
                UriBuilder uriBuilder = new UriBuilder();
                uriBuilder.Scheme = "http";
                uriBuilder.Host = EditLink.Text;
                Uri uri = uriBuilder.Uri;
                EditLink.Text = uri.ToString();
            }
            //Write all the fields to the text file
            try
            {
                TextWriter tsw = new StreamWriter(@"./Resources/GamesList.txt", true);
                tsw.WriteLine(EditTitle.Text + "|" +
                              EditGenre.Text + "|" +
                              EditPath.Text + "|" +
                              EditLink.Text + "|" +
                              EditIcon.Text + "|" +
                              EditPoster.Text + "|" +
                              EditBanner.Text + "|" +
                              Guid.NewGuid());
                tsw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }

            clearFields();
            ClearGenreBoxes();
            ModifyFile.RemoveGameFromFile(guid);
            ((MainWindow)Application.Current.MainWindow)?.RefreshGames();
            EditGameDialog.IsOpen = false;
        }

        private void CancelEditGame_OnClick(object sender, RoutedEventArgs e)
        {
            EditGameDialog.IsOpen = false;
            ClearGenreBoxes();
            clearFields();
        }

        private void clearFields()
        {
            EditTitle.Text = "";
            EditPath.Text = "";
            EditGenre.Text = "";
            EditLink.Text = "";
            EditIcon.Text = "";
            EditPoster.Text = "";
            EditBanner.Text = "";
        }

        private void EditGenre_OnClick(object sender, RoutedEventArgs e)
        {
            string genrePlaceHolder = null;
            for (int i = 0; i < GenreAGList.Items.Count; i++)
            {
                ContentPresenter c = (ContentPresenter)GenreAGList.ItemContainerGenerator.ContainerFromItem(GenreAGList.Items[i]);
                CheckBox cb = c.ContentTemplate.FindName("genreCheckBox", c) as CheckBox;
                if (cb.IsChecked.Value)
                {
                    genrePlaceHolder += cb.Content.ToString() + " ";
                }
            }
            if (genrePlaceHolder != null)
            {
                genrePlaceHolder = genrePlaceHolder.Replace(" ", "; ");
                genrePlaceHolder = genrePlaceHolder.TrimEnd(' ');
                genrePlaceHolder = genrePlaceHolder.TrimEnd(';');
            }
            EditGenre.Text = genrePlaceHolder;
            return;
        }

        private void ClearGenreSelection_OnClick(object sender, RoutedEventArgs e)
        {
            ClearGenreBoxes();
        }

        //CheckGenreBoxes currently linked to a button - NEED THIS TO BE AUTO WHEN DIALOG OPENS
        private void CheckGenreBoxes(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < GenreAGList.Items.Count; i++)
            {
                ContentPresenter c = (ContentPresenter)GenreAGList.ItemContainerGenerator.ContainerFromItem(GenreAGList.Items[i]);
                if (c != null)
                {
                    CheckBox cb = c.ContentTemplate.FindName("genreCheckBox", c) as CheckBox;
                    if (EditGenre.Text.Contains(cb.Content.ToString()))
                    {
                        cb.IsChecked = true;
                    }
                    else if (!EditGenre.Text.Contains(cb.Content.ToString()))
                    {
                        cb.IsChecked = false;
                    }
                }
            }
        }

        private void ClearGenreBoxes()
        {
            for (int i = 0; i < GenreAGList.Items.Count; i++)
            {
                ContentPresenter c = (ContentPresenter)GenreAGList.ItemContainerGenerator.ContainerFromItem(GenreAGList.Items[i]);
                if (c != null)
                {
                    if (c.ContentTemplate.FindName("genreCheckBox", c) != null)
                    {
                        CheckBox cb = c.ContentTemplate.FindName("genreCheckBox", c) as CheckBox;
                        if (cb.IsChecked.Value)
                        {
                            cb.IsChecked = false;
                        }
                    }
                }
            }
        }

        private void AttachLauncher_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.RestoreDirectory = true;
            fileDialog.Filter = "Executable Files (*.exe) | *.exe;*.lnk;*.url";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true)
            {
                string ngLauncherFile = fileDialog.FileName;
                CreateShortcutForLauncher(ngLauncherFile);
                EditPath.Text = ngLauncherFile;
            }
        }

        public string ngLauncherShortcut;

        private void CreateShortcutForLauncher(string inputFile)
        {
            ngLauncherShortcut = inputFile;
        }

        private void AttachIcon_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.RestoreDirectory = true;
            fileDialog.Filter = "Images (*.jpg;*.png;*.bmp | *.jpg;*.png;*.bmp";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true)
            {
                string ngIconFile = fileDialog.FileName;
                //File.Copy(ngIconFile, @"./Resources/img/" + EditTitle.Text + "-icon.png");
                //NewGameIcon.Text = @"./Resources/img/" + EditTitle.Text + "-icon.png";
                EditIcon.Text = ngIconFile;
            }
        }

        private void AttachPoster_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.RestoreDirectory = true;
            fileDialog.Filter = "Images (*.jpg;*.png;*.bmp | *.jpg;*.png;*.bmp";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true)
            {
                string ngPosterFile = fileDialog.FileName;
                //File.Copy(ngIconFile, @"./Resources/img/" + EditTitle.Text + "-poster.png");
                //NewGameIcon.Text = @"./Resources/img/" + EditTitle.Text + "-poster.png";
                EditPoster.Text = ngPosterFile;
            }
        }

        private void AttachBanner_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.RestoreDirectory = true;
            fileDialog.Multiselect = false;
            fileDialog.Filter = "Images (*.jpg;*.png;*.bmp | *.jpg;*.png;*.bmp";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true)
            {
                string ngBannerFile = fileDialog.FileName;
                //File.Copy(ngIconFile, @"./Resources/img/" + EditTitle.Text + "-banner.png");
                //NewGameIcon.Text = @"./Resources/img/" + EditTitle.Text + "-banner.png";
                EditBanner.Text = ngBannerFile;
            }
        }

        public void currentGuid(string currentGuid)
        {
            guid = currentGuid;
        }
    }
}