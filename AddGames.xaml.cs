using GameLauncher.ViewModels;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using IWshRuntimeLibrary;
using MaterialDesignThemes.Wpf;
using GameLauncher.Properties;
using System.Diagnostics;
using GameLauncher.Models;

namespace GameLauncher
{
    public partial class AddGames : Page
    {
        private LoadAllGames lag = new LoadAllGames();
        public string alltitles;
        public int offset = 0;
        public AddGames()
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
        }

        private void AddGame_OnClick(object sender, RoutedEventArgs e)
        {
            if (NewGameTitle.Text != "")
            {
                //This part repairs the link so it launches properly
                string ngl = NewGameLink.Text;
                if (!ngl.Contains("http") && (ngl != ""))
                {
                    UriBuilder uriBuilder = new UriBuilder();
                    uriBuilder.Scheme = "http";
                    uriBuilder.Host = NewGameLink.Text;
                    Uri uri = uriBuilder.Uri;
                    NewGameLink.Text = uri.ToString();
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
                    if (alltitles.Contains(" | " + NewGameTitle.Text.Trim().ToLower() + " | "))
                    {
                        MessageBox.Show("A game with this title already exists");
                        NewGameTitle.Text = "";
                    }
                    else
                    {
                        try
                        {
                            TextWriter tsw = new StreamWriter(@"./Resources/GamesList.txt", true);
                            Guid gameGuid = Guid.NewGuid();
                            tsw.WriteLine(NewGameTitle.Text + "|" +
                                          NewGameGenre.Text + "|" +
                                          NewGamePath.Text + "|" +
                                          NewGameLink.Text + "|" +
                                          NewGameIcon.Text + "|" +
                                          NewGamePoster.Text + "|" +
                                          NewGameBanner.Text + "|" +
                                          gameGuid);
                            tsw.Close();
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(DateTime.Now + ": AddGameOnClick: " + ex.Message);
                        }
                        Trace.WriteLine(DateTime.Now + ": Added Game manually: " + NewGameTitle.Text);
                        ClearGenreBoxes();
                        clearFields();
                        ((MainWindow)Application.Current.MainWindow)?.RefreshGames();
                        ((MainWindow)Application.Current.MainWindow).isDialogOpen = false;
                        AddGameDialog.IsOpen = false;
                    }
                }
                else

                {
                    try
                    {
                        TextWriter tsw = new StreamWriter(@"./Resources/GamesList.txt", true);
                        Guid gameGuid = Guid.NewGuid();
                        tsw.WriteLine(NewGameTitle.Text + "|" +
                                      NewGameGenre.Text + "|" +
                                      NewGamePath.Text + "|" +
                                      NewGameLink.Text + "|" +
                                      NewGameIcon.Text + "|" +
                                      NewGamePoster.Text + "|" +
                                      NewGameBanner.Text + "|" +
                                      gameGuid);
                        tsw.Close();
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine(DateTime.Now + ": AddGameOnClick2: " + ex.Message);
                    }
                    ClearGenreBoxes();
                    clearFields();
                    ((MainWindow)Application.Current.MainWindow)?.RefreshGames();
                    AddGameDialog.IsOpen = false;

                }
            }
            else
            {
                MessageBox.Show("Please enter a game title first.");
            }
        }

        private void CancelAddGame_OnClick(object sender, RoutedEventArgs e)
        {
            ClearGenreBoxes();
            ((MainWindow)Application.Current.MainWindow).isDialogOpen = false;
            AddGameDialog.IsOpen = false;
            clearFields();
        }

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

        private void TitleTextChanged(object sender, EventArgs e)
        {
            if ( NewGameTitle.Text.IndexOfAny(Path.GetInvalidFileNameChars()) > -1)
            {
                MessageBox.Show("Unfortunately your title must be valid to save files. This means it cannot contain characters like : ? \\ / etc.");
            }
        }

