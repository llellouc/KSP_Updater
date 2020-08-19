using System;
using System.Collections.Generic;
using System.Text;
using KSPUpdater.Drivers.Common.Interfaces;

namespace KSPUpdater.Drivers.SpaceDock
{
    public class RemoveIrrelevantSpaceDockLinks : IRemoveIrrelevantLinks
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
