using System;
using System.Linq;
using System.Text.RegularExpressions;
using KSPUpdater.Common;
using KSPUpdater.Drivers.Common;
using KSPUpdater.Drivers.Common.Interfaces;

namespace KSPUpdater.Drivers.Curseforge
{
    [DriverDetails("curseforge.com", typeof(RemoveIrrelevantCurseforgeLink))]
    public class DownloadCurseforgeLink : IDownloadLink
    {
        public override string UrlPattern => "curseforge.com";

        protected override void GetZipURL()
        {
            var downloadURL = GetDownloadPageURL();
            this.ZipLink = _wb.LoadAndGetRedirectionURLOf(downloadURL);
        }

        /// <summary>
        /// Convert the URL from the .version file to the URL of the ZIP file
        /// </summary>
        /// <exception cref="ArgumentException">To document</exception>
        private string GetDownloadPageURL()
        {
            if (UrlBase == null)
                throw new ArgumentNullException(nameof(UrlBase), "URLBase not set in DownloadCurseforgeLink class");

            //TODO : Add an if when there is a download inside the link

            // Like : http:// www.curseforge.com/kerbal/ksp-mods/easy-vessel-switch-evs/
            if (Regex.IsMatch(UrlBase, "^http(s?)://(www\\.)?curseforge.com/[^/]+/[^/]+/[^/]+/$"))
                return UrlBase + "download";
            // Like : http:// www.curseforge.com/kerbal/ksp-mods/easy-vessel-switch-evs
            else if (Regex.IsMatch(UrlBase, "^http(s?)://(www\\.)?curseforge.com/[^/]+/[^/]+/[^/]+$"))
                return UrlBase + "/download";
            // Like : http:// kerbal.curseforge.com/projects/easy-vessel-switch-evs
            else if (Regex.IsMatch(UrlBase, "^http(s?)://kerbal.curseforge.com/projects/[^/]+(/files)?(/)?$"))
            {
                var doc = _wb.LoadAndGetContentOf(UrlBase);

                var href = doc.DocumentNode.DescendantsAndSelf("a").FirstOrDefault(x =>
                    Regex.IsMatch(x.GetAttributeValue("href", ""), "^/[^/]+/[^/]+/[^/]+/download$"))?.GetAttributeValue("href", "");
                return "https://www.curseforge.com" + href;
            }
            else
                throw new ArgumentException("Impossible to parse Curseforge URL", nameof(UrlBase));
        }

        public DownloadCurseforgeLink()
        {
        }
    }
}