        private void AddGenre_OnClick(object sender, RoutedEventArgs e)
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
                catch (Exception ex) { Trace.WriteLine(DateTime.Now + ": AddGenre: " + ex); }
            }
            NewGameGenre.Text = genrePlaceHolder;
            return;
        }

        private void ClearGenreSelection_OnClick(object sender, RoutedEventArgs e)
        {
            ClearGenreBoxes();
        }

        private void ClearGenreBoxes()
        {
            for (int i = 0; i < GenreAGList.Items.Count; i++)
            {
                ContentPresenter c = (ContentPresenter)GenreAGList.ItemContainerGenerator.ContainerFromItem(GenreAGList.Items[i]);
                if (c != null)
                {
                    try
                    {
                        CheckBox cb = c.ContentTemplate.FindName("genreCheckBox", c) as CheckBox;
                        if (cb.IsChecked.Value)
                        {
                            cb.IsChecked = false;
                        }
                    }
                    catch (Exception e) { Trace.WriteLine(DateTime.Now + ": ClearGenre: " + e); }
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
            if (dialogResult == true && NewGameTitle.Text != "")
            {
                newgametitle = NewGameTitle.Text.Replace(":", " -");
                CreateShortcut(fileDialog.FileName);
                string installPath = AppDomain.CurrentDomain.BaseDirectory;
                installPath = installPath.Replace("\\", "/");
                string ngNewShortcut = installPath + "Resources/shortcuts/" + newgametitle + ".lnk";
                NewGamePath.Text = newgametitle + ".lnk";
            }
            else if (dialogResult == true && NewGameTitle.Text == "")
            {
                MessageBox.Show("Please enter a game title first.");
            }
        }

        public string newgametitle;

        private void AttachIcon_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.RestoreDirectory = true;
            fileDialog.Filter = "Images (*.jpg;*.png;*.bmp) | *.jpg;*.png;*.bmp";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true && NewGameTitle.Text != "")
            {
                string installPath = AppDomain.CurrentDomain.BaseDirectory;
                installPath = installPath.Replace("\\", "/");
                string ngIconFile = fileDialog.FileName;
                newgametitle = NewGameTitle.Text.Replace(":", " -");
                System.IO.File.Copy(ngIconFile, @"./Resources/img/" + newgametitle + "-icon.png", true);
                NewGameIcon.Text = newgametitle + "-icon.png";
            }
            else if (dialogResult == true && newgametitle == "")
            {
                MessageBox.Show("Please enter a game title first.");
            }
        }

        private void AttachPoster_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            fileDialog.RestoreDirectory = true;
            fileDialog.Filter = "Images (*.jpg;*.png;*.bmp) | *.jpg;*.png;*.bmp";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true && NewGameTitle.Text != "")
            {
                string installPath = AppDomain.CurrentDomain.BaseDirectory;
                installPath = installPath.Replace("\\", "/");
                string ngPosterFile = fileDialog.FileName;
                newgametitle = NewGameTitle.Text.Replace(":", " -"); //this line needs to be used to block any chars that cant be used
                System.IO.File.Copy(ngPosterFile, @"./Resources/img/" + newgametitle + "-poster.png", true);
                NewGamePoster.Text = newgametitle + "-poster.png";
            }
            else if (dialogResult == true && NewGameTitle.Text == "")
            {
                MessageBox.Show("Please enter a game title first.");
            }
        }

        private void AttachBanner_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.RestoreDirectory = true;
            fileDialog.Multiselect = false;
            fileDialog.Filter = "Images (*.jpg;*.png;*.bmp) | *.jpg;*.png;*.bmp";
            var dialogResult = fileDialog.ShowDialog();
            if (dialogResult == true && NewGameTitle.Text != "")
            {
                string installPath = AppDomain.CurrentDomain.BaseDirectory;
                installPath = installPath.Replace("\\", "/");
                string ngBannerFile = fileDialog.FileName;
                    System.IO.File.Copy(ngBannerFile, @"./Resources/img/" + newgametitle + "-banner.png", true);
                    NewGameBanner.Text = newgametitle + "-banner.png";
            }
            else if (dialogResult == true && NewGameTitle.Text == "")
            {
                MessageBox.Show("Please enter a game title first.");
            }
        }

        private void CreateShortcut(string linkname)
        {
            newgametitle = NewGameTitle.Text.Replace(":", " -");
            string installPath = AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(installPath + "\\Resources\\shortcuts"))
            {
                Directory.CreateDirectory(installPath + "\\Resources\\shortcuts");
            }
            //create shortcut from linkname, place shortut in dir
            WshShell wsh = new WshShell();
            IWshShortcut shortcut = wsh.CreateShortcut(
                installPath + "\\Resources\\shortcuts" + "\\" + newgametitle + ".lnk") as IWshShortcut;
            shortcut.Arguments = "";
            shortcut.TargetPath = linkname;
            shortcut.WindowStyle = 1;
            shortcut.Description = "Shortcut to " + newgametitle;
            shortcut.WorkingDirectory = "C:\\App";
            shortcut.IconLocation = linkname;
            shortcut.Save();
        }
        private void SearchIcon_OnClick(object sender, RoutedEventArgs e)
        {
            LoadSearch ls = new LoadSearch();
            string gametitle = NewGameTitle.Text;
            string imagetype = "icon";
            string searchstring = gametitle + " game icon";
            ls.SearchLinks(gametitle, imagetype, searchstring, offset);
            ((MainWindow)Application.Current.MainWindow)?.OpenImageDL(gametitle, searchstring, imagetype);
        }
        private void SearchPoster_OnClick(object sender, RoutedEventArgs e)
        {
            LoadSearch ls = new LoadSearch();
            string gametitle = NewGameTitle.Text;
            string imagetype = "poster";
            string searchstring = gametitle + " game poster";
            ls.SearchLinks(gametitle, imagetype, searchstring, offset);
            ((MainWindow)Application.Current.MainWindow)?.OpenImageDL(gametitle, searchstring, imagetype);
        }
        private void SearchBanner_OnClick(object sender, RoutedEventArgs e)
        {
            LoadSearch ls = new LoadSearch();
            string gametitle = NewGameTitle.Text;
            string imagetype = "banner";
            string searchstring = gametitle + " game banner";
            ls.SearchLinks(gametitle, imagetype, searchstring, offset);
            ((MainWindow)Application.Current.MainWindow)?.OpenImageDL(gametitle, searchstring, imagetype);
        }
    }
}
