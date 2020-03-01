using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using KSPUpdater.Extensions;

namespace KSPUpdater.DownloadLink
{
    class ZipExtractor
    {
        #region Properties

        public string ZipUrl { get; private set; }
        public string TmpDirectory { get; private set; }
        public string ZipFile { get; private set; }
        public string UnzippedDirectory { get; private set; }

        #endregion

        #region Constructor

        public ZipExtractor(string zipUrl)
        {
            this.ZipUrl = zipUrl;
            this.TmpDirectory = Directory.CreateDirectory(Path.GetTempPath() + "KSPUpdater" + Path.DirectorySeparatorChar).FullName;
            this.ZipFile = null;
            this.UnzippedDirectory = null;
        }

        ~ZipExtractor()
        {
            try
            {
                Directory.Delete(this.UnzippedDirectory, true);
            }
            catch (DirectoryNotFoundException e)
            {

            }
        }

        #endregion


        private void Download()
        {
            if (this.ZipUrl == null)
                throw new ArgumentNullException(nameof(this.ZipUrl), "Impossible to download the ZipFile. The URL is null");

            this.ZipFile = this.TmpDirectory + Path.GetRandomFileName();
            this.ZipFile = Regex.Replace(this.ZipFile, ".[\\w]+$", ".zip");

            using (var client = new WebClient())
            {
                client.DownloadFile(ZipUrl, this.ZipFile);
            }
        }

        private void Extract()
        {
            this.UnzippedDirectory = this.TmpDirectory + Path.GetRandomFileName();
            this.UnzippedDirectory = Regex.Replace(this.UnzippedDirectory, ".[\\w]+$", "");

            System.IO.Compression.ZipFile.ExtractToDirectory(this.ZipFile, this.UnzippedDirectory);
        }

        public void DownloadAndExtract()
        {
            Download();
            Extract();
            File.Delete(this.ZipFile);
        }
    }
}
