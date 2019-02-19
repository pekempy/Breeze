using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace GameLauncher.Models
{
    class ExeSearch
    {
        private MainWindow mw = ((MainWindow)Application.Current.MainWindow);
        public List<string> steamGameDirs = new List<string>();
        public ObservableCollection<GameExecutables> Exes { get; set; }
        private ObservableCollection<GameExecutables> exes = new ObservableCollection<GameExecutables>();

        public void SearchForShortcuts()
        {
            SearchSteam();
            SearchOrigin();
            SearchUPlay();
        }
        

        public void SearchSteam()
        {
            steamGameDirs.Clear();
            string steam32 = "SOFTWARE\\VALVE\\";
            string steam64 = "SOFTWARE\\Wow6432Node\\Valve\\";
            string steam32path;
            string steam64path;
            string config32path;
            string config64path;
            RegistryKey key32 = Registry.LocalMachine.OpenSubKey(steam32);
            RegistryKey key64 = Registry.LocalMachine.OpenSubKey(steam64);
            if (key64.ToString() == null || key64.ToString() == "")
            {
                foreach (string k32subKey in key32.GetSubKeyNames())
                {
                    using (RegistryKey subKey = key32.OpenSubKey(k32subKey))
                    {
                        steam32path = subKey.GetValue("InstallPath").ToString();
                        config32path = steam32path + "/steamapps/libraryfolders.vdf";
                        string driveRegex = @"[A-Z]:\\";
                        if (File.Exists(config32path))
                        {
                            string[] configLines = File.ReadAllLines(config32path);
                            foreach (var item in configLines)
                            {
                                Console.WriteLine("32:  " + item);
                                Match match = Regex.Match(item, driveRegex);
                                if (item != string.Empty && match.Success)
                                {
                                    string matched = match.ToString();
                                    string item2 = item.Substring(item.IndexOf(matched));
                                    item2 = item2.Replace("\\\\", "\\");
                                    item2 = item2.Replace("\"", "\\steamapps\\common\\");
                                    steamGameDirs.Add(item2);
                                }
                            }
                            steamGameDirs.Add(steam32path + "\\steamapps\\common\\");
                        }
                    }
                }
            }
            foreach(string k64subKey in key64.GetSubKeyNames())
            {
                using (RegistryKey subKey = key64.OpenSubKey(k64subKey))
                {
                    steam64path = subKey.GetValue("InstallPath").ToString();
                    config64path = steam64path + "/steamapps/libraryfolders.vdf";
                    string driveRegex = @"[A-Z]:\\";
                    if (File.Exists(config64path))
                    {
                        string[] configLines = File.ReadAllLines(config64path);
                        foreach (var item in configLines)
                        {
                            Console.WriteLine("64:  " + item);
                            Match match = Regex.Match(item, driveRegex);
                            if(item != string.Empty && match.Success)
                            {
                                string matched = match.ToString();
                                string item2 = item.Substring(item.IndexOf(matched));
                                item2 = item2.Replace("\\\\", "\\");
                                item2 = item2.Replace("\"", "\\steamapps\\common\\");
                                steamGameDirs.Add(item2);
                            }
                        }
                        steamGameDirs.Add(steam64path + "\\steamapps\\common\\");
                    }
                }
            }

            foreach(string item in steamGameDirs)
            {
                string GameTitle;
                string Exe1;
                string Exe2;
                string Exe3;
                string Exe4;
                string Exe5;
                string Exe6;
                string[] Executables = new string[0];
                string[] steamGames = Directory.GetDirectories(item);
                foreach (var dir in steamGames)
                {
                    GameTitle = null;  Exe1 = null; Exe2 = null; Exe3 = null; Exe4 = null; Exe5 = null; Exe6 = null;
                    string title = dir.Substring(dir.IndexOf("\\common\\"));
                    string[] titlex = title.Split('\\');
                    title = titlex[2].ToString();
                    GameTitle = title;
                    Console.WriteLine("Title: " + GameTitle);
                    Console.WriteLine("Directory: " + dir);
                    string[] executables = Directory.GetFiles(dir, "*.exe");
                    int num = 1;
                    foreach (var ex in executables)
                    {
                        //add "ex" to Executables[] if poss? lol
                        Console.WriteLine("Executable: " + ex);
                        if (num == 1) { Exe1 = ex; }
                        if (num == 2) { Exe2 = ex; }
                        if (num == 3) { Exe3 = ex; }
                        if (num == 4) { Exe4 = ex; }
                        if (num == 5) { Exe5 = ex; }
                        if (num == 6) { Exe6 = ex; }
                        num++;
                    }
                    if (GameTitle != "Steamworks Shared")
                    {
                        exes.Add(new GameExecutables
                        {
                            Title = GameTitle,
                            Exe1 = Exe1,
                            Exe2 = Exe2,
                            Exe3 = Exe3,
                            Exe4 = Exe4,
                            Exe5 = Exe5,
                            Exe6 = Exe6,

                        });
                    }
                }
                    
            }
        }
        
        public void SearchOrigin()
        {

        }

        public void SearchUPlay()
        {

        }
    }
}