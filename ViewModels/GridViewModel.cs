using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using GameLauncher.Models;
using GameLauncher.Views;

namespace GameLauncher.ViewModels
{
    class GridViewModel
    {
        public ObservableCollection<GameList> GridView { get; set; }

        public void LoadGames()
        {
            ObservableCollection<GameList> games = new ObservableCollection<GameList>();
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
                    columns = gamesArr[numberOfGames].Split('|');
                    games.Add(new GameList
                    {
                        Title = columns[0],
                        Genre = columns[1],
                        Path = columns[2],
                        Link = columns[3],
                        Icon = columns[4],
                        Poster = columns[5],
                        Banner = columns[6],
                    });
                    numberOfGames++;
                }
            }
            GridView = games;
        }
    }
}
