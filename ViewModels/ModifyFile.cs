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

        public static void RemoveGameFromFile(object gameGuid)
        {
            gameGuid = gameGuid.ToString();
            var text = File.ReadAllLines("./Resources/GamesList.txt", Encoding.UTF8);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Contains($"{gameGuid}"))
                {
                    try
                    {
                        text[i] = "";

                        text = text.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        File.WriteAllLines("./Resources/GamesList.txt", text);
                        if(new FileInfo("./Resources/GamesList.txt").Length == 0)
                        {
                            File.Delete("./Resources/GamesList.txt");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
        }

        public static void EditGameInfile(object gameGuid)
        {
            gameGuid = gameGuid.ToString();
            var text = File.ReadAllLines("./Resources/GamesList.txt", Encoding.UTF8);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Contains($"{gameGuid}"))
                {
                    try
                    {
                        Console.WriteLine(text[i]); //Write entire game line to output window
                        string[] column = text[i].Split('|');
                        MainWindow.DialogEditGames.EditTitle.Text = column[0];
                        MainWindow.DialogEditGames.EditGenre.Text = column[1];
                        MainWindow.DialogEditGames.EditPath.Text = column[2];
                        MainWindow.DialogEditGames.EditLink.Text = column[3];
                        MainWindow.DialogEditGames.EditIcon.Text = column[4];
                        MainWindow.DialogEditGames.EditPoster.Text = column[5];
                        MainWindow.DialogEditGames.EditBanner.Text = column[6];
                        MainWindow.OpenEditGameDialog(gameGuid.ToString());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                }
            }
        }

        public static void DeleteGameImages(string title)
        {
            string installPath = AppDomain.CurrentDomain.BaseDirectory;
            installPath = installPath.Replace("\\", "/");
            //need to bypass the files being in use problem :|
            File.Delete(installPath + "Resources/img/" + title + "-icon.png");
            File.Delete(installPath + "Resources/img/" + title + "-poster.png");
            File.Delete(installPath + "Resources/img/" + title + "-banner.png");
            File.Delete(installPath + "Resources/shortcuts/" + title + ".lnk");
        }

        public static void RemoveGenreFromFile(object genreGuid)
        {
            genreGuid = genreGuid.ToString();
            var text = File.ReadAllLines("./Resources/GenreList.txt", Encoding.UTF8);
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i].Contains($"{genreGuid}"))
                {
                    try
                    {
                        text[i] = "";

                        text = text.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                        File.WriteAllLines("./Resources/GenreList.txt", text);
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