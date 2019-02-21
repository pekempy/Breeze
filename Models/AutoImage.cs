using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace GameLauncher.Models
{
    public class AutoImage
    {
        public List<string> iconList = new List<string>();
        public List<string> posterList = new List<string>();
        public List<string> bannerList = new List<string>();
        public string installPath = AppDomain.CurrentDomain.BaseDirectory;

        public void AutoDownloadImages(string title, string type)
        {
            string url = "https://www.qwant.com/?q=" + title.Replace(" ", "%20") + "%20game%20" + type.Replace(" ", "%20") + "&t=images";
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
                    string directimage = imglink;
                    if (type == "icon")
                    {
                        iconList.Add(directimage);
                    }
                    if (type == "poster")
                    {
                        posterList.Add(directimage);
                    }
                    if (type == "banner")
                    {
                        bannerList.Add(directimage);
                    }
                }
                DownloadImage(title, type);
                iconList.Clear();
                posterList.Clear();
                bannerList.Clear();
            }
            catch (Exception e) { Console.WriteLine("Error autodl: " + e); }
        }

        public void DownloadImage(string title, string type)
        {
            using (WebClient client = new WebClient())
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.UseDefaultCredentials = true;
                client.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                if (type == "icon")
                {
                    string iconURL = iconList[0];
                    string fileName = @"Resources\img\" + title + "-" + type + ".png";
                    client.DownloadFile(new Uri(iconURL), fileName);
                }
                if (type == "poster")
                {
                    string posterURL = posterList[0];
                    string fileName = @"Resources\img\" + title + "-" + type + ".png";
                    client.DownloadFile(new Uri(posterURL), fileName);
                }
                if (type == "banner")
                {
                    string bannerURL = bannerList[0];
                    string fileName = @"Resources\img\" + title + "-" + type + ".png";
                    client.DownloadFile(new Uri(bannerURL), fileName);
                }
            }
        }
    }
}
