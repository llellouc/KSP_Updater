using System;
using System.Collections.Generic;
using System.Text;

namespace KSPUpdater.Client.UpdateDisplay
{
    public enum UpdateStatus
    {
        ModAdded = 0,
        SuccessfullyUpdated = 1,
        AlreadyUpdated = 2,
        FailedToUpdate = 3,
    }
}
