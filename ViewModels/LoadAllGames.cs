using GameLauncher.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GameLauncher.ViewModels
{
    public class LoadAllGames
    {
        public ObservableCollection<GameList> Games { get; set; }
        public ObservableCollection<GenreList> Genres { get; set; }
        private ObservableCollection<GameList> games = new ObservableCollection<GameList>();
        private ObservableCollection<GenreList> genres = new ObservableCollection<GenreList>();
        private MainWindow MainWindow = ((MainWindow)Application.Current.MainWindow);
        public BitmapImage icon;
        public BitmapImage poster;
        public BitmapImage banner;
        public string title;
        public string path;
        public string genre;
        public string link;
        public string guid;
        public string genreName;
        public string genreGuid;
        public int percentage;

        public void LoadGames()
        {
            try { games.Clear(); } catch { }
            try { Games.Clear(); } catch { }
            ReadFile();
            Games = games;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            ((MainWindow)Application.Current.MainWindow).RefreshDataContext()));
        }

        public void LoadGenres()
        {
            try { Genres.Clear(); } catch { }
            try { genres.Clear(); } catch { }
            if (File.Exists("./Resources/GenreList.txt"))
            {
                string genreFile = "./Resources/GenreList.txt";
                string[] genresArr = File.ReadAllLines(genreFile);
                string[] columns = new string[0];
                int numberOfGenres = 0;
                foreach (var item in genresArr)
                {
                    columns = genresArr[numberOfGenres].Split('|');
                    genreName = columns[0];
                    genreGuid = columns[1];
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    AddGenresToOC()));
                    genreName = null;
                    genreGuid = null;
                    numberOfGenres++;
                }
            }
            Genres = genres;
        }

        public void ReadFile()
        {
            if (File.Exists("./Resources/GamesList.txt"))
            {
                string gameFile = "./Resources/GamesList.txt";
                string[] columns = new string[0];
                int itemcount = 0;
                int GameCount = File.ReadLines(gameFile).Count();
                foreach (var item in File.ReadAllLines(gameFile))
                {
                    columns = item.Split('|');
                    if (columns[4] != "")
                    {
                        try
                        {
                            string installDir = AppDomain.CurrentDomain.BaseDirectory;
                            string iconpath = installDir + "Resources/img/" + columns[4];
                            icon = new BitmapImage();
                            icon.BeginInit();
                            icon.UriSource = new Uri(iconpath);
                            icon.DecodePixelWidth = 80;
                            icon.CacheOption = BitmapCacheOption.OnLoad;
                            icon.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                            icon.EndInit();
                            icon.Freeze();
                        }
                        catch (Exception e) { Trace.WriteLine("Error saving image (Icon): " + e); }
                    }
                    if (columns[5] != "")
                    {
                        try
                        {
                            string installDir = AppDomain.CurrentDomain.BaseDirectory;
                            string posterpath = installDir + "Resources/img/" + columns[5];
                            poster = new BitmapImage();
                            poster.BeginInit();
                            poster.UriSource = new Uri(posterpath);
                            poster.DecodePixelWidth = 200;
                            poster.CacheOption = BitmapCacheOption.OnLoad;
                            poster.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                            poster.EndInit();
                            poster.Freeze();
                        }
                        catch (Exception e) { Trace.WriteLine("Error saving image (Poster): " + e); }
                    }
                    if (columns[6] != "")
                    {
                        try
                        {
                            string installDir = AppDomain.CurrentDomain.BaseDirectory;
                            string columnpath = installDir + "Resources/img/" + columns[6];
                            banner = new BitmapImage();
                            banner.BeginInit();
                            banner.UriSource = new Uri(columnpath);
                            banner.DecodePixelWidth = 300;
                            banner.CacheOption = BitmapCacheOption.OnLoad;
                            banner.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                            banner.EndInit();
                            banner.Freeze();
                        }
                        catch (Exception e) { Trace.WriteLine("Error saving image (Banner): " + e); }
                    }
                    itemcount++;
                    title = columns[0];
                    genre = columns[1];
                    path = columns[2];
                    link = columns[3];
                    guid = columns[7];
                    double percent = itemcount / GameCount;
                    percentage = Convert.ToInt32(percent);
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    AddGameToOC()));
                    icon = null;
                    poster = null;
                    banner = null;
                }
            }
        }
        public void AddGameToOC()
        {
            string threadState = Thread.CurrentThread.IsBackground.ToString();
            games.Add(new GameList
            {
                Title = title,
                Genre = genre,
                Path = path,
                Link = link,
                Icon = icon,
                Poster = poster,
                Banner = banner,
                Guid = guid
            });
        }
        public void AddGenresToOC()
        {
            genres.Add(new GenreList
            {
                Name = genreName,
                Guid = genreGuid
            });
        }
    }
}
