using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KSPUpdater.Drivers.Common.Interfaces;

namespace KSPUpdater.Drivers.Github
{
    public class RemoveIrrelevantGithubLink : IRemoveIrrelevantLinks
    {
        public List<string> RemoveDuplicateEntries(List<string> source)
        {
            var toRet = new Dictionary<GithubDetails, string>();
            foreach (var link in source)
            {
                var elems = link.Split('/');

                var details = new GithubDetails(elems[3], elems[4]);

                if (!toRet.ContainsKey(details))
                    toRet.Add(details, link);
            }
            return toRet.Values.ToList();
        }

        public List<string> RemoveNotLinkedEntries(List<string> source, string modName)
        {
            // Maybe use a more complex function to compare strings (have error with Interstellar Fuel switch for example
            var toRet = new List<string>();

            modName = modName.ToLower().Replace(" ", "");

            foreach (var link in source)
            {
                var elems = link.Split('/');
                var modNameInLink = elems[4].ToLower().Replace(" ", "");

                if (modName == modNameInLink)
                    toRet.Add(link);
            }
            return toRet;
        }
    }

    class GithubDetails
    {
        public string Owner { get; set; }
        public string RepoName { get; set; }

        public GithubDetails()
        {
        }

        public GithubDetails(string owner, string repoName)
        {
            Owner = owner;
            RepoName = repoName;
        }

        public override bool Equals(object obj)
        {
            if (obj is GithubDetails other)
                return this.Owner == other.Owner && this.RepoName == other.RepoName;
            //else
            return false;
        }

        public override int GetHashCode()
        {
            return Owner.GetHashCode() ^ RepoName.GetHashCode();
        }
    }
}
