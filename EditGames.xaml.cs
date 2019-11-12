using GameLauncher.Properties;
using GameLauncher.ViewModels;
using IWshRuntimeLibrary;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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
        public int offset;
        public string installPath = AppDomain.CurrentDomain.BaseDirectory;

        public EditGames()
        {
            InitializeComponent();
            if (Settings.Default.theme.ToString() == "Dark")
            {
                ThemeAssist.SetTheme(this, BaseTheme.Dark);
            }
            else if (Settings.Default.theme.ToString() == "Light")
            {
                ThemeAssist.SetTheme(this, BaseTheme.Light);
            }
            installPath = installPath.Replace("\\", "/");
        }

        private void EditGame_OnClick(object sender, RoutedEventArgs e)
        {
            //Need to change shortcut name in this bit
            string ngl = EditLink.Text;
            if (!ngl.Contains("http") && (ngl != ""))
            {
                UriBuilder uriBuilder = new UriBuilder
                {
                    Scheme = "http",
                    Host = EditLink.Text
                };
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
                    if (!alltitles.Contains(NewTitle + " |"))
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
                            Trace.WriteLine(DateTime.Now + ": EditGame: " + ex.Message);
                        }
                        clearFields();
                        ClearGenreBoxes();
                        alltitles = null;
                        OldTitle = null;
                        ModifyFile.RemoveGameFromFile(guid);
                        ((MainWindow)Application.Current.MainWindow)?.RefreshGames();
                        ((MainWindow)Application.Current.MainWindow).isDialogOpen = false;
                        EditGameDialog.IsOpen = false;
                    }
                    else
                    {
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
                            Trace.WriteLine(DateTime.Now + ": EditGame: " + ex.Message);
                        }
                        clearFields();
                        ClearGenreBoxes();
                        alltitles = null;
                        OldTitle = null;
                        ModifyFile.RemoveGameFromFile(guid);
                        ((MainWindow)Application.Current.MainWindow)?.RefreshGames();
                        ((MainWindow)Application.Current.MainWindow).isDialogOpen = false;
                        EditGameDialog.IsOpen = false;
                    }
                }
                catch (Exception ex) { Trace.WriteLine(DateTime.Now + ": EditGame2: " + ex); }
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
                oldicon = OldTitle + "-icon.png";
                newicon = EditTitle.Text + "-icon.png";
                if (System.IO.File.Exists(installPath + "Resources/img/" + oldicon))
                {
                    EditIcon.Text = EditIcon.Text.Replace(OldTitle, NewTitle);
                    System.IO.File.Copy(installPath + "Resources/img/" + oldicon, installPath + "Resources/img/" + newicon, true);
                    System.IO.File.Delete(installPath + "Resources/img/" + oldicon);
                }
                oldposter = OldTitle + "-poster.png";
                newposter = EditTitle.Text + "-poster.png";
                if (System.IO.File.Exists(installPath + "Resources/img/" + oldposter))
                {
                    EditPoster.Text = EditPoster.Text.Replace(OldTitle, NewTitle);
                    System.IO.File.Copy(installPath + "Resources/img/" + oldposter, installPath + "Resources/img/" + newposter, true);
                    System.IO.File.Delete(installPath + "Resources/img/" + oldposter);
                }
                oldbanner = OldTitle + "-banner.png";
                newbanner = EditTitle.Text + "-banner.png";
                if (System.IO.File.Exists(installPath + "Resources/img/" + oldbanner))
                {
                    EditBanner.Text = EditBanner.Text.Replace(OldTitle, NewTitle);
                    System.IO.File.Copy(installPath + "Resources/img/" + oldbanner, installPath + "Resources/img/" + newbanner, true);
                    System.IO.File.Delete(installPath + "Resources/img/" + oldbanner);
                }
                oldshortcut = OldTitle + ".lnk";
                newshortcut = EditTitle.Text + ".lnk";
                if (System.IO.File.Exists(installPath + "Resources/img/" + oldshortcut))
                {
                    EditPath.Text = EditPath.Text.Replace(OldTitle, NewTitle);
                    System.IO.File.Copy(installPath + "Resources/img/" + oldshortcut, installPath + "Resources/img/" + newshortcut, true);
                    System.IO.File.Delete(installPath + "Resources/img/" + oldshortcut);
                }
            }
        }
        private void CancelEditGame_OnClick(object sender, RoutedEventArgs e)
        {
            EditGameDialog.IsOpen = false;
            ((MainWindow)Application.Current.MainWindow).isDialogOpen = false;
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
                try
                {
                    CheckBox cb = c.ContentTemplate.FindName("genreCheckBox", c) as CheckBox;
                    if (cb.IsChecked.Value)
                    {
                        genrePlaceHolder += cb.Content.ToString() + ";";
                    }
                }
                catch (Exception exc) { Trace.WriteLine(DateTime.Now + ": EditGenre: " + exc); }
            }
            EditGenre.Text = genrePlaceHolder;
            return;
        }

        private void ClearGenreSelection_OnClick(object sender, RoutedEventArgs e)
        {
            ClearGenreBoxes();
        }

        private void CheckGenreBoxes(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < GenreAGList.Items.Count; i++)
            {
                ContentPresenter c = (ContentPresenter)GenreAGList.ItemContainerGenerator.ContainerFromItem(GenreAGList.Items[i]);
                if (c != null)
                {
                    try
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
                    catch (Exception exce) { Trace.WriteLine(DateTime.Now + ": CheckGenreBox: " + exce); }
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
                    catch (Exception e) { Trace.WriteLine(DateTime.Now + ": ClearGenreBox: " + e); }
                }
            }
        }

        private void AttachLauncher_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                RestoreDirectory = true,
                Filter = "Executable Files (*.exe) | *.exe;*.lnk;*.url"
            };
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true && EditTitle.Text != "")
            {
                CreateShortcut(fileDialog.FileName);
                string installPath = AppDomain.CurrentDomain.BaseDirectory;
                string ngNewShortcut = installPath + "Resources/shortcuts/" + EditTitle.Text + ".lnk";
                EditPath.Text = EditTitle.Text + ".lnk";
            }
            else if (dialogResult == true && EditTitle.Text == "")
            {
                MessageBox.Show("Please enter a game title first.");
            }
        }

        private void AttachIcon_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                RestoreDirectory = true,
                Filter = "Images (*.jpg;*.png;*.bmp | *.jpg;*.png;*.bmp"
            };
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true && EditTitle.Text != "")
            {
                if (EditTitle.Text.Contains(":")) { edittitle = EditTitle.Text.Replace(":", " -"); }
                else if (EditTitle.Text.Contains(":")) { edittitle = EditTitle.Text; }
                else { edittitle = EditTitle.Text; }
                string installPath = AppDomain.CurrentDomain.BaseDirectory;
                string filedialogoutput = fileDialog.FileName;
                UpdateFile(EditTitle.Text, filedialogoutput, "icon");
                EditIcon.Text = edittitle + "-icon.png";
            }
            else if (dialogResult == true && EditTitle.Text == "")
            {
                MessageBox.Show("Please enter a game title first.");
            }
        }

        private void AttachPoster_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                RestoreDirectory = true,
                Filter = "Images (*.jpg;*.png;*.bmp | *.jpg;*.png;*.bmp"
            };
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true && EditTitle.Text != "")
            {
                if (EditTitle.Text.Contains(":")) { edittitle = EditTitle.Text.Replace(":", " -"); }
                else if (EditTitle.Text.Contains(":")) { edittitle = EditTitle.Text; }
                else { edittitle = EditTitle.Text; }
                string installPath = AppDomain.CurrentDomain.BaseDirectory;
                string filedialogoutput = fileDialog.FileName;
                UpdateFile(EditTitle.Text, filedialogoutput, "poster");
                EditPoster.Text = edittitle + "-poster.png";
            }
            else if (dialogResult == true && EditTitle.Text == "")
            {
                MessageBox.Show("Please enter a game title first.");
            }
        }

        private void AttachBanner_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Multiselect = false,
                RestoreDirectory = true,
                Filter = "Images (*.jpg;*.png;*.bmp | *.jpg;*.png;*.bmp"
            };
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true && EditTitle.Text != "")
            {
                if (EditTitle.Text.Contains(":")) { edittitle = EditTitle.Text.Replace(":", " -"); }
                else if (EditTitle.Text.Contains(":")) { edittitle = EditTitle.Text; }
                else { edittitle = EditTitle.Text; }
                string installPath = AppDomain.CurrentDomain.BaseDirectory;
                string filedialogoutput = fileDialog.FileName;
                UpdateFile(EditTitle.Text, filedialogoutput, "banner");
                EditBanner.Text = edittitle + "-banner.png";
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
            if (!Directory.Exists(installPath + "Resources\\shortcuts"))
            {
                Directory.CreateDirectory(installPath + "Resources\\shortcuts");
            }
            WshShell wsh = new WshShell();
            IWshShortcut shortcut = wsh.CreateShortcut(
                installPath + "Resources\\shortcuts" + "\\" + EditTitle.Text + ".lnk") as IWshShortcut;
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
                    gametitle = gametitle + "-icon.png";
                    string iconpath = installPath + "Resources/img/" + gametitle;
                    System.IO.File.Copy(sourcefile, iconpath, true);
                }
                else if (type == "poster")
                {
                    gametitle = gametitle + "-poster.png";
                    string posterpath = installPath + "Resources/img/" + gametitle;
                    System.IO.File.Copy(sourcefile, posterpath, true);
                }
                else if (type == "banner")
                {
                    gametitle = gametitle + "-banner.png";
                    string bannerpath = installPath + "Resources/img/" + gametitle;
                    System.IO.File.Copy(sourcefile, bannerpath, true);
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
                    oldtitle = OldTitle + "-icon.png";
                    gametitle = gametitle + "-icon.png";
                    string iconpath = installPath + "Resources/img/" + gametitle;
                    System.IO.File.Copy(installPath + "Resources/img/" + oldtitle, iconpath, true);
                }
                else if (type == "poster")
                {
                    oldtitle = OldTitle + "-poster.png";
                    gametitle = gametitle + "-poster.png";
                    string posterpath = installPath + "Resources/img/" + gametitle;
                    System.IO.File.Copy(installPath + "Resources/img/" + oldtitle, posterpath, true);
                }
                else if (type == "banner")
                {
                    oldtitle = OldTitle + "-banner.png";
                    gametitle = gametitle + "-banner.png";
                    string bannerpath = installPath + "Resources/img/" + gametitle;
                    System.IO.File.Copy(installPath + "Resources/img/" + oldtitle, bannerpath, true);
                }
                else if (type == "shortcut")
                {
                    oldtitle = OldTitle + ".lnk";
                    gametitle = gametitle + ".lnk";
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
            ls.SearchLinks(gametitle, imagetype, searchstring, offset);
            ((MainWindow)Application.Current.MainWindow)?.OpenImageDL(gametitle, searchstring, imagetype);
        }
        private void SearchPoster_OnClick(object sender, RoutedEventArgs e)
        {
            LoadSearch ls = new LoadSearch();
            string gametitle = EditTitle.Text;
            string imagetype = "poster";
            string searchstring = gametitle + " game poster";
            ls.SearchLinks(gametitle, imagetype, searchstring, offset);
            ((MainWindow)Application.Current.MainWindow)?.OpenImageDL(gametitle, searchstring, imagetype);
        }
        private void SearchBanner_OnClick(object sender, RoutedEventArgs e)
        {
            LoadSearch ls = new LoadSearch();
            string gametitle = EditTitle.Text;
            string imagetype = "banner";
            string searchstring = gametitle + " game banner";
            ls.SearchLinks(gametitle, imagetype, searchstring, offset);
            ((MainWindow)Application.Current.MainWindow)?.OpenImageDL(gametitle, searchstring, imagetype);
        }

    }
}