using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace KSPUpdater.DownloadLink
{
    class DownloadGithubLink : IDownloadLink
    {

        #region Protected Methods

        protected override void DownloadZip()
        {
            throw new NotImplementedException();
        }

        protected override void ParseUrl()
        {
            if(UrlBase == null)
                throw new SystemException("URLBase not set in DownloadGithubLink class");

            if (Regex.IsMatch(UrlBase, "^http(s?)://github.com/[\\w]+/[\\w]+/releases/latest/?$"))
                UrlLatestVersionPage = UrlBase;
            else if (Regex.IsMatch(UrlBase, "^http(s?)://github.com/[\\w]+/[\\w]+/releases/$"))
                UrlLatestVersionPage = UrlBase + "latest";
            else if (Regex.IsMatch(UrlBase, "^http(s?)://github.com/[\\w]+/[\\w]+/releases$"))
                UrlLatestVersionPage = UrlBase + "/latest";
            else
                throw new ArgumentException("Impossible to parse Github URL", nameof(UrlBase));
        }

        protected override void ParseHtml()
        {
            var href = this.LatestVersionPageHtmlDocument.DocumentNode.DescendantsAndSelf("a").SingleOrDefault(x =>
                Regex.IsMatch(x.GetAttributeValue("href", ""), "^/[\\w]+/[\\w]+/releases/download/[0-9\\.]+/[\\S]+\\.zip$"))?.GetAttributeValue("href", "");

            this.ZipLink = "https://github.com" + href;

        }

        #endregion Protected Methods



        #region Constructor
        public DownloadGithubLink(string urlBase)
        {
            UrlBase = urlBase;
            UrlLatestVersionPage = null;
            LatestVersionPageHtmlDocument = null;
            ZipLink = null;
        }
        #endregion Constructor

    }
}
