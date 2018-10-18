﻿using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using GameLauncher.Models;

namespace GameLauncher.ViewModels
{
    public class ListViewModel
    {
        public ObservableCollection<GameList> Games { get; set; }

        public void LoadGames()
        {
            ObservableCollection<GameList> games = new ObservableCollection<GameList>();
            if (File.Exists("./Resources/GamesList.txt"))
            {
                //Read file to gameFile
                string gameFile = "./Resources/GamesList.txt";
                StreamReader sr = new StreamReader(gameFile, true);
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
                sr.Close();
            }
            Games = games;
        }
    }
}