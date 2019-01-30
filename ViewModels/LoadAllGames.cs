using GameLauncher.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;

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
                //Read file to gameFile
                string gameFile = "./Resources/GamesList.txt";
                //gamesArr is array containing all game info per item
                string[] gamesArr = File.ReadAllLines(gameFile);
                //columns is array containing each element of particular game
                string[] columns = new string[0];
                int numOfGames = 0;
                int numberOfGames = 0;
                foreach (var item in gamesArr)
                {
                    //Copy the files to a working dir, to prevent overwrites
                    columns = gamesArr[numOfGames].Split('|');
                    string gameTitle = columns[0];
                    string gameLauncher = columns[2];
                    string gameIcon = columns[4];
                    string gamePoster = columns[5];
                    string gameBanner = columns[6];
                    string fileToCopy;
                    string targetFile;
                    string installPath = AppDomain.CurrentDomain.BaseDirectory;
                    installPath = installPath.Replace("\\", "/");
                    if (!Directory.Exists(installPath + "Resources/working/")) { Directory.CreateDirectory(installPath + "Resources/working/"); }
                    if (gameTitle.Contains(":")) { gameTitle = gameTitle.Replace(":", " -"); }

                    //check if there is an icon befor doing this
                    if (gameIcon != "")
                    {
                        fileToCopy = installPath + "Resources/img/" + gameTitle + "-icon.png";
                        targetFile = installPath + "Resources/working/" + gameTitle + "-icon.png";
                        if (!File.Exists(targetFile)) { File.Copy(fileToCopy, targetFile); }
                    }
                    if (gamePoster != "")
                    {
                        fileToCopy = installPath + "Resources/img/" + gameTitle + "-poster.png";
                        targetFile = installPath + "Resources/working/" + gameTitle + "-poster.png";
                        if (!File.Exists(targetFile)) { File.Copy(fileToCopy, targetFile); }
                    }
                    if (gameBanner != "")
                    {
                        fileToCopy = installPath + "Resources/img/" + gameTitle + "-banner.png";
                        targetFile = installPath + "Resources/working/" + gameTitle + "-banner.png";
                        if (!File.Exists(targetFile)) { File.Copy(fileToCopy, targetFile); }
                    }
                    if (gameLauncher != "")
                    {
                        fileToCopy = installPath + "Resources/shortcuts/" + gameTitle + ".lnk";
                        targetFile = installPath + "Resources/working/" + gameTitle + ".lnk";
                        if (!File.Exists(targetFile)) { File.Copy(fileToCopy, targetFile); }
                    }
                    numOfGames++;
                }
                foreach (var item in gamesArr)
                {
                    string installPath = AppDomain.CurrentDomain.BaseDirectory;
                    string imgPath = "/img/";
                    string shortcutPath = "/shortcuts/";
                    string workingPath = "/working/";
                    columns = gamesArr[numberOfGames].Split('|');

                    //Fix paths
                    columns[2] = columns[2].Replace(shortcutPath, workingPath);
                    columns[4] = columns[4].Replace(imgPath, workingPath);
                    columns[5] = columns[5].Replace(imgPath, workingPath);
                    columns[6] = columns[6].Replace(imgPath, workingPath);
                    //Here, we need to somehow release the file in /working/game-x.png
                    //Then we need to overwrite it with the one from /img/game-x.png
                    //Then we need to load the game icon from /working/game-x.png again
                    //This should allow the UI to refresh the icon, as currently the /working/ dir
                    //cannot be updated while the UI is using it
                    games.Add(new GameList
                    {
                        Title = columns[0],
                        Genre = columns[1],
                        Path = columns[2],
                        Link = columns[3],
                        Icon = columns[4],
                        Poster = columns[5],
                        Banner = columns[6],
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
