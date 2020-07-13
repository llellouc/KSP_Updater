using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPUpdater.Extensions;

namespace KSPUpdater.DownloadLink
{
    public class DownloadForumKsp : IDownloadLink
    {
        public IDownloadLink SubDownloadLink { get; set; }
        public DownloadForumKsp(string urlBase, MyWebView wb = null) : base(urlBase, wb)
        {
        }

        protected override void GetZipURL()
        {
            var homePage = Utils.DownloadHtmlDocument(UrlBase);

            var posts = homePage.DocumentNode.Descendants("div").SingleOrDefault(x => x.Id == "elPostFeed");
            var firstPost = posts?.SelectSingleNode(posts.XPath + "/form[1]/article[1]");

            var aTags = firstPost?.Descendants("a").ToList();

            var githubLink = aTags?.Where(x => x.GetAttributeValue("href", "").Contains("github.com")).ToList();
            if (githubLink?.Count == 1)
            {
                SubDownloadLink = new DownloadGithubLink(githubLink[0].GetAttributeValue("href", ""));
                this.ZipLink = SubDownloadLink.ZipLink;
                if(string.IsNullOrEmpty(ZipLink) == false)
                    return;
            }

            var spaceDockLink = aTags?.Where(x => x.GetAttributeValue("href", "").Contains("spacedock.info")).ToList();
            if (spaceDockLink?.Count == 1)
            {
                SubDownloadLink = new DownloadSpaceDock(spaceDockLink[0].GetAttributeValue("href", ""));
                this.ZipLink = SubDownloadLink.ZipLink;
                if (string.IsNullOrEmpty(ZipLink) == false)
                    return;
            }

            var curseforgeLink = aTags?.Where(x => x.GetAttributeValue("href", "").Contains("curseforge.com")).ToList();
            if (curseforgeLink?.Count == 1)
            {
                SubDownloadLink = new DownloadCurseforgeLink(curseforgeLink[0].GetAttributeValue("href", ""), _wb);
                this.ZipLink = SubDownloadLink.ZipLink;
                if (string.IsNullOrEmpty(ZipLink) == false)
                    return;
            }
            throw new NotImplementedException("Unable to get the ZIP link of this Ksp forum Mod");
        }
    }
}
