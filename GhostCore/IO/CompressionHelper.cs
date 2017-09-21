using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;

namespace GhostCore.IO
{
    public static class CompressionHelper
    {
        public static List<string> CurrentlyCompressing
        {
            get; private set;
        }

        static CompressionHelper()
        {
            CurrentlyCompressing = new List<string>();
        }

        public static void Compress(string path, params string[] parameters)
        {
            CurrentlyCompressing.Add(path);

            if (File.Exists(path))
                File.Delete(path);

            FileStream fsOut = File.Create(path);
            ZipOutputStream zipStream = new ZipOutputStream(fsOut);
            zipStream.SetLevel(3);

            foreach (var param in parameters)
            {
                if (!File.Exists(param))
                {
                    CurrentlyCompressing.Remove(path);
                    throw new FileNotFoundException(param);
                }

                ZipEntry entry = new ZipEntry(Path.GetFileName(param));

                zipStream.PutNextEntry(entry);
                ZipFileCore(zipStream, param);
                zipStream.CloseEntry();
            }

            zipStream.IsStreamOwner = true;
            zipStream.Close();
            CurrentlyCompressing.Remove(path);
        }

        public static void Decompress(string path, string outFolder, FileStream optFs = null)
        {
            ZipFile zf = null;
            try
            {
                FileStream fs = null;
                if (optFs == null)
                    fs = File.OpenRead(path);
                else
                    fs = optFs;

                zf = new ZipFile(fs);
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue; // Ignore directories
                    }
                    String entryFileName = zipEntry.Name;

                    byte[] buffer = new byte[4096];
                    Stream zipStream = zf.GetInputStream(zipEntry);

                    String fullZipToPath = Path.Combine(outFolder, entryFileName);
                    string directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                        Directory.CreateDirectory(directoryName);

                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true;
                    zf.Close();
                }
            }
        }

        private static void ZipFileCore(ZipOutputStream zipStream, string filename)
        {
            byte[] buffer = new byte[4096];
            using (FileStream streamReader = File.OpenRead(filename))
            {
                StreamUtils.Copy(streamReader, zipStream, buffer);
            }
        }
    }

}
