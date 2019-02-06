using GameLauncher.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
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

        public void LoadGames()
        {
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
                string[] gamesArr = File.ReadAllLines(gameFile);
                string[] columns = new string[0];
                int numberOfGames = 0;
                foreach (var item in gamesArr)
                {
                    columns = gamesArr[numberOfGames].Split('|');
                    //these lines convert the strings to bitmapimages
                    BitmapImage icon = new BitmapImage();
                    BitmapImage poster = new BitmapImage();
                    BitmapImage banner = new BitmapImage();
                    icon.BeginInit();
                    poster.BeginInit();
                    banner.BeginInit();
                    icon.UriSource = new Uri(columns[4]);
                    poster.UriSource = new Uri(columns[5]);
                    banner.UriSource = new Uri(columns[6]);
                    icon.DecodePixelWidth = 200;
                    poster.DecodePixelWidth = 200;
                    banner.DecodePixelWidth = 200;
                    icon.CacheOption = BitmapCacheOption.OnLoad;
                    poster.CacheOption = BitmapCacheOption.OnLoad;
                    banner.CacheOption = BitmapCacheOption.OnLoad;
                    icon.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    poster.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    banner.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                    icon.EndInit();
                    poster.EndInit();
                    banner.EndInit();

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
                    numberOfGames++;
                }
            }
        }
    }
}
