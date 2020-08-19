using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using KSPUpdater.Common;
using KSPUpdater.Drivers.Common.Interfaces;

namespace KSPUpdater.Drivers.Common
{
    public class DownloadLinkHelper
    {
        public static readonly Lazy<Dictionary<string, Type>> Drivers = new Lazy<Dictionary<string, Type>>(LoadTypes);

        //TODO : Add some try/catch
        private static Dictionary<string, Type> LoadTypes()
        {
            var toRet = new Dictionary<string, Type>();

            var files = Directory.GetFiles(".", "KSPUpdater.Drivers.*.dll");
            foreach (var file in files)
            {
                // Assembly.LoadFrom prevent to load the same assembly twice
                // Assembly.LoadFile prevent confusion between two different assemblies with the same name (so reload everytime)
                var assembly = Assembly.LoadFrom(file);
                var types = assembly.GetExportedTypes().Where(t => t.BaseType == typeof(IDownloadLink)).ToList();

                foreach (var type in types)
                {
                    var emptyElem = (IDownloadLink)Activator.CreateInstance(type);

                    var pattern = emptyElem.UrlPattern;
                    if(pattern != null)
                        toRet.Add(pattern, type);
                }
            }
            return toRet;
        }

        /// <exception cref="NotImplementedException">When the given url doesn't have any driver</exception>
        /// <exception cref="ArgumentException">When can't parse the url to obtain the download link</exception>
        public static IDownloadLink GetHostType(string urlBase, string modName, MyWebView wb)
        {
            foreach (var driver in Drivers.Value)
            {
                if (urlBase.Contains(driver.Key))
                {
                    IDownloadLink obj = (IDownloadLink)Activator.CreateInstance(driver.Value);
                    obj.Initialize(urlBase, modName,wb);
                    return obj;
                }
            }
            throw new NotImplementedException("The link " + urlBase + "comes from an unknown host");
        }
    }
}
