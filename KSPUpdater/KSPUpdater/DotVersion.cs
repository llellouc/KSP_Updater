using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KSPUpdater
{
    public class DotVersion : IComparable
    {
        public string Path { get; private set; }
        public Version Version { get; private set; }
        public JObject Json { get; private set; }

        public DotVersion(string path)
        {
            this.Path = path;
            this.Version = null;
            Json = null;
        }

        public void LoadContent()
        {
            this.Json = JObject.Parse(File.ReadAllText(Path));

            var major = JsonConvert.DeserializeObject<int>((string) Json["VERSION"]["MAJOR"]);
            var minor = JsonConvert.DeserializeObject<int>((string)Json["VERSION"]["MINOR"]);
            var patch = JsonConvert.DeserializeObject<int>((string)Json["VERSION"]["PATCH"]);
            var build = JsonConvert.DeserializeObject<int>((string)Json["VERSION"]["BUILD"]);

            this.Version = new Version(major, minor, patch, build);
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
