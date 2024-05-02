using CsvHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;

namespace GhostCore.UWP.Helpers
{
    public class FileHelpers
    {
        public static async Task<StorageFile> TryGetFileAsync(string path)
        {
            try
            {
                var file = await StorageFile.GetFileFromPathAsync(path);
                return file;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }
        public static async Task<StorageFolder> TryGetFolderAsync(string path)
        {
            try
            {
                var folder = await StorageFolder.GetFolderFromPathAsync(path);
                return folder;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public async static Task<IList<string>> ReadLinesFromTextFileAsync(StorageFile file)
        {
            if (file == null)
                return null;

            return await FileIO.ReadLinesAsync(file);
        }

        public async static Task<IEnumerable<Dictionary<string, string>>> ParseCsvRaw(string path)
        {
            StorageFile file = await TryGetFileAsync(path);

            if (file == null)
                return null;

            using (var fileStream = await file.OpenStreamForReadAsync().ConfigureAwait(false))
            using (StreamReader sr = new StreamReader(fileStream, Encoding.UTF8))
            using (CsvReader reader = new CsvReader(sr, CultureInfo.InvariantCulture))
            {
                reader.Configuration.IncludePrivateMembers = true;
                var records = reader.GetRecords<dynamic>();

                var returnValue = new List<Dictionary<string, string>>();

                foreach (IDictionary<string, object> record in records)
                {
                    returnValue.Add(new Dictionary<string, string>(record.Select(x => new KeyValuePair<string, string>(x.Key, x.Value?.ToString()))));
                }

                return returnValue;
            }
        }

        public static async Task<IEnumerable<T>> ParseCsv<T>(string path)
        {
            StorageFile file = await TryGetFileAsync(path);

            if (file == null)
                return null;

            using (var fileStream = await file.OpenStreamForReadAsync().ConfigureAwait(false))
            using (StreamReader sr = new StreamReader(fileStream, Encoding.UTF8))
            using (CsvReader reader = new CsvReader(sr, CultureInfo.InvariantCulture))
            {
                reader.Configuration.IncludePrivateMembers = true;
                var records = reader.GetRecords<T>();

                return records;
            }
        }

        /// <summary>
        /// Returns paths from svg (list of tuples (id, PathData))
        /// </summary>
        /// <param name="path">Path to svg file</param>
        /// <returns></returns>
        public async static Task<List<(string, string)>> LoadPathsFromSvgAsync(string path)
        {
            var file = await TryGetFileAsync(path);

            if (file == null)
                return null;

            var svg = await XmlDocument.LoadFromFileAsync(file);

            if (svg == null)
                return null;

            var elements = svg.GetElementsByTagName("path").OfType<XmlElement>();

            List<(string, string)> res = new List<(string, string)>();

            foreach (var el in elements)
            {
                var d = el?.Attributes?.GetNamedItem("d")?.NodeValue?.ToString();
                var id = el?.Attributes?.GetNamedItem("id")?.NodeValue?.ToString();

                if (d != null && id != null)
                    res.Add((id, d));
            }

            return res;
        }
    }
}
