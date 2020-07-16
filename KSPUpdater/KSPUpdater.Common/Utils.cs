using HtmlAgilityPack;

namespace KSPUpdater.Common
{
    public static class Utils
    {
        public static HtmlDocument DownloadHtmlDocument(string url)
        {
            var toRet = new HtmlWeb()
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:71.0) Gecko/20100101 Firefox/71.0",
                UseCookies = true,
                
            };

            return toRet.Load(url);
        }
    }
}
