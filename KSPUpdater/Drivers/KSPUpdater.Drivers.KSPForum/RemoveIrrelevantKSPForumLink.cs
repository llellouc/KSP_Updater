using System;
using System.Collections.Generic;
using System.Text;
using KSPUpdater.Drivers.Common.Interfaces;

namespace KSPUpdater.Drivers.KSPForum
{
    public class RemoveIrrelevantKSPForumLink : IRemoveIrrelevantLinks
    {
        public List<string> RemoveDuplicateEntries(List<string> source)
        {
            return source;
        }

        public List<string> RemoveNotLinkedEntries(List<string> source, string modName)
        {
            return source;
        }
    }
}
