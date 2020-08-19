using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;
using KSPUpdater.Drivers.Common.Interfaces;

namespace KSPUpdater.Drivers.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class DriverDetailsAttribute : Attribute
    {

        public string UrlPattern { get; private set; }

        public Func<List<string>, List<string>> RemoveDuplicateEntries { get; private set; }
        public Func<List<string>, string, List<string>> RemoveNotLinkedEntries { get; private set; }

        public DriverDetailsAttribute(string urlPattern, Type removeIrrelevantEntriesClass)
        {
            UrlPattern = urlPattern;

            var instance = Activator.CreateInstance(removeIrrelevantEntriesClass);
            var duplicateEntriesMethods = removeIrrelevantEntriesClass.GetMethod(nameof(IRemoveIrrelevantLinks.RemoveDuplicateEntries));
            var notLinkedEntriesMethods = removeIrrelevantEntriesClass.GetMethod(nameof(IRemoveIrrelevantLinks.RemoveNotLinkedEntries));

            if (duplicateEntriesMethods == null || notLinkedEntriesMethods == null)
                throw new NotImplementedException("Method RemoveDuplicateEntries and RemoveNotLinkedEntries not implemented in " + urlPattern + " driver");

            RemoveDuplicateEntries = (List<string> source) =>
            {
                object[] param = {source};
                return (List<string>)duplicateEntriesMethods.Invoke(instance, param);
            };

            RemoveNotLinkedEntries = (List<string> source, string modName) =>
            {
                object[] param = { source, modName };
                return (List<string>)notLinkedEntriesMethods.Invoke(instance, param);
            };
        }
    }
}