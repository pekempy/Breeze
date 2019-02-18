using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameLauncher.Models
{
    class ExeSearch
    {
        //steamGameDirs will contain all directories which are set as steam libraries! :D
        public List<string> steamGameDirs = new List<string>();

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

            foreach(string k32subKey in key32.GetSubKeyNames())
            {
                using (RegistryKey subKey = key32.OpenSubKey(k32subKey))
                {
                    steam32path = subKey.GetValue("InstallPath").ToString();
                    config32path = steam32path + "/steamapps/libraryfolders.vdf";
                    if (File.Exists(config32path))
                    {
                        string[] configLines = File.ReadAllLines(config32path);
                        foreach(var item in configLines)
                        {
                            Console.WriteLine("32:  " + item);
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
                                steamGameDirs.Add(item2);
                            }
                        }
                        steamGameDirs.Add(steam64path + "\\steamapps\\common\\");
                    }
                }
            }

            foreach(string item in steamGameDirs)
            {
                //Need to store each exe found, and be able to sort them by folder name they were found in
                //Not sure how to do this cos need to consider how it displays on the screen for user to select
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