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

        public static async void LaunchUpdate(object paramObj)
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
                    DotVersion dotVersionFile = null;
                    try
                    {
                        dotVersionFile = new DotVersion(modpath);
                    }
                    catch (ArgumentException e)
                    {
                        Trace.WriteLine(e);
                    }

                    if (dotVersionFile == null || string.IsNullOrEmpty(dotVersionFile.DownloadLink))
                    {
                        Trace.WriteLine("Impossible to read .version of " + modpath);
                        continue;
                    }

                    IDownloadLink hostLink = DownloadLinkHelper.GetHostType(dotVersionFile.DownloadLink, param.Webview);
                    var modName = new DirectoryInfo(modpath).Name;
                    if (hostLink.ZipLink != null)
                    {
                        var zipExtractor = new ZipExtractor(hostLink.ZipLink);
                        zipExtractor.DownloadAndExtract();

                        var updateMod = new PushUpdatedMod(zipExtractor.UnzippedDirectory, param.GameDataPath, modName);

                        await updateMod.AutomaticPush();
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
