using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public static void LaunchUpdate(object paramObj)
        {
            Task.Run(async () =>
            {
                var param = (UpdateOrchestraMasterParams)paramObj;

                var modPathList = GetModPathList(param.GameDataPath);
                //var modPathList = new List<string>();
                //modPathList.Add("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Kerbal Space Program\\GameData\\AirplanePlus");     // Forum KSP URL
                //modPathList.Add("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Kerbal Space Program\\GameData\\B9PartSwitch");     // Github URL
                //modPathList.Add("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Kerbal Space Program\\GameData\\EasyVesselSwitch"); // Curseforge URL
                //modPathList.Add("C:\\Program Files (x86)\\Steam\\steamapps\\common\\Kerbal Space Program\\GameData\\FuelTanksPlus");    // Spacedock URL

                //Todo : Parallel.ForEach()
                foreach (var modpath in modPathList)
                {
                    try
                    {
                        var modName = new DirectoryInfo(modpath).Name;
                        var dotVersionFile = new DotVersion(modpath);

                        if (string.IsNullOrEmpty(dotVersionFile.DownloadLink))
                        {
                            Trace.WriteLine("No Download link inside " + modName + " mod");
                            continue;
                        }

                        IDownloadLink hostLink = DownloadLinkHelper.GetHostType(dotVersionFile.DownloadLink, dotVersionFile.ModName,param.Webview);
                        var zipExtractor = new ZipExtractor(hostLink.ZipLink);
                        zipExtractor.DownloadAndExtract();

                        var updateMod = new PushUpdatedMod(zipExtractor.UnzippedDirectory, param.GameDataPath, modName);

                        await updateMod.AutomaticPush();
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine(e.Message);
                    }
                }
            }).Wait();
        }
    }

    public class UpdateOrchestraMasterParams
    {
        public MyWebView Webview { get; set; }
        public string GameDataPath { get; set; }
    }
}
