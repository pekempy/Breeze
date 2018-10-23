using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GameLauncher.ViewModels
{
    public class ModifyFile
    {
        private static readonly MainWindow MainWindow = (MainWindow)Application.Current.MainWindow;

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
            gameName = gameName.ToString();
            var text = File.ReadAllLines("./Resources/GamesList.txt", Encoding.UTF8);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Contains($"{gameName}|"))
                {
                    try
                    {
                        Console.WriteLine(text[i]); //Write entire game line to output window
                        string[] column = text[i].Split('|');
                        MainWindow.DialogAddGames.NewGameTitle.Text = column[0];
                        MainWindow.DialogAddGames.NewGameGenre.Text = column[1];
                        MainWindow.DialogAddGames.NewGamePath.Text = column[2];
                        MainWindow.DialogAddGames.NewGameLink.Text = column[3];
                        MainWindow.DialogAddGames.NewGameIcon.Text = column[4];
                        MainWindow.DialogAddGames.NewGamePoster.Text = column[5];
                        MainWindow.DialogAddGames.NewGameBanner.Text = column[6];
                        MainWindow.OpenAddGameDialog();
                        RemoveGameFromFile(gameName); //Prevent duplicates
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