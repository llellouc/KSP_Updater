using System;
using System.Collections.Generic;
using System.Text;
using KSPUpdater.DownloadLink;

namespace KSPUpdater
{
    public static class UpdaterOrchestraMaster
    {
        public static void LaunchUpdate()
        {
            var link = "https://github.com/linuxgurugamer/ClickThroughBlocker/releases";
            var GameDataPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Kerbal Space Program\\GameData\\";

            IDownloadLink hostLink = DownloadLink.Utils.GetHostType(link);

            var zipExtractor = new ZipExtractor(hostLink.GetZipLink());
            zipExtractor.DownloadAndExtract();

            var updateMod = new PushUpdatedMod(zipExtractor.UnzippedDirectory, GameDataPath);

            updateMod.AutomaticPush();

        }
    }
}
