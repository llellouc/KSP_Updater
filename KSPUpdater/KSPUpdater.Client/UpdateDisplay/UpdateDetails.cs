using System;
using System.Collections.Generic;
using System.Text;

namespace KSPUpdater.Client.UpdateDisplay
{
    public class UpdateDetails
    {
        public string ModName { get; set; }
        public string Tooltip { get; set; }
        public UpdateStatus Status { get; set; }
    }
}
