using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using KSPUpdater.DownloadLink;

namespace KSPUpdater.DownloadLink
{
    static class Utils
    {
        public static HtmlDocument DownloadHtmlDocument(string url)
        {
            //Marchait bien mais il y a plus simple
            /*WebClient client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:70.0) Gecko/20100101 Firefox/70.0");

            Stream data = client.OpenRead(url);
            StreamReader reader = new StreamReader(data);
            string readed = reader.ReadToEnd();
            data.Close();
            reader.Close();

            var toRet = new HtmlDocument();
            toRet.LoadHtml(readed);
            */

            var toRet = new HtmlWeb();
            
            return toRet.Load(url);
        }


        public static IDownloadLink GetHostType(string urlBase)
        {
            if (urlBase.Contains("github.com"))
                return new DownloadGithubLink(urlBase);
            else
                throw new NotImplementedException("The link " + urlBase + "comes from an unknown host");
        }
    }
}
