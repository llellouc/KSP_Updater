using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using KSPUpdater.Extensions;

namespace KSPUpdater.DownloadLink
{
    public class DownloadSpaceDock : IDownloadLink
    {
        public DownloadSpaceDock(string urlBase, MyWebView wb = null) : base(urlBase, wb)
        {
        }

        protected override void GetZipURL()
        {
            var homePage = Utils.DownloadHtmlDocument(UrlBase);
            var preDownloadPageLink = homePage.DocumentNode.Descendants("a").SingleOrDefault(x => x.Id == "download-link-primary")
                ?.GetAttributeValue("href", "");

            var preDownloadPage = Utils.DownloadHtmlDocument("http://spacedock.info" + preDownloadPageLink);

            ZipLink = preDownloadPage.DocumentNode.Descendants("a").SingleOrDefault()
                ?.GetAttributeValue("href", "");
        }
    }
}
