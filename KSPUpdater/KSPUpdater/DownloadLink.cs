using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace KSPUpdater
{
    static class DownloadLink
    {
        private static string download(string url)
        {
            WebClient client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:70.0) Gecko/20100101 Firefox/70.0");

            Stream data = client.OpenRead(url);
            StreamReader reader = new StreamReader(data);
            string readed = reader.ReadToEnd();
            data.Close();
            reader.Close();

            return readed;
        }

        private static string FromGitHubUrl(string urlBase)
        {

            var urlContent = download("https://github.com/linuxgurugamer/ClickThroughBlocker/releases");

            return "";
        }

        public static string FromUrl(string urlBase)
        {
            if (urlBase.Contains("github.com"))
                return FromGitHubUrl(urlBase);
            return "";
        }
    }
}
