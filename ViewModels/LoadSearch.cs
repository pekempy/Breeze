using GameLauncher.Models;
using HtmlAgilityPack;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace GameLauncher.ViewModels
{
    public class LoadSearch
    {
        public ObservableCollection<SearchResults> SearchList { get; set; }
        private ObservableCollection<SearchResults> searchlist = new ObservableCollection<SearchResults>();
        public int numofitems = 0;
        public void Search(string gametitle, string imagetype, string searchstring, int offset)
        {
            SearchLinks(gametitle, imagetype, searchstring, 0);
            searchstring = gametitle + imagetype;
            SearchLinks(gametitle, imagetype, searchstring, 0);
            searchstring = imagetype + " for " + gametitle;
            SearchLinks(gametitle, imagetype, searchstring, 0);
            searchstring = gametitle + imagetype + " png";
            SearchLinks(gametitle, imagetype, searchstring, 0);
            SearchList = searchlist;
        }

        public void SearchLinks(string gametitle, string imagetype, string searchstring, int offset)
        {
            searchstring = searchstring.Replace(" ", "%20");
            var url = "https://www.qwant.com/?q=" + searchstring +"&t=images&count=" + offset;
            try
            {
                HtmlWeb hw = new HtmlWeb();
                HtmlDocument doc = hw.Load(url);
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
                    numofitems++;
                }
                SearchList = searchlist;
            }
            catch (Exception e) { Trace.WriteLine(DateTime.Now + ": SearchLinks: " + e); }
        }
    }
}
