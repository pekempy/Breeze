using GameLauncher.Models;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;

namespace GameLauncher.ViewModels
{
    public class LoadSearch
    {
        public ObservableCollection<SearchResults> SearchList { get; set; }
        private ObservableCollection<SearchResults> searchlist = new ObservableCollection<SearchResults>();
        public int numofitems = 0;
        public string url;
        public string urlapi;
        public List<ImageResult> result = new List<ImageResult>();
        public void Search(string gametitle, string imagetype, string searchstring, int offset)
        {
            SearchLinks(gametitle, imagetype, searchstring, 0);
            SearchList = searchlist;
        }

        public void SearchLinks(string gametitle, string imagetype, string searchstring, int offset)
        {
            if (searchstring != "")
            {
                searchstring = searchstring.Replace(" ", "%20");
                url = "https://www.qwant.com/?q=" + searchstring + "&t=images";
                urlapi = "https://api.qwant.com/api/search/images?count=50&offset=0&q=" + searchstring + "&t=images&uiv=1";
                APISearch();
            }
        }
        public void APISearch()
        {
            string jsonSearch;
            var client = new WebClient();
            var headers = new WebHeaderCollection();
            headers.Add(HttpRequestHeader.Accept, "text/html, application/xhtml+xml, */*");
            headers.Add(HttpRequestHeader.AcceptLanguage, "en-GB");
            headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.90 Safari/537.36");
            client.Headers = headers;
            client.Encoding = System.Text.Encoding.UTF8;

            jsonSearch = client.DownloadString(urlapi);
            var obj = JObject.Parse(jsonSearch);
            JToken resultlist = obj["data"]["result"]["items"]; //get all search results
            foreach (var item in resultlist)
            {
                searchlist.Add(new SearchResults
                {
                    Title = item["title"].Value<string>(),
                    DirectLink = item["media"].Value<string>(),
                    DirectLinkQwant = "http:" + item["media_fullsize"].Value<string>(),
                    ThumbnailLink = "http:" + item["thumbnail"].Value<string>(),
                    Height = item["height"].Value<string>(),
                    Width = item["width"].Value<string>()
                });
            }
            SearchList = searchlist;
        }

    }
    public class ImageResult{
        public string Title { get; set; }
        public string DirectLink { get; set; }
        public string DirectLinkQwant { get; set; }
        public string ThumbnailLink { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        }
}
