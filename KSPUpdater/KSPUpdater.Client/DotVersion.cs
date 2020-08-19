using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace KSPUpdater.Client
{
    public class DotVersion : IComparable
    {
        #region Properties
        public string ModName { get; private set; }
        public string Path { get; private set; }
        public string DownloadLink { get; private set; }
        public Version Version { get; private set; }
        public JObject Json { get; private set; }

        #endregion

        /// <exception cref="ArgumentException">To document</exception>
        private string GetPath(string path)
        {
            if (path.EndsWith(".version"))
                return path;
            //else
            var toRet = Directory.GetFiles(path, "*.version", SearchOption.AllDirectories).FirstOrDefault();

            if (toRet == null) 
                throw new ArgumentException("Impossible to find any .version file inside the following directory : " + path, nameof(path));
            return toRet;
        }

        /// <exception cref="ArgumentException">When can't find any .version</exception>
        public DotVersion(string path)
        {
            this.Path = GetPath(path);

            this.Json = JObject.Parse(File.ReadAllText(Path));

            if (Json["VERSION"].SelectToken("MAJOR") != null)
            {
                Int32.TryParse((string) Json["VERSION"]["MAJOR"], out int major);
                Int32.TryParse((string) Json["VERSION"]["MINOR"], out int minor);
                Int32.TryParse((string) Json["VERSION"]["PATCH"], out int patch);
                Int32.TryParse((string) Json["VERSION"]["BUILD"], out int build);
                this.Version = new Version(major, minor, patch, build);
            }
            else
                this.Version = Version.Parse(Json["VERSION"].Value<string>());

            this.DownloadLink = (string) Json["DOWNLOAD"];
            this.ModName = (string) Json["NAME"];
        }

        #region IComparable

        public static bool operator <(DotVersion a, DotVersion b)
        {
            return (a.CompareTo(b) < 0);
        }

        public static bool operator >(DotVersion a, DotVersion b)
        {
            return (a.CompareTo(b) > 0);
        }

        public int CompareTo(object obj)
        {
            //TODO : Maybe change it. Because it allows to compare version but 2 totally different mods with the same Version are interpreted as the same object
            if (obj is DotVersion other)
            {
                return this.Version.CompareTo(other.Version);
            }
            throw new ArgumentException("Can't compare DotVersion with type " + obj.GetType());
        }

        #endregion
    }
}
