using System;
using System.Collections.Generic;
using System.Text;
using HtmlAgilityPack;

namespace KSPUpdater.DownloadLink
{
    abstract class IDownloadLink
    {
        public string UrlBase { get; protected set; }
        public string UrlLatestVersionPage { get; protected set; }
        public HtmlDocument LatestVersionPageHtmlDocument { get; protected set; }
        public string ZipLink { get; protected set; }

        protected abstract void ParseUrl();

        protected abstract void ParseHtml();

        protected void DownloadHtml()
        {
            LatestVersionPageHtmlDocument = Utils.DownloadHtmlDocument(UrlLatestVersionPage);
        }

        protected abstract void DownloadZip();

        public void AutomaticDownloadZip()
        {
            this.ParseUrl();
            this.DownloadHtml();
            this.ParseHtml();
            this.DownloadZip();
        }
    }
}
