using System.IO;
using System.Threading.Tasks;
using KSPUpdater.Common;

namespace KSPUpdater.Client
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

        private async Task DeleteOld()
        {
            await DeleteFolderAsync.DeleteAsync(this.OldModPath, true);
        }

        private void MoveNewModtoDefinitivePath()
        {
            Directory.Move(NewModTemporaryPath, OldModPath);
        }

        public async Task Execute()
        {
            await DeleteOld();
            MoveNewModtoDefinitivePath();
        }
    }
}
