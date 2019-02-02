using GameLauncher.Models;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Data;

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
                HtmlAgilityPack.HtmlWeb hw = new HtmlAgilityPack.HtmlWeb();
                HtmlAgilityPack.HtmlDocument doc = hw.Load(url);
                foreach (HtmlNode link in doc.DocumentNode.SelectNodes("//img"))
                {
                    string imgValue = link.GetAttributeValue("src", string.Empty);
                    string[] imgLink = imgValue.Split('=');
                    string imglink = imgLink[1].Replace("%3A", ":");
                    imglink = imglink.Replace("%2F", "/");
                    imglink = imglink.Remove(imglink.Length - 2);
                    imgValue = "http:" + imgValue;
                    searchlist.Add(new SearchResults
                    {
                        Thumbnail = imgValue,
                        Image = imglink
                    });
                }
                SearchList = searchlist;
                ObservableList();
            }
            catch (Exception e) { Console.WriteLine("Error: " + e); }
        }
        public void ObservableList() //Prints out contents of ObsCol
        {
            for (int i = 0; i < SearchList.Count; i++)
            {
                Console.WriteLine(string.Concat(SearchList[i].Thumbnail, " | ", SearchList[i].Image));
            }
        }
    }
}
