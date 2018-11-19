using GameLauncher.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GameLauncher.ViewModels
{
    public class LoadAllGames
    {
        public ObservableCollection<GameList> Games { get; set; }
        public ObservableCollection<GenreList> Genres { get; set; }
        private ObservableCollection<GameList> games = new ObservableCollection<GameList>();
        private ObservableCollection<GenreList> genres = new ObservableCollection<GenreList>();

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
                        Checked = columns[1],
                        Guid = columns[2]
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
                //Read file to gameFile
                string gameFile = "./Resources/GamesList.txt";
                //gamesArr is array containing all game info per item
                string[] gamesArr = File.ReadAllLines(gameFile);
                //columns is array containing each element of particular game
                string[] columns = new string[0];
                int numberOfGames = 0;
                foreach (var item in gamesArr)
                {
                    string installPath = AppDomain.CurrentDomain.BaseDirectory;
                    installPath = installPath.Replace("\\", "/");
                    string imgPath = installPath + "Resources/img/";
                    string shortcutPath = installPath + "Resources/shortcuts/";
                    columns = gamesArr[numberOfGames].Split('|');

                    BitmapImage icon = new BitmapImage(new Uri(columns[4], UriKind.Absolute));
                    BitmapImage poster = new BitmapImage(new Uri(columns[5], UriKind.Absolute));
                    BitmapImage banner = new BitmapImage(new Uri(columns[6], UriKind.Absolute));

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

        public void ObservableList() //Prints out contents of ObsCol
        {
            for (int i = 0; i < Games.Count; i++)
            {
                Console.WriteLine(string.Concat(Games[i].Title, " | ", Games[i].Genre));
            }
        }
    }
}