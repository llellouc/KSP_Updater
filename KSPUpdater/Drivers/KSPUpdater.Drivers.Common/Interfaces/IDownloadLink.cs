using KSPUpdater.Common;

namespace KSPUpdater.Drivers.Common.Interfaces
{
    public abstract class IDownloadLink
    {
        protected MyWebView _wb;

        /// <summary>
        /// Mod Name in the .version file
        /// </summary>
        public string ModName { get; protected set; }

        /// <summary>
        /// URL in the .version file
        /// </summary>
        public string UrlBase { get; protected set; }
        /// <summary>
        /// URL of the zip. Just need to use WebClient.DownloadFile() to have the Zip
        /// </summary>
        public string ZipLink { get; protected set; }

        public abstract string UrlPattern { get; }

        protected abstract void GetZipURL();

        public void Initialize(string urlBase, string modName, MyWebView wb = null)
        {
            UrlBase = urlBase;
            ModName = modName;
            _wb = wb;
            ZipLink = null;

            GetZipURL();
        }
    }
}
