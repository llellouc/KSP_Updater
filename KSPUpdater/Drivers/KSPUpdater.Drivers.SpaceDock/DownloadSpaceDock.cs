using System.Linq;
using KSPUpdater.Common;
using KSPUpdater.Drivers.Common.Interfaces;

namespace KSPUpdater.Drivers.SpaceDock
{
    public class DownloadSpaceDock : IDownloadLink
    {
        public override string UrlPattern => "spacedock.info";

        public DownloadSpaceDock()
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
