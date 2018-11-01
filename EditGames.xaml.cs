using GameLauncher.ViewModels;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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

        private void CheckGenreBoxes()
        {
            for (int i = 0; i < GenreAGList.Items.Count; i++)
            {
                ContentPresenter c = (ContentPresenter)GenreAGList.ItemContainerGenerator.ContainerFromItem(GenreAGList.Items[i]);
                if (c != null)
                {
                    CheckBox cb = c.ContentTemplate.FindName("genreCheckBox", c) as CheckBox;
                    if (EditGenre.Text == cb.Name)
                    {
                        cb.IsChecked = true;
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
                    CheckBox cb = c.ContentTemplate.FindName("genreCheckBox", c) as CheckBox;
                    if (cb.IsChecked.Value)
                    {
                        cb.IsChecked = false;
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
                EditPath.Text = ngLauncherFile;
            }
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
                EditBanner.Text = ngBannerFile;
            }
        }

        public void currentGuid(String currentGuid)
        {
            guid = currentGuid;
        }
    }
}