using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KSPUpdater.Client.UpdateDisplay;

namespace KSPUpdater.Client
{
    class PushUpdatedMod
    {
        public string UnzippedModRootPath { get; private set; }
        public string UnzippedModGameDataPath { get; private set; }
        public string GameDataPath { get; private set; }
        public string ModName { get; private set; }

        public List<string> ModsNames { get; private set; }

        public PushUpdatedMod(string unzippedModRootPath, string gameDataPath, string modName)
        {
            this.UnzippedModRootPath = unzippedModRootPath;
            this.GameDataPath = gameDataPath;
            this.ModName = modName;

            this.UnzippedModGameDataPath = null;
            ModsNames = new List<string>();
        }

        private void ParseUnzippedModPath()
        {
            var subDirectories = Directory.GetDirectories(this.UnzippedModRootPath).ToList();
            if (subDirectories.Any(x => x.EndsWith("GameData")))
                UnzippedModGameDataPath = UnzippedModRootPath + Path.DirectorySeparatorChar +"GameData" + Path.DirectorySeparatorChar;
            else if (subDirectories.Any(x => x.EndsWith(this.ModName)))
            {
                UnzippedModGameDataPath = UnzippedModRootPath + Path.DirectorySeparatorChar;
            }
            else
                throw new ArgumentException("Impossible to parse UnzippedModGameDataPath", nameof(UnzippedModGameDataPath));
        }

        private void GetModsNames()
        {
            var regex = new Regex("[^/\\\\]+$");
            foreach (var dir in Directory.GetDirectories(this.UnzippedModGameDataPath))
            {
                ModsNames.Add( regex.Match(dir).Value);
            }
        }

        private bool IsDowloadedModMoreRecent(string oldModPath, string unzippedModPath)
        {
            var oldDotVersionPath = Directory.GetFiles(oldModPath, "*.version", SearchOption.AllDirectories).FirstOrDefault();
            var newDotVersionPath = Directory.GetFiles(unzippedModPath, "*.version", SearchOption.AllDirectories).FirstOrDefault();
            if (string.IsNullOrEmpty(oldDotVersionPath) || string.IsNullOrEmpty(newDotVersionPath))
                return true;

            //else
            DotVersion oldModVersion = new DotVersion(oldDotVersionPath);
            DotVersion unzippedModVersion = new DotVersion(newDotVersionPath);

            return unzippedModVersion > oldModVersion;
        }


        /// <returns>The list of updated Mods</returns>
        public async Task<List<UpdateDetails>> AutomaticPush()
        {
            var listOfUpdatedMod = new List<UpdateDetails>();
            ParseUnzippedModPath();
            GetModsNames();

            //TODO : Also copy the module manager and other dll at the root of the folder
            foreach (var modName in ModsNames)
            {
                var gameDataPathMod = GameDataPath + "\\" + modName + Path.DirectorySeparatorChar;
                var unzippedModGameDataPath = UnzippedModGameDataPath + modName + Path.DirectorySeparatorChar;

                if (!Directory.Exists(gameDataPathMod))
                {
                    var updater = new UpdateMod(gameDataPathMod, unzippedModGameDataPath);
                    updater.MoveNewModtoDefinitivePath();
                    listOfUpdatedMod.Add(new UpdateDetails()
                    {
                            ModName = modName,
                            Status = UpdateStatus.ModAdded,
                            Tooltip = modName + " added",
                    });
                }
                else if (IsDowloadedModMoreRecent(gameDataPathMod, unzippedModGameDataPath))
                {
                    var updater = new UpdateMod(gameDataPathMod, unzippedModGameDataPath);

                    await updater.Execute();
                    listOfUpdatedMod.Add(new UpdateDetails()
                    {
                        ModName = modName,
                        Status = UpdateStatus.SuccessfullyUpdated,
                        Tooltip = modName + " updated"
                    });
                }
                else
                {
                    listOfUpdatedMod.Add(new UpdateDetails()
                    {
                        ModName = modName,
                        Status = UpdateStatus.AlreadyUpdated,
                        Tooltip = modName + " is already up to date"
                    });
                }
            }

            return listOfUpdatedMod;
        }
    }
}
