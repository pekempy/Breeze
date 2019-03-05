using GameLauncher.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
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
        public void LoadGames()
        {
            try { games.Clear(); } catch { }
            try { Games.Clear(); } catch { }
            ReadFile();
            Games = games;
        }

        public void LoadGenres()
        {
            if (File.Exists("./Resources/GenreList.txt"))
            {
                string genreFile = "./Resources/GenreList.txt";
                string[] genresArr = File.ReadAllLines(genreFile);
                string[] columns = new string[0];
                int numberOfGenres = 0;
                foreach (var item in genresArr)
                {
                    columns = genresArr[numberOfGenres].Split('|');
                    genres.Add(new GenreList
                    {
                        Name = columns[0],
                        Guid = columns[1]
                    });
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
                foreach (var item in File.ReadLines(gameFile))
                {
                    columns = item.Split('|');
                    //these lines convert the strings to bitmapimages
                        if (columns[4] != "")
                        {
                            icon = new BitmapImage();
                            icon.BeginInit();
                            icon.UriSource = new Uri(columns[4]);
                            icon.DecodePixelWidth = 80;
                            icon.CacheOption = BitmapCacheOption.OnLoad;
                            icon.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                            icon.EndInit();
                        }
                        if (columns[5] != "")
                        {
                            poster = new BitmapImage();
                            poster.BeginInit();
                            poster.UriSource = new Uri(columns[5]);
                            poster.DecodePixelWidth = 200;
                            poster.CacheOption = BitmapCacheOption.OnLoad;
                            poster.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                            poster.EndInit();
                        }
                        if (columns[6] != "")
                        {
                            banner = new BitmapImage();
                            banner.BeginInit();
                            banner.UriSource = new Uri(columns[6]);
                            banner.DecodePixelWidth = 300;
                            banner.CacheOption = BitmapCacheOption.OnLoad;
                            banner.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                            banner.EndInit();
                        }
                    games.Add(new GameList
                    {
                        Title = columns[0],
                        Genre = columns[1],
                        Path = columns[2],
                        Link = columns[3],
                        Icon = icon,
                        Poster = poster,
                        Banner = banner,
                        Guid = columns[7]
                    });
                    icon = null;
                    poster = null;
                    banner = null;
                }
            }
        }
    }
}
