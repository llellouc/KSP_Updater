using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KSPUpdater.Common
{
    public static class DeleteAndPreventAlreadyInUse
    {
        public static void DeleteFile(string path)
        {
            DeleteFile(path, false);
        }

        public static void DeleteDirectory(string path, bool recursive)
        {
            DeleteDirectory(path, recursive, false);
        }

        private static void DeleteDirectory(string path, bool recursive, bool asAlreadyRetry)
        {
            try
            {
                Directory.Delete(path, recursive);
            }
            catch (IOException e)
            {
                //If as already retry or if the exception is not an already in use exception (for example a directory not found exception)
                if (asAlreadyRetry || e.GetType() != typeof(IOException))
                    throw;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                DeleteAndPreventAlreadyInUse.DeleteDirectory(path, recursive,true);
            }
        }

        private static void DeleteFile(string path, bool asAlreadyRetry)
        {
            try
            {
                File.Delete(path);
            }
            catch (IOException e)
            {
                //If as already retry or if the exception is not an already in use exception (for example a file not found exception)
                if (asAlreadyRetry || e.GetType() != typeof(IOException))
                    throw;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                DeleteAndPreventAlreadyInUse.DeleteFile(path, true);
            }
        }
    }
}
