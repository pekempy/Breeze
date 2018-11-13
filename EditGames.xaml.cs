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
        private LoadAllGames lag = new LoadAllGames();
        private string guid;
        public string edittitle;
        public string installPath = AppDomain.CurrentDomain.BaseDirectory;

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
            ((MainWindow)Application.Current.MainWindow)?.RefreshGames("working");
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
                    genrePlaceHolder += cb.Content.ToString() + ";";
                }
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
            if (dialogResult == true && EditTitle.Text != "")
            {
                if (EditTitle.Text.Contains(":")) { edittitle = EditTitle.Text.Replace(":", " -"); }
                CreateShortcut(fileDialog.FileName);
                string installPath = AppDomain.CurrentDomain.BaseDirectory;
                installPath = installPath.Replace("\\", "/");
                string ngNewShortcut = installPath + "Resources/shortcuts/" + edittitle + ".lnk";
                EditPath.Text = ngNewShortcut;
            }
            else if (dialogResult == true && EditTitle.Text == "")
            {
                MessageBox.Show("Please enter a game title first.");
            }
        }

        private void AttachIcon_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.RestoreDirectory = true;
            fileDialog.Filter = "Images (*.jpg;*.png;*.bmp | *.jpg;*.png;*.bmp";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true && EditTitle.Text != "")
            {
                if (EditTitle.Text.Contains(":")) { edittitle = EditTitle.Text.Replace(":", " -"); }
                installPath = installPath.Replace("\\", "/");
                string ngIconFile = fileDialog.FileName;
                DeleteFile(ngIconFile, "icon");
                EditIcon.Text = installPath + "Resources/img/" + edittitle + "-icon.png";
            }
            else if (dialogResult == true && EditTitle.Text == "")
            {
                MessageBox.Show("Please enter a game title first.");
            }
        }

        private void AttachPoster_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.RestoreDirectory = true;
            fileDialog.Filter = "Images (*.jpg;*.png;*.bmp | *.jpg;*.png;*.bmp";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true && EditTitle.Text != "")
            {
                if (EditTitle.Text.Contains(":")) { edittitle = EditTitle.Text.Replace(":", " -"); }
                string installPath = AppDomain.CurrentDomain.BaseDirectory;
                installPath = installPath.Replace("\\", "/");
                string ngPosterFile = fileDialog.FileName;
                DeleteFile(edittitle, "poster");
                EditPoster.Text = installPath + "Resources/img/" + edittitle + "-poster.png";
            }
            else if (dialogResult == true && EditTitle.Text == "")
            {
                MessageBox.Show("Please enter a game title first.");
            }
        }

        private void AttachBanner_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.RestoreDirectory = true;
            fileDialog.Multiselect = false;
            fileDialog.Filter = "Images (*.jpg;*.png;*.bmp | *.jpg;*.png;*.bmp";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true && EditTitle.Text != "")
            {
                if (EditTitle.Text.Contains(":")) { edittitle = EditTitle.Text.Replace(":", " -"); }
                string installPath = AppDomain.CurrentDomain.BaseDirectory;
                installPath = installPath.Replace("\\", "/");
                string ngBannerFile = fileDialog.FileName;
                DeleteFile(ngBannerFile, "banner");
                EditBanner.Text = installPath + "Resources/img/" + edittitle + "-banner.png";
            }
            else if (dialogResult == true && EditTitle.Text == "")
            {
                MessageBox.Show("Please enter a game title first.");
            }
        }

        public void currentGuid(string currentGuid)
        {
            guid = currentGuid;
        }

        private void CreateShortcut(string linkname)
        {
            if (!Directory.Exists(installPath + "\\Resources\\shortcuts"))
            {
                System.IO.Directory.CreateDirectory(installPath + "\\Resources\\shortcuts");
            }
            WshShell wsh = new WshShell();
            IWshRuntimeLibrary.IWshShortcut shortcut = wsh.CreateShortcut(
                installPath + "\\Resources\\shortcuts" + "\\" + EditTitle.Text + ".lnk") as IWshRuntimeLibrary.IWshShortcut;
            shortcut.Arguments = "";
            shortcut.TargetPath = linkname;
            shortcut.WindowStyle = 1;
            shortcut.Description = "Shortcut to " + EditTitle.Text;
            shortcut.WorkingDirectory = "C:\\App";
            shortcut.IconLocation = linkname;
            shortcut.Save();
        }

        private void DeleteFile(string gametitle, string type)
        {
            //This needs to be ran in EditGame_OnClick I think?
            //1. if (type=poster) > else if (type=icon) > etc
            //2. Set particular images to be located in /resources/img instead of /resources/working (??? is this possible cos binding)
            //3. Delete image in /working
            //4. Copy file from /img to /working
            //5. Set images to be located in /working
            //potentially need to refresh the UI at times but not sure when
            PosterViewModel pvm = new PosterViewModel();
            if (type == "icon")
            {
            }
            else if (type == "poster")
            {
                MainWindow MainWindow = ((MainWindow)Application.Current.MainWindow);
                pvm.LoadGames("norm");
                //Delete images from working
                gametitle = installPath + "Resources/working/" + gametitle;
                gametitle = gametitle.Replace("\\", "/");
                string gametitlenorm = gametitle.Replace("Resources/working", "Resources/img");
                string gametitlework = gametitle.Replace("Resources/img", "Resources/working");
                //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                //Need to make the UI use /img/ first, but MainWindow is null
                //How can we refresh the UI here, and also at the v end if MW is null?
                //@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
                MainWindow.PosterViewActive("norm");//This SHOULD be settings the mainwindow context to posterview, but with the /img/ as path
                //It appears to work but still the file is in use! e.g. PosterViewOC contains "/resources/img" in the poster path
                GC.Collect();
                GC.WaitForPendingFinalizers();
                System.IO.File.Delete(gametitlework + "-poster.png");
                System.IO.File.Copy(gametitlenorm + "-poster.png", gametitlework + "-poster.png", true);
                MainWindow.PosterViewActive("working");
            }
            else if (type == "banner")
            {
            }
            else if (type == "shortcut")
            {
            }
        }
    }
}