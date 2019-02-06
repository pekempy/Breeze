using GameLauncher.ViewModels;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using IWshRuntimeLibrary;
using GameLauncher.Views;

namespace GameLauncher
{
    public partial class EditGames : Page
    {
        private LoadAllGames lag = new LoadAllGames();
        private string guid;
        public string edittitle;
        public string NewTitle;
        public string OldTitle;
        public string oldtitle;
        public string alltitles;
        public string installPath = AppDomain.CurrentDomain.BaseDirectory;

        public EditGames()
        {
            InitializeComponent();
            installPath = installPath.Replace("\\", "/");
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
            if (System.IO.File.Exists("./Resources/GamesList.txt"))
            {
                string[] allgames = System.IO.File.ReadAllLines("./Resources/GamesList.txt");
                string[] columns = new string[0];
                int numofgames = 0;
                foreach (var item in allgames)
                {
                    columns = allgames[numofgames].Split('|');
                    string gametitle = columns[0];
                    gametitle = columns[0];
                    gametitle = gametitle.Trim().ToLower();
                    alltitles += " | " + gametitle + " | ";
                    numofgames++;
                }
                try
                {
                    RenameFiles(OldTitle, NewTitle);
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
                    alltitles = null;
                    OldTitle = null;
                    ModifyFile.RemoveGameFromFile(guid);
                    ((MainWindow)Application.Current.MainWindow)?.RefreshGames();
                    EditGameDialog.IsOpen = false;
                }
                catch (Exception ex) { Console.WriteLine("Error message EditGames.xaml.cs: " + ex); }
            }
        }
        private void RenameFiles(string OldTitle, string NewTitle)
        {
            if (OldTitle != NewTitle)
            {
                string oldicon;
                string newicon;
                string oldposter;
                string newposter;
                string oldbanner;
                string newbanner;
                string oldshortcut;
                string newshortcut;
                oldicon = installPath + "Resources/img/" + OldTitle + "-icon.png";
                newicon = installPath + "Resources/img/" + EditTitle.Text + "-icon.png";
                if (System.IO.File.Exists(oldicon))
                {
                    EditIcon.Text = EditIcon.Text.Replace(OldTitle, NewTitle);
                    System.IO.File.Copy(oldicon, newicon, true);
                    System.IO.File.Delete(oldicon);
                }
                oldposter = installPath + "Resources/img/" + OldTitle + "-poster.png";
                newposter = installPath + "Resources/img/" + EditTitle.Text + "-poster.png";
                if (System.IO.File.Exists(oldposter))
                {
                    EditPoster.Text = EditPoster.Text.Replace(OldTitle, NewTitle);
                    System.IO.File.Copy(oldposter, newposter, true);
                    System.IO.File.Delete(oldposter);
                }
                oldbanner = installPath + "Resources/img/" + OldTitle + "-banner.png";
                newbanner = installPath + "Resources/img/" + EditTitle.Text + "-banner.png";
                if (System.IO.File.Exists(oldbanner))
                {
                    EditBanner.Text = EditBanner.Text.Replace(OldTitle, NewTitle);
                    System.IO.File.Copy(oldbanner, newbanner, true);
                    System.IO.File.Delete(oldbanner);
                }
                oldshortcut = installPath + "Resources/shortcuts/" + OldTitle + ".lnk";
                newshortcut = installPath + "Resources/shortcuts/" + EditTitle.Text + ".lnk";
                if (System.IO.File.Exists(oldshortcut))
                {
                    EditPath.Text = EditPath.Text.Replace(OldTitle, NewTitle);
                    System.IO.File.Copy(oldshortcut, newshortcut, true);
                    System.IO.File.Delete(oldshortcut);
                }
            }
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
                    this.ApplyTemplate();
                    try
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
                    catch(Exception e) { Console.WriteLine("Error: " + e); }
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
                else if (EditTitle.Text.Contains(":")) { edittitle = EditTitle.Text; }
                else { edittitle = EditTitle.Text; }
                string installPath = AppDomain.CurrentDomain.BaseDirectory;
                string filedialogoutput = fileDialog.FileName;
                UpdateFile(EditTitle.Text, filedialogoutput, "icon");
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
                else if (EditTitle.Text.Contains(":")) { edittitle = EditTitle.Text; }
                else { edittitle = EditTitle.Text; }
                string installPath = AppDomain.CurrentDomain.BaseDirectory;
                string filedialogoutput = fileDialog.FileName;
                UpdateFile(EditTitle.Text, filedialogoutput, "poster");
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
            fileDialog.Multiselect = false;
            fileDialog.RestoreDirectory = true;
            fileDialog.Filter = "Images (*.jpg;*.png;*.bmp | *.jpg;*.png;*.bmp";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true && EditTitle.Text != "")
            {
                if (EditTitle.Text.Contains(":")) { edittitle = EditTitle.Text.Replace(":", " -"); }
                else if (EditTitle.Text.Contains(":")) { edittitle = EditTitle.Text; }
                else { edittitle = EditTitle.Text; }
                string installPath = AppDomain.CurrentDomain.BaseDirectory;
                string filedialogoutput = fileDialog.FileName;
                UpdateFile(EditTitle.Text, filedialogoutput, "banner");
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
        private void EditTitle_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (EditTitle.Text.IndexOfAny(Path.GetInvalidFileNameChars()) > -1)
            {
                MessageBox.Show("Unfortunately your title must be valid to save files. This means it cannot contain characters like : ? \\ \" / etc.");
            }
            else
            {
                NewTitle = EditTitle.Text;
            }
        }
        private void UpdateFile(string gametitle, string sourcefile, string type)
        {
            if (OldTitle == gametitle)
            {
                if (type == "icon")
                {
                    gametitle = installPath + "Resources/img/" + gametitle + "-icon.png";
                    System.IO.File.Copy(sourcefile, gametitle, true);
                }
                else if (type == "poster")
                {
                    gametitle = installPath + "Resources/img/" + gametitle + "-poster.png";
                    System.IO.File.Copy(sourcefile, gametitle, true);
                }
                else if (type == "banner")
                {
                    gametitle = installPath + "Resources/img/" + gametitle + "-banner.png";
                    System.IO.File.Copy(sourcefile, gametitle, true);
                }
                else if (type == "shortcut")
                {
                    gametitle = installPath + "Resources/shortcuts/" + gametitle + ".lnk";
                    System.IO.File.Copy(sourcefile, gametitle, true);
                }
            }
            else
            {
                if (type == "icon")
                {
                    oldtitle = installPath + "Resources/img/" + OldTitle + "-icon.png";
                    gametitle = installPath + "Resources/img/" + gametitle + "-icon.png";
                    System.IO.File.Copy(oldtitle, gametitle, true);
                }
                else if (type == "poster")
                {
                    oldtitle = installPath + "Resources/img/" + OldTitle + "-poster.png";
                    gametitle = installPath + "Resources/img/" + gametitle + "-poster.png";
                    System.IO.File.Copy(oldtitle, gametitle, true);
                }
                else if (type == "banner")
                {
                    oldtitle = installPath + "Resources/img/" + OldTitle + "-banner.png";
                    gametitle = installPath + "Resources/img/" + gametitle + "-banner.png";
                    System.IO.File.Copy(oldtitle, gametitle, true);
                }
                else if (type == "shortcut")
                {
                    oldtitle = installPath + "Resources/img/" + OldTitle + ".lnk";
                    gametitle = installPath + "Resources/img/" + gametitle + ".lnk";
                    System.IO.File.Copy(oldtitle, gametitle, true);
                }
            }
        }
        private void SearchIcon_OnClick(object sender, RoutedEventArgs e)
        {
            LoadSearch ls = new LoadSearch();
            string gametitle = EditTitle.Text;
            string imagetype = "icon";
            string searchstring = gametitle + " game icon";
            ls.SearchLinks(gametitle, imagetype, searchstring);
            ((MainWindow)Application.Current.MainWindow)?.OpenImageDL(gametitle, searchstring, imagetype);
        }
        private void SearchPoster_OnClick(object sender, RoutedEventArgs e)
        {
            LoadSearch ls = new LoadSearch();
            string gametitle = EditTitle.Text;
            string imagetype = "poster";
            string searchstring = gametitle + " game poster";
            ls.SearchLinks(gametitle, imagetype, searchstring);
            ((MainWindow)Application.Current.MainWindow)?.OpenImageDL(gametitle, searchstring, imagetype);
        }
        private void SearchBanner_OnClick(object sender, RoutedEventArgs e)
        {
            LoadSearch ls = new LoadSearch();
            string gametitle = EditTitle.Text;
            string imagetype = "banner";
            string searchstring = gametitle + " game banner";
            ls.SearchLinks(gametitle, imagetype, searchstring);
            ((MainWindow)Application.Current.MainWindow)?.OpenImageDL(gametitle, searchstring, imagetype);
        }

    }
}