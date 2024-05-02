using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace GhostCore.UWP.Utils
{
    public static class StorageFileUtils
    {
        public static async Task<bool> FileExists(string path)
        {
            try
            {
                var sf = await StorageFile.GetFileFromPathAsync(path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<bool> FolderExists(string path)
        {
            try
            {
                var sf = await StorageFolder.GetFolderFromPathAsync(path);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
