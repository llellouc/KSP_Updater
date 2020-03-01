using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KSPUpdater.DownloadLink;
using KSPUpdater.Extensions;

namespace KSPUpdater
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
            modPathList.Add("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Kerbal Space Program\\GameData\\000_ClickThroughBlocker");
            modPathList.Add("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Kerbal Space Program\\GameData\\EasyVesselSwitch");

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

                IDownloadLink hostLink = DownloadLink.Utils.GetHostType(dotVersionFile.DownloadLink, wb);
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
