using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KSPUpdater.Common
{
    public class DeleteFolderAsync : IDisposable
    {
        private FileSystemWatcher _watcher;
        
        public SemaphoreSlim Sem { get; private set; }

        private DeleteFolderAsync(string path)
        {
            Sem = new SemaphoreSlim(0);

            _watcher = new FileSystemWatcher();

            DirectoryInfo di = new DirectoryInfo(path);
            var parentPath = di.Parent.FullName;
            var endpath = di.Name;

            _watcher.Path = parentPath;
            _watcher.Filter = "*" + endpath;

            // Watch for changes in LastAccess and LastWrite times, and
            // the renaming of files or directories.
            _watcher.NotifyFilter = NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName
                                   | NotifyFilters.DirectoryName;

            // Add event handlers.
            _watcher.Deleted += OnDelete;

            // Begin watching.
            _watcher.EnableRaisingEvents = true;
        }

        ~DeleteFolderAsync()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_watcher != null)
            {
                _watcher.Deleted -= OnDelete;
                _watcher.EnableRaisingEvents = false;
            }
            _watcher?.Dispose();
        }

        public static async Task DeleteAsync(string path, bool recursive)
        {
            var watcher = new DeleteFolderAsync(path);
            Directory.Delete(path, recursive);
            await watcher.Sem.WaitAsync();
            watcher.Dispose();
        }

        private void OnDelete(object sender, FileSystemEventArgs e)
        {
            Sem.Release();
        }
    }
}
