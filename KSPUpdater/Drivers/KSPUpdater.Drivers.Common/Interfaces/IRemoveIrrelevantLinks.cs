using System;
using System.Collections.Generic;
using System.Text;

namespace KSPUpdater.Drivers.Common.Interfaces
{
    public interface IRemoveIrrelevantLinks
    {
        //TODO : Create a function that do both at the same time
        List<string> RemoveDuplicateEntries(List<string> source);
        List<string> RemoveNotLinkedEntries(List<string> source, string modName);

    }
}
