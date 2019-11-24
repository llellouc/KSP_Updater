using System;
using System.Net;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using KSPUpdater.DownloadLink;

namespace KSPUpdater
{
    class AccueilViewModel
    {
        public AccueilViewModel()
        {
        }

        public void Update()
        {
            var link = "https://github.com/linuxgurugamer/ClickThroughBlocker/releases";
            IDownloadLink hostLink = DownloadLink.Utils.GetHostType(link);

            hostLink.AutomaticDownloadZip();
        }
    }
}
