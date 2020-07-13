using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using HtmlAgilityPack;
using KSPUpdater.DownloadLink;
using KSPUpdater.Extensions;

namespace KSPUpdater.DownloadLink
{
    static class Utils
    {
        public static HtmlDocument DownloadHtmlDocument(string url)
        {
            var toRet = new HtmlWeb()
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:71.0) Gecko/20100101 Firefox/71.0",
                UseCookies = true,
                
            };

            return toRet.Load(url);
        }


        public static IDownloadLink GetHostType(string urlBase, MyWebView wb)
        {
            if (urlBase.Contains("github.com"))
                return new DownloadGithubLink(urlBase);
            else if (urlBase.Contains("curseforge.com"))
                return new DownloadCurseforgeLink(urlBase, wb);
            else if (urlBase.Contains("spacedock.info"))
                return new DownloadSpaceDock(urlBase);
            else
                throw new NotImplementedException("The link " + urlBase + "comes from an unknown host");
        }
    }
}
