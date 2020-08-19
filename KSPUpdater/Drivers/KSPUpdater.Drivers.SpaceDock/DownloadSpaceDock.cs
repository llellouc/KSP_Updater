using System.IO;
using System.Linq;
using KSPUpdater.Common;
using KSPUpdater.Drivers.Common;
using KSPUpdater.Drivers.Common.Interfaces;

namespace KSPUpdater.Drivers.SpaceDock
{
    [DriverDetails("spacedock.info", typeof(RemoveIrrelevantSpaceDockLinks))]
    public class DownloadSpaceDock : IDownloadLink
    {
        public override string UrlPattern => "spacedock.info";

        public DownloadSpaceDock()
        {
        }

        /// <exception cref="InvalidDataException">To document</exception>
        protected override void GetZipURL()
        {
            var homePage = Utils.DownloadHtmlDocument(UrlBase);
            var preDownloadPageLink = homePage.DocumentNode.Descendants("a").SingleOrDefault(x => x.Id == "download-link-primary")
                ?.GetAttributeValue("href", "");

            if (preDownloadPageLink != null)
            {
                var preDownloadPage = Utils.DownloadHtmlDocument("http://spacedock.info" + preDownloadPageLink);

                ZipLink = preDownloadPage.DocumentNode.Descendants("a").SingleOrDefault()?.GetAttributeValue("href", "");
            }
            else if (homePage.DocumentNode.InnerText.Contains("Looks like this was deleted, or maybe was never here. Who knows.")) //TODO : improve verify better
            {
                throw new InvalidDataException("Error 404 when trying to access to the following link " + this.UrlBase);
            }
        }
    }
}
