using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLauncher.Models
{
    class ExeSearch
    {
        public void SearchForShortcuts()
        {
            bool test = false;
            SearchSteam();
            if (test == true)
            {
                string programdata = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData).ToString();
                string startMenu = programdata + "\\Microsoft\\Windows\\Start Menu\\";
                startMenu = "C:\\Program Files";
                string[] ext = { ".exe", ".lnk", ".url" };
                foreach (string file in Directory.EnumerateFiles(startMenu, "*.*", SearchOption.AllDirectories).Where(s => ext.Any(ex => ex == Path.GetExtension(s))))
                {
                    Console.WriteLine(file);
                    //somehow need to work out which are games? If possible
                    //Otherwise, swap this method with something that can find purely games
                    //Once we've only got games printing out, we then need to open AddGame 
                    //Autofill in the path with whatever we've got, try and fill in title etc.

                    //RE Steam - need to find steam directory, then read "Steam\Config\config.vdf" to find "BaseInstallFolder" for where user stores library
                    //Then search the baseinstallfolders for the executables
                }
            }
        }

        public void SearchSteam()
        {
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
                    if (File.Exists(config64path))
                    {
                        string[] configLines = File.ReadAllLines(config64path);
                        foreach (var item in configLines)
                        {
                            Console.WriteLine("64:  " + item);
                        }
                    }
                }
            }
        }
    }
}