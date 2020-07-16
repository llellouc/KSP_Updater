using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using KSPUpdater.Common;
using KSPUpdater.Drivers.Common;
using KSPUpdater.Drivers.Common.Interfaces;

namespace KSPUpdater.Client
{
    public static class UpdaterOrchestraMaster
    {
        public static List<string> GetModPathList(string path)
        {
            return Directory.GetDirectories(path).ToList();
        }

        public static void LaunchUpdate(object wbObject)
        {
            var wb = (MyWebView) wbObject;

            var GameDataPath = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Kerbal Space Program\\GameData\\";

            //var modPathList = GetModPathList(GameDataPath);
            var modPathList = new List<string>();
            modPathList.Add("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Kerbal Space Program\\GameData\\AirplanePlus");

            //Todo : Parallel.ForEach()
            foreach (var modpath in modPathList)
            {
                DotVersion dotVersionFile = null;
                try
                {
                    dotVersionFile = new DotVersion(modpath);
                }
                catch (ArgumentException e)
                {
                    Trace.WriteLine(e);
                }

                if (dotVersionFile == null)
                    continue;

                IDownloadLink hostLink = DownloadLinkHelper.GetHostType(dotVersionFile.DownloadLink, wb);
                if (hostLink.ZipLink != null)
                {
                    var zipExtractor = new ZipExtractor(hostLink.ZipLink);
                    zipExtractor.DownloadAndExtract();

                    var updateMod = new PushUpdatedMod(zipExtractor.UnzippedDirectory, GameDataPath);

                    updateMod.AutomaticPush();
                }
            }

        }
    }
}
