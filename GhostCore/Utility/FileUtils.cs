using System.IO;

namespace GhostCore.Utility
{
    public static class FileUtils
    {
        public static bool IsFileLocked(this FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }

        public static bool IsFileLocked(this string fileName)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException(string.Format("The file {0} does not exist", fileName));

            FileInfo fnfo = new FileInfo(fileName);
            return fnfo.IsFileLocked();
        }
    }

}
