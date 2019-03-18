using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using GameLauncher.Views;
using System.Windows;
using System.Diagnostics;

namespace GameLauncher.Models
{
    public class AutoImage
    {
        public List<string> iconList = new List<string>();
        public List<string> posterList = new List<string>();
        public List<string> bannerList = new List<string>();
        public string installPath = AppDomain.CurrentDomain.BaseDirectory;
        public string fileName;
        public ExeSelection exs;

        public string AutoDownloadImages(string title, string type)
        {
            //Somehow, a check in here to see if the anti-robot comes up would be useful
            //Maybe a popup with iFrame of the website so user can pass the bot check in-app
            //then continue processing?
            string url = "https://www.qwant.com/?q=" + title.Replace(" ", "%20") + "%20" + type.Replace(" ", "%20") + "&t=images";
            HtmlWeb hw = new HtmlWeb();
            try
            {
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
                
                fileName = installPath + fileName;
                return fileName;
            } catch (Exception) { return null; }
        }
        public string DownloadImage(string title, string type)
        {
            Trace.WriteLine(DateTime.Now + ": Downloading " + title + " " + type);
            App.Current.Dispatcher.Invoke(new Action(() =>
               ((MainWindow)Application.Current.MainWindow).IncreaseExeSearch()));
            using (WebClient client = new WebClient())
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                client.UseDefaultCredentials = true;
                client.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                bool downloadSuccess = false;
                if (type == "icon")
                {
                    while (downloadSuccess == false)
                    {
                        for (int i = 0; i < iconList.Count; i++)
                        {
                            string iconURL = iconList[i];
                            fileName = @"Resources\img\" + title + "-" + type + ".png";
                            try
                            {
                                client.DownloadFile(new Uri(iconURL), fileName);
                            }
                            catch (WebException) { }
                            if (File.Exists(installPath + fileName)) { downloadSuccess = true; return fileName; }
                        }
                    }
                }
                if (type == "poster")
                {
                    while (downloadSuccess == false)
                    {
                        for (int i = 0; i < posterList.Count; i++)
                        {
                            string posterURL = posterList[i];
                            fileName = @"Resources\img\" + title + "-" + type + ".png";
                            try
                            {
                                client.DownloadFile(new Uri(posterURL), fileName);
                            }
                            catch (WebException) { }
                            if (File.Exists(installPath + fileName)) { downloadSuccess = true; return fileName; }
                        }
                    }
                }
                if (type == "banner")
                {
                    while (downloadSuccess == false)
                    {
                        for (int i = 0; i < bannerList.Count; i++)
                        {
                            string bannerURL = bannerList[i];
                            fileName = @"Resources\img\" + title + "-" + type + ".png";
                            try
                            {
                                client.DownloadFile(new Uri(bannerURL), fileName);
                            }
                            catch (WebException) { }
                            if (File.Exists(installPath + fileName)) { downloadSuccess = true; return fileName; }
                        }
                    }
                }
                else { return null; }
            }
            if (fileName != null || fileName != "") { return fileName; }
            else { return null; }
        }
    }
}
