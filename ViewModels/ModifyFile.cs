using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLauncher.ViewModels
{
    public class ModifyFile
    {
        public static void RemoveGameFromFile(object gameName)
        {
            gameName = gameName.ToString();
            var text = File.ReadAllLines("./Resources/GamesList.txt", Encoding.UTF8);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Contains($"{gameName}|"))
                {
                    try
                    {
                        text[i] = "";

                        text = text.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        File.WriteAllLines("./Resources/GamesList.txt", text);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
        }

        public static void EditGameInfile(object gameName)
        {
            AddGames eg = new AddGames();
            gameName = gameName.ToString();
            var text = File.ReadAllLines("./Resources/GamesList.txt", Encoding.UTF8);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Contains($"{gameName}|"))
                {
                    try
                    {
                        string[] column = new string[0];
                        Console.WriteLine(text[i]); //Write entire game line to output window
                        column = text[i].Split('|');
                        eg.NewGameTitle.Text = column[0];
                        eg.NewGameGenre.Text = column[1];
                        eg.NewGamePath.Text = column[2];
                        eg.NewGameLink.Text = column[3];
                        eg.NewGameIcon.Text = column[4];
                        eg.NewGamePoster.Text = column[5];
                        eg.NewGameBanner.Text = column[6];
                        RemoveGameFromFile(gameName); //Prevent duplicates
                        eg.AddGameDialog.IsOpen = true;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
        }
    }
}