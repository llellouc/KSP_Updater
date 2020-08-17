using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using KSPUpdater.Common;
using KSPUpdater.Drivers.Common.Interfaces;

namespace KSPUpdater.Drivers.Github
{
    public class DownloadGithubLink : IDownloadLink
    {
        public override string UrlPattern => "github.com";

        #region Protected Methods

        protected override void GetZipURL()
        {
            var lastRealeaseURL = GetLastRealeaseURL();
            var lastReleaseHTMLDocument = Utils.DownloadHtmlDocument(lastRealeaseURL);
            this.ZipLink = GetZipLink(lastReleaseHTMLDocument);
        }

        #endregion Protected Methods

        #region Private Methods
        private string GetLastRealeaseURL()
        {
            if (UrlBase == null)
                throw new ArgumentNullException(nameof(UrlBase), "URLBase not set in DownloadGithubLink class");

            if (Regex.IsMatch(UrlBase, "^http(s?)://github.com/[^/]+/[^/]+/releases/latest/?$"))
                return UrlBase;
            else if (Regex.IsMatch(UrlBase, "^http(s?)://github.com/[^/]+/[^/]+/releases/tag/[^/]+$"))
                return UrlBase.Split("tag/").First() + "latest";
            else if (Regex.IsMatch(UrlBase, "^http(s?)://github.com/[^/]+/[^/]+/releases/$"))
                return UrlBase + "latest";
            else if (Regex.IsMatch(UrlBase, "^http(s?)://github.com/[^/]+/[^/]+/releases$"))
                return UrlBase + "/latest";
            else if (Regex.IsMatch(UrlBase, "^http(s?)://github.com/[^/]+/[^/]+/$"))
                return UrlBase + "releases/latest";
            else if (Regex.IsMatch(UrlBase, "^http(s?)://github.com/[^/]+/[^/]+$"))
                return UrlBase + "/releases/latest";
            else
                throw new ArgumentException("Impossible to parse Github URL", nameof(UrlBase));
        }

        /// <exception cref="ArgumentException">To document</exception>
        /// <exception cref="InvalidDataException">To document</exception>
        private string GetZipLink(HtmlDocument doc)
        {
            var href = doc.DocumentNode.DescendantsAndSelf("a").SingleOrDefault(x =>
                Regex.IsMatch(x.GetAttributeValue("href", ""), "^/[\\w]+/[\\w]+/releases/download/[\\S]+\\.zip$"))?.GetAttributeValue("href", "");

            if(!string.IsNullOrEmpty(href))
                return "https://github.com" + href;
           
            else if (doc.DocumentNode.InnerText.Contains("There aren’t any releases here"))
                throw new InvalidDataException("No release on this Github : " + this.GetLastRealeaseURL()); //Can be improved
            //else
            throw new ArgumentException("Unable to Get ZipLink from Github : " + this.GetLastRealeaseURL());

        }
        #endregion

        #region Constructor
        public DownloadGithubLink()
        {
        }
        #endregion Constructor

    }
}
