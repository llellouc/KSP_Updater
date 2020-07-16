using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace KSPUpdater.Client
{
    class PushUpdatedMod
    {
        public string UnzippedModRootPath { get; private set; }
        public string UnzippedModGameDataPath { get; private set; }
        public string GameDataPath { get; private set; }

        public List<string> ModsNames { get; private set; }

        public PushUpdatedMod(string unzippedModRootPath, string gameDataPath)
        {
            this.UnzippedModRootPath = unzippedModRootPath;
            this.GameDataPath = gameDataPath;

            this.UnzippedModGameDataPath = null;
            ModsNames = new List<string>();
        }

        private void ParseUnzippedModPath()
        {
            var subDirectories = Directory.GetDirectories(this.UnzippedModRootPath).ToList();
            if (subDirectories.Any(x => x.EndsWith("GameData")))
                UnzippedModGameDataPath = UnzippedModRootPath + Path.DirectorySeparatorChar +"GameData" + Path.DirectorySeparatorChar;
            else
                throw new ArgumentException("Impossible to parse UnzippedModGameDataPath",
                    nameof(UnzippedModGameDataPath));
        }

        private void GetModsNames()
        {
            var regex = new Regex("[\\w]+$");
            foreach (var dir in Directory.GetDirectories(this.UnzippedModGameDataPath))
            {
                ModsNames.Add( regex.Match(dir).Value);
            }
        }

        private bool IsDowloadedModMoreRecent(string oldModPath, string unzippedModPath)
        {
            DotVersion oldModVersion = new DotVersion(Directory.GetFiles(oldModPath, "*.version", SearchOption.AllDirectories).First());
            DotVersion unzippedModVersion = new DotVersion(Directory.GetFiles(unzippedModPath, "*.version", SearchOption.AllDirectories).First());

            return unzippedModVersion > oldModVersion;
        }


        /// <returns>The list of updated Mods</returns>
        public List<string> AutomaticPush()
        {
            var listOfUpdatedMod = new List<string>();
            ParseUnzippedModPath();
            GetModsNames();

            foreach (var modName in ModsNames)
            {
                var gameDataPathMod = GameDataPath + modName + Path.DirectorySeparatorChar;
                var unzippedModGameDataPath = UnzippedModGameDataPath + modName + Path.DirectorySeparatorChar;

                if (IsDowloadedModMoreRecent(gameDataPathMod, unzippedModGameDataPath))
                {
                    var updater = new UpdateMod(gameDataPathMod, unzippedModGameDataPath);

                    updater.Execute();
                    listOfUpdatedMod.Add(modName);
                    Trace.WriteLine(modName + " updated");
                }
                else
                {
                    Trace.WriteLine(modName + " is already up to date");
                }
            }

            return listOfUpdatedMod;
        }
    }
}
