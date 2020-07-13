using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using KSPUpdater.Extensions;

namespace KSPUpdater.DownloadLink
{
    public abstract class IDownloadLink
    {
        protected MyWebView _wb;

        /// <summary>
        /// URL in the .version file
        /// </summary>
        public string UrlBase { get; protected set; }
        /// <summary>
        /// URL of the zip. Just need to use WebClient.DownloadFile() to have the Zip
        /// </summary>
        public string ZipLink { get; protected set; }

        protected abstract void GetZipURL();

        public IDownloadLink(string urlBase, MyWebView wb = null)
        {
            UrlBase = urlBase;
            _wb = wb;
            ZipLink = null;

            GetZipURL();
        }
    }
}
