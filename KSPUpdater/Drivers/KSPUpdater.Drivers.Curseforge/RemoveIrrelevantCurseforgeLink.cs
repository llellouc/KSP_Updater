using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Data;
using KSPUpdater.Drivers.Common.Interfaces;

namespace KSPUpdater.Drivers.Curseforge
{
    public class RemoveIrrelevantCurseforgeLink : IRemoveIrrelevantLinks
    {
        public List<string> RemoveDuplicateEntries(List<string> source)
        {
            var toRet = new Dictionary<string, CurseforgeDetails>();

            foreach (var link in source)
            {
                var modName = "";
                var priority = 0; //The priority is the download number inside the url or intMax if we have a direct link to the folder
                var splitedLink = link.Split('/');

                //Directly with the download inside
                if (Regex.IsMatch(link, "^http(s?)://(www\\.)?curseforge.com/kerbal/ksp-mods/[^/]+(/download/[^/]+)?(/)?$"))
                {
                    modName = splitedLink[5];
                    if(splitedLink.Length >= 8)
                        Int32.TryParse(splitedLink[7], out priority);

                }
                else if (Regex.IsMatch(link, "^http(s?)://kerbal.curseforge.com/projects/[^/]+(/files)?(/)?$"))
                {
                    modName = splitedLink[4];
                    priority = Int32.MaxValue;
                }
                else
                    throw new NotImplementedException("Impossible to parse this entry to remove duplicate : " + link);
                modName = modName.ToLower().Replace(" ", "");
                if (!toRet.ContainsKey(modName))
                    toRet.Add(modName, new CurseforgeDetails(priority, link));
                else if (toRet[modName].Priority < priority)
                    toRet[modName] = new CurseforgeDetails(priority, link);
            }
            return toRet.Values.Select(x => x.Link).ToList();
        }

        public List<string> RemoveNotLinkedEntries(List<string> source, string modName)
        {
            var toRet = new List<string>();
            foreach (var link in source)
            {
                var splitedLink = link.Split('/');
                var linkModName = "";

                if (Regex.IsMatch(link, "^http(s?)://(www\\.)?curseforge.com/kerbal/ksp-mods/[^/]+(/download/[^/]+)?(/)?$"))
                    linkModName = splitedLink[5];
                else if (Regex.IsMatch(link, "^http(s?)://kerbal.curseforge.com/projects/[^/]+(/files)?(/)?$"))
                    linkModName = splitedLink[4];
                else
                    throw new NotImplementedException("Impossible to parse this entry to remove duplicate : " + link);

                var cleanModName = new Regex("[ \\-_]");

                linkModName = cleanModName.Replace(linkModName, "").ToLower();
                modName = cleanModName.Replace(modName, "").ToLower();

                if(modName == linkModName)
                    toRet.Add(link);
            }

            return toRet;
        }
    }

    class CurseforgeDetails
    {
        public int Priority { get; set; }
        public string Link { get; set; }

        public CurseforgeDetails() { }

        public CurseforgeDetails(int priority, string link)
        {
            Priority = priority;
            Link = link;
        }
    }
}
