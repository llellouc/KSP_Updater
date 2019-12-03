﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KSPUpdater
{
    class UpdateMod
    {
        public string OldModPath { get; private set; }
        public string NewModTemporaryPath { get; private set; }

        public UpdateMod(string oldModPath, string newModTemporaryPath)
        {
            this.OldModPath = oldModPath;
            this.NewModTemporaryPath = newModTemporaryPath;
        }

        private void DeleteOld()
        {
            Directory.Delete(this.OldModPath, true);
        }

        private void MoveNewModtoDefinitivePath()
        {
            Directory.Move(NewModTemporaryPath, OldModPath);
        }

        public void Execute()
        {
            DeleteOld();
            MoveNewModtoDefinitivePath();
        }
    }
}
