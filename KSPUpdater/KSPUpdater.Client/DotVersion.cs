using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace KSPUpdater.Client
{
    public class DotVersion : IComparable
    {
        public string Path { get; private set; }
        public string DownloadLink { get; private set; }
        public Version Version { get; private set; }
        public JObject Json { get; private set; }

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

        public DotVersion(string path)
        {
            this.Path = GetPath(path);
            this.Version = null;
            Json = null;

            this.Json = JObject.Parse(File.ReadAllText(Path));

            var test = Json["VERSION"];
            if (Json["VERSION"].SelectToken("MAJOR") != null)
            {
                Int32.TryParse((string)Json["VERSION"]["MAJOR"], out int major);
                Int32.TryParse((string)Json["VERSION"]["MINOR"], out int minor);
                Int32.TryParse((string)Json["VERSION"]["PATCH"], out int patch);
                Int32.TryParse((string)Json["VERSION"]["BUILD"], out int build);
                this.Version = new Version(major, minor, patch, build);
            }
            else
                this.Version = System.Version.Parse(Json["VERSION"].Value<string>());
            this.DownloadLink = (string)Json["DOWNLOAD"];
        }

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
            if (obj is DotVersion other)
            {
                return this.Version.CompareTo(other.Version);
            }
            throw new ArgumentException("Can't compare DotVersion with type " + obj.GetType());
        }
    }
}
