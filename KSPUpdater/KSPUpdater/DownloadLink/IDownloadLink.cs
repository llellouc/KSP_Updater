using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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



        public string GetZipLink()
        {
            this.ParseUrl();
            this.DownloadHtml();
            this.ParseHtml();

            return ZipLink;

            
        }
    }
}
