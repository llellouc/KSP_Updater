using System;
using System.Linq;
using KSPUpdater.Common;
using KSPUpdater.Drivers.Common;
using KSPUpdater.Drivers.Common.Interfaces;

namespace KSPUpdater.Drivers.KSPForum
{
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

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException">To document</exception>
        protected override void GetZipURL()
        {
            var homePage = Utils.DownloadHtmlDocument(UrlBase);

            var posts = homePage.DocumentNode.Descendants("div").SingleOrDefault(x => x.Id == "elPostFeed");
            var firstPost = posts?.SelectSingleNode(posts.XPath + "/form[1]/article[1]");

            var aTags = firstPost?.Descendants("a").ToList();

            var githubLink = aTags?.Where(x => x.GetAttributeValue("href", "").Contains("github.com")).ToList();
            if (githubLink?.Count == 1)
            {
                SubDownloadLink = DownloadLinkHelper.GetHostType(githubLink[0].GetAttributeValue("href", ""), _wb);
                this.ZipLink = SubDownloadLink.ZipLink;
                if(string.IsNullOrEmpty(ZipLink) == false)
                    return;
            }

            var spaceDockLink = aTags?.Where(x => x.GetAttributeValue("href", "").Contains("spacedock.info")).ToList();
            if (spaceDockLink?.Count == 1)
            {
                SubDownloadLink = DownloadLinkHelper.GetHostType(spaceDockLink[0].GetAttributeValue("href", ""), _wb);
                this.ZipLink = SubDownloadLink.ZipLink;
                if (string.IsNullOrEmpty(ZipLink) == false)
                    return;
            }

            var curseforgeLink = aTags?.Where(x => x.GetAttributeValue("href", "").Contains("curseforge.com")).ToList();
            if (curseforgeLink?.Count == 1)
            {
                SubDownloadLink = DownloadLinkHelper.GetHostType(curseforgeLink[0].GetAttributeValue("href", ""), _wb);
                this.ZipLink = SubDownloadLink.ZipLink;
                if (string.IsNullOrEmpty(ZipLink) == false)
                    return;
            }
            throw new NotImplementedException("Unable to get the ZIP link of this Ksp forum Mod");
        }
    }
}
