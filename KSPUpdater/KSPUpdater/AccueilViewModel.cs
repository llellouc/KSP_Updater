using System;
using System.Net;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace KSPUpdater
{
    class AccueilViewModel
    {
        private WebClient Client;

        public AccueilViewModel()
        {
        }

        public void Update()
        {
            var a = DownloadLink.FromUrl("github.com");
        }
    }
}
