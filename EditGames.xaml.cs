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
            Deletelocalfiles(edittitle);
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
                string filedialogoutput = fileDialog.FileName;
                string imgpath = installPath + "Resources/img/" + edittitle + "-poster.png";
                try { System.IO.File.Copy(filedialogoutput, imgpath, true); } //trips here if editing twice
                catch { Console.WriteLine("We've got an error! /img/ file is locked!!!!! :C "); }

                DeleteFile(edittitle, "poster"); //method to pass in games modified title, and "poster" as the edited type
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
                MainWindow.PosterViewActive("norm");
                MainWindow.pv.gameListView.ApplyTemplate();
            }
            else if (type == "banner")
            {
            }
            else if (type == "shortcut")
            {
            }
        }

        public void Deletelocalfiles(string gametitle)
        {
            MainWindow MainWindow = ((MainWindow)Application.Current.MainWindow);
            string workingfile = installPath + "Resources/working/" + gametitle + "-poster.png";
            string imgfile = installPath + "Resources/img/" + gametitle + "-poster.png";
            workingfile = workingfile.Replace("\\", "/");
            imgfile = imgfile.Replace("\\", "/");

            try
            {
                System.IO.File.Delete(workingfile);//will sometimes delete, if you select some text in poster box before closing
                System.IO.File.Copy(imgfile, workingfile, true);
            }
            catch (Exception e) { Console.WriteLine("We've got an error! File is locked :'( "); }

            MainWindow.PosterViewActive("working");
            MainWindow.pv.gameListView.ApplyTemplate();
        }
    }
}