using GameLauncher.Models;
using HtmlAgilityPack;
using System;
using System.Collections.ObjectModel;

namespace GameLauncher.ViewModels
{
    public class LoadSearch
    {
        public ObservableCollection<SearchResults> SearchList { get; set; }
        private ObservableCollection<SearchResults> searchlist = new ObservableCollection<SearchResults>();

        public void Search(string gametitle, string imagetype, string searchstring)
        {
            SearchLinks(gametitle, imagetype, searchstring);
            SearchList = searchlist;
        }

        public void SearchLinks(string gametitle, string imagetype, string searchstring)
        {
            searchstring = searchstring.Replace(" ", "%20");
            var url = "https://www.qwant.com/?q=" + searchstring +"&t=images";
            try
            {
                HtmlWeb hw = new HtmlWeb();
                HtmlDocument doc = hw.Load(url);
                int numofitems = 0;
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//img"))
                {
                    string imgValue = link.GetAttributeValue("src", string.Empty);
                    string[] imgLink = imgValue.Split('=');
                    string imglink = imgLink[1].Replace("%3A", ":");
                    imglink = imglink.Replace("%2F", "/");
                    imglink = imglink.Remove(imglink.Length - 2);
                    imgValue = "http:" + imgValue;
                    imgValue = imgValue.Substring(0, imgValue.LastIndexOf("&q="));
                    searchlist.Add(new SearchResults
                    {
                        Thumbnail = imgValue,
                        Image = imglink
                    });
                    Console.WriteLine("Game #" + numofitems);
                    Console.WriteLine("Thumbnail:" + imgValue);
                    Console.WriteLine("Image: " + imglink);
                    Console.WriteLine();
                    numofitems++;
                }
                SearchList = searchlist;
            }
            catch (Exception e) { Console.WriteLine("Error: " + e); }
        }
    }
}
