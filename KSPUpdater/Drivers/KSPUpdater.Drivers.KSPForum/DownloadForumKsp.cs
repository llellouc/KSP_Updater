using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using KSPUpdater.Common;
using KSPUpdater.Drivers.Common;
using KSPUpdater.Drivers.Common.Interfaces;

namespace KSPUpdater.Drivers.KSPForum
{
    [DriverDetails("forum.kerbalspaceprogram.com", typeof(RemoveIrrelevantKSPForumLink))]
    public class DownloadForumKsp : IDownloadLink
    {
        #region Properties

        public override string UrlPattern => "forum.kerbalspaceprogram.com";
        public IDownloadLink SubDownloadLink { get; set; }

        #endregion

        #region Constructor

        public DownloadForumKsp()
        {

        }

        #endregion

        /// <exception cref="NotImplementedException">To document</exception>
        protected override void GetZipURL()
        {
            var homePage = Utils.DownloadHtmlDocument(UrlBase);

            var posts = homePage.DocumentNode.Descendants("div").SingleOrDefault(x => x.Id == "elPostFeed");
            var firstPost = posts?.SelectSingleNode(posts.XPath + "/form[1]/article[1]");

            var aTags = firstPost?.Descendants("a").ToList();

            foreach (var driver in DownloadLinkHelper.Drivers.Value)
            {
                var driverAttributes = driver.Value.GetCustomAttribute<DriverDetailsAttribute>();

                // Prevent from infinite recursivity
                if (driverAttributes.UrlPattern == UrlPattern)
                    continue;

                var links = aTags?.Where(x => x.GetAttributeValue("href", "").Contains(driverAttributes.UrlPattern)).Select(x => x.GetAttributeValue("href", "")).ToList();

                links = driverAttributes.RemoveDuplicateEntries(links);
                links = driverAttributes.RemoveNotLinkedEntries(links, this.ModName);

                if (links.Count == 1)
                {
                    SubDownloadLink = DownloadLinkHelper.GetHostType(links[0], this.ModName,_wb);
                    this.ZipLink = SubDownloadLink.ZipLink;
                    if (!string.IsNullOrEmpty(ZipLink))
                        return;
                }
            }
            throw new NotImplementedException("Unable to get the ZIP link of this Ksp forum Mod");
        }
    }
}
