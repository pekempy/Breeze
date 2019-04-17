using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace GameLauncher.Models
{
    public class ExeSearch
    {
        private MainWindow mw = ((MainWindow)Application.Current.MainWindow);
        public List<string> steamGameDirs = new List<string>();
        public List<string> originGameDirs = new List<string>();
        public List<string> battleGameDirs = new List<string>();
        public List<string> uplayGamedirs = new List<string>();
        public List<string> listOfBattleNetGames = new List<string>();
        public ObservableCollection<GameExecutables> Exes { get; set; }
        private ObservableCollection<GameExecutables> exes = new ObservableCollection<GameExecutables>();
        private ObservableCollection<GameExecutables> exes2 = new ObservableCollection<GameExecutables>();
        public string title;
        public string publisher;
        public string installLocation;
        public string exe;
        public bool duplicate = false;

        public void SearchForShortcuts()
        {
            exes.Clear();
            try { SearchSteam(); }
            catch (Exception e) { Trace.WriteLine("SearchSteam Failed: " + e); }
            try { SearchOrigin(); }
            catch (Exception e) { Trace.WriteLine("SearchOrigin Failed: " + e); }
            try { SearchUplay(); }
            catch (Exception e) { Trace.WriteLine("SearchUplay Failed: " + e); }
            try { SearchEpic(); }
            catch (Exception e) { Trace.WriteLine("SearchEpic Failed: " + e); }
            try { SearchBattle(); }
            catch (Exception e) { Trace.WriteLine("SearchBattleNet Failed: " + e); }

            var text = File.ReadAllLines("./Resources/GamesList.txt", Encoding.UTF8);
            foreach (var exeItem in exes.ToList())
            {
                for (int i = 0; i < text.Length; i++)
                {
                    string[] columns = text[i].Split('|');
                    if (columns[0] == exeItem.Title)
                    {
                        Trace.WriteLine(DateTime.Now + ": Matched exe found, skipping - " + exeItem.Title);
                        exes.Remove(exeItem);
                    }
                }
            }
            exes2 = exes;
            Exes = exes2;
        }

        public void UpdateObsCol(string title, string exe)
        {
            var item = exes.FirstOrDefault(i => i.Title == title);
            if (item != null)
            {
                item.Exe1 = exe;
                Exes = exes;
            }
        }

        public bool CheckBinding(string title)
        {
            var item = exes.FirstOrDefault(i => i.Title == title);
            if (item != null)
            {
                if (item.Exe1 != null && item.Exe2 == null)
                {
                    return true;
                }
                else { return false; }
            }
            else { return false; }
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
                                Match match = Regex.Match(item, driveRegex);
                                if (item != string.Empty && match.Success)
                                {
                                    string matched = match.ToString();
                                    string item2 = item.Substring(item.IndexOf(matched));
                                    item2 = item2.Replace("\\\\", "\\");
                                    item2 = item2.Replace("\"", "\\");
                                    if (Directory.Exists(item2 + "\\steamapps\\common"))
                                    {
                                        item2 = item2 + "steamapps\\common\\";
                                        steamGameDirs.Add(item2);
                                    }
                                }
                            }
                            steamGameDirs.Add(steam32path + "\\steamapps\\common\\");
                        }
                    }
                }
            }
            foreach (string k64subKey in key64.GetSubKeyNames())
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
                            Match match = Regex.Match(item, driveRegex);
                            if (item != string.Empty && match.Success)
                            {
                                string matched = match.ToString();
                                string item2 = item.Substring(item.IndexOf(matched));
                                item2 = item2.Replace("\\\\", "\\");
                                item2 = item2.Replace("\"", "\\");
                                if (Directory.Exists(item2 + "steamapps\\common"))
                                {
                                    item2 = item2 + "steamapps\\common\\";
                                    steamGameDirs.Add(item2);
                                }
                            }
                        }
                        steamGameDirs.Add(steam64path + "\\steamapps\\common\\");
                    }
                }
            }

            foreach (string item in steamGameDirs)
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
                    GameTitle = null; Exe1 = null; Exe2 = null; Exe3 = null; Exe4 = null; Exe5 = null; Exe6 = null;
                    string title = dir.Substring(dir.IndexOf("\\common\\"));
                    string[] titlex = title.Split('\\');
                    title = titlex[2].ToString();
                    GameTitle = title;
                    string[] executables = Directory.GetFiles(dir, "*.exe");
                    int num = 1;
                    foreach (var ex in executables)
                    {
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
                            Exe6 = Exe6

                        });
                    }
                }

            }
        }

        public void SearchOrigin()
        {
            originGameDirs.Clear();
            string regkey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(regkey);
            bool PublisherFound = false;
            foreach (string ksubKey in key.GetSubKeyNames())
            {
                using (RegistryKey subKey = key.OpenSubKey(ksubKey))
                {
                    foreach (string subkeyname in subKey.GetValueNames())
                    {
                        PublisherFound = false;
                        if (subkeyname.ToString() == "Publisher")
                        {
                            publisher = subKey.GetValue("Publisher").ToString();
                            title = subKey.GetValue("DisplayName").ToString();
                            PublisherFound = true;
                        }
                        if (subkeyname.ToString() == "InstallLocation")
                        {
                            installLocation = subKey.GetValue("InstallLocation").ToString();
                        }
                        if (PublisherFound == true)
                        {
                            if (publisher.Contains("Electronic Arts") && !title.Contains("Origin"))
                            {
                                if (originGameDirs.Count > 0)
                                {
                                    foreach (var item in originGameDirs)
                                    {
                                        if (item.Contains(title)) { duplicate = true; }
                                        else
                                        {
                                            duplicate = false;
                                        }
                                    }
                                }
                                if (duplicate == false)
                                {
                                    originGameDirs.Add(installLocation);
                                }
                                else if (duplicate == true)
                                {
                                    Trace.WriteLine(DateTime.Now + ": ExeSearch Duplicate");
                                }
                            }
                        }
                    }
                }
            }
            foreach (string item in originGameDirs)
            {
                string GameTitle;
                string Exe1;
                string Exe2;
                string Exe3;
                string Exe4;
                string Exe5;
                string Exe6;
                string[] Executables = new string[0];
                GameTitle = null; Exe1 = null; Exe2 = null; Exe3 = null; Exe4 = null; Exe5 = null; Exe6 = null;
                string[] splitTitle = item.Split('\\');
                int largest = splitTitle.Length;
                largest = largest - 2;
                title = splitTitle[largest];
                GameTitle = title;
                string[] executables = Directory.GetFiles(item, "*.exe");
                int num = 1;
                foreach (var ex in executables)
                {
                    if (num == 1) { Exe1 = ex; }
                    if (num == 2) { Exe2 = ex; }
                    if (num == 3) { Exe3 = ex; }
                    if (num == 4) { Exe4 = ex; }
                    if (num == 5) { Exe5 = ex; }
                    if (num == 6) { Exe6 = ex; }
                    num++;
                }
                exes.Add(new GameExecutables
                {
                    Title = GameTitle,
                    Exe1 = Exe1,
                    Exe2 = Exe2,
                    Exe3 = Exe3,
                    Exe4 = Exe4,
                    Exe5 = Exe5,
                    Exe6 = Exe6

                });
            }


        }

        public void SearchUplay()
        {
            uplayGamedirs.Clear();
            string regkey = "SOFTWARE\\WOW6432Node\\Ubisoft\\Launcher\\Installs";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(regkey);
            foreach (string ksubKey in key.GetSubKeyNames())
            {
                using (RegistryKey subKey = key.OpenSubKey(ksubKey))
                {
                    installLocation = subKey.GetValue("InstallDir").ToString();
                    string[] splitTitle = installLocation.Split('/');
                    int largest = splitTitle.Length;
                    largest = largest - 2;
                    title = splitTitle[largest];
                    uplayGamedirs.Add(installLocation);
                }
            }
            foreach (string item in uplayGamedirs)
            {
                string GameTitle;
                string Exe1;
                string Exe2;
                string Exe3;
                string Exe4;
                string Exe5;
                string Exe6;
                string[] Executables = new string[0];
                GameTitle = null; Exe1 = null; Exe2 = null; Exe3 = null; Exe4 = null; Exe5 = null; Exe6 = null;
                string[] splitTitle = item.Split('/');
                int largest = splitTitle.Length;
                largest = largest - 2;
                title = splitTitle[largest];
                GameTitle = title;
                string[] executables = Directory.GetFiles(item, "*.exe");
                int num = 1;
                foreach (var ex in executables)
                {
                    if (num == 1) { Exe1 = ex; }
                    if (num == 2) { Exe2 = ex; }
                    if (num == 3) { Exe3 = ex; }
                    if (num == 4) { Exe4 = ex; }
                    if (num == 5) { Exe5 = ex; }
                    if (num == 6) { Exe6 = ex; }
                    num++;
                }
                exes.Add(new GameExecutables
                {
                    Title = GameTitle,
                    Exe1 = Exe1,
                    Exe2 = Exe2,
                    Exe3 = Exe3,
                    Exe4 = Exe4,
                    Exe5 = Exe5,
                    Exe6 = Exe6
                });
            }
        }

        public void SearchEpic()
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            DirectoryInfo dir = new DirectoryInfo(desktop);
            string epicGamesDir = null;
            string epicRegistry = "SOFTWARE\\WOW6432Node\\EpicGames\\Unreal Engine";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(epicRegistry);

            foreach (string ksubkey in key.GetSubKeyNames())
            {
                using (RegistryKey subkey = key.OpenSubKey(ksubkey))
                {
                    epicGamesDir = subkey.GetValue("InstalledDirectory").ToString();
                    epicGamesDir = epicGamesDir.Substring(0, epicGamesDir.Length - 4);
                }
            }
            foreach (var file in dir.GetFiles("*.url"))
            {
                string url = desktop + "\\" + file.ToString();
                string[] l = File.ReadAllLines(url);
                foreach (var item in l)
                {
                    if (item.Contains("URL=com.epicgames.launcher://apps/"))
                    {
                        string gameName = file.ToString().Substring(0, file.ToString().Length - 4);
                        string shortcut = url;
                        string newshortcut = epicGamesDir + gameName + ".url";
                        File.Copy(url, newshortcut, true);
                        exes.Add(new GameExecutables
                        {
                            Title = gameName,
                            Exe1 = newshortcut
                        });
                    }
                }
            }
        }

        public void SearchBattle()
        {
            addBattleNetGames();
            battleGameDirs.Clear();
            string regkey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(regkey);
            foreach (string ksubKey in key.GetSubKeyNames())
            {
                using (RegistryKey subKey = key.OpenSubKey(ksubKey))
                {
                    foreach (string subkeyname in subKey.GetValueNames())
                    {
                        foreach (var game in listOfBattleNetGames)
                        {
                            string temptitle = null;
                            if (subkeyname.ToString() == "DisplayName")
                            {
                                temptitle = subKey.GetValue("DisplayName").ToString();
                                title = temptitle;
                            }
                            if (game.Contains(title))
                            {
                                Console.WriteLine(title);
                                if (battleGameDirs.Count == 0)
                                {
                                    installLocation = subKey.GetValue("InstallLocation").ToString();
                                    battleGameDirs.Add(installLocation);
                                }
                                else if (battleGameDirs.Count > 0)
                                {
                                    foreach (var item in battleGameDirs)
                                    {
                                        if (item.Contains(title)) { duplicate = true; }
                                        else { duplicate = false; }
                                    }
                                    if (duplicate == false)
                                    {
                                        installLocation = subKey.GetValue("InstallLocation").ToString();
                                        battleGameDirs.Add(installLocation);
                                    }
                                    else if (duplicate == true)
                                    {
                                        Trace.WriteLine(DateTime.Now + ": ExeSearch Duplicate");
                                    }
                                }
                            }
                        }
                    }
                }
            }
            foreach (string item in battleGameDirs)
            {
                string GameTitle;
                string Exe1;
                string Exe2;
                string Exe3;
                string Exe4;
                string Exe5;
                string Exe6;
                string[] Executables = new string[0];
                GameTitle = null; Exe1 = null; Exe2 = null; Exe3 = null; Exe4 = null; Exe5 = null; Exe6 = null;
                string[] splitTitle = item.Split('\\');
                int largest = splitTitle.Length;
                largest = largest - 1;
                title = splitTitle[largest];
                GameTitle = title;
                string[] executables = Directory.GetFiles(item, "*.exe");
                int num = 1;
                foreach (var ex in executables)
                {
                    if (num == 1) { Exe1 = ex; }
                    if (num == 2) { Exe2 = ex; }
                    if (num == 3) { Exe3 = ex; }
                    if (num == 4) { Exe4 = ex; }
                    if (num == 5) { Exe5 = ex; }
                    if (num == 6) { Exe6 = ex; }
                    num++;
                }
                exes.Add(new GameExecutables
                {
                    Title = GameTitle,
                    Exe1 = Exe1,
                    Exe2 = Exe2,
                    Exe3 = Exe3,
                    Exe4 = Exe4,
                    Exe5 = Exe5,
                    Exe6 = Exe6

                });
            }
        }
        public void addBattleNetGames()
        {
            listOfBattleNetGames.Clear();
            listOfBattleNetGames.Add("Hearthstone");
            listOfBattleNetGames.Add("World of Warcraft");
            listOfBattleNetGames.Add("Diablo III");
            listOfBattleNetGames.Add("Starcraft II");
            listOfBattleNetGames.Add("Heroes of the Storm");
            listOfBattleNetGames.Add("Overwatch");
            listOfBattleNetGames.Add("StarCraft");
            listOfBattleNetGames.Add("Warcraft III");
            listOfBattleNetGames.Add("Destiny 2");
            listOfBattleNetGames.Add("Call of Duty: Black Ops 4");
        }
    }
}