using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLauncher.ViewModels
{
    public class RemoveGame
    {
        private MainWindow mw = new MainWindow();

        public void RemoveGameFromFile(object gameName)
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
            mw.RefreshGames();//This line in the code needs to be the way to refresh the UI
        }
    }
}