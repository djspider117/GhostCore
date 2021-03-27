using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Windows.Storage;

namespace GhostCore.Animations
{
    public class FoundationCompositionWriter
    {
        private static volatile FoundationCompositionWriter _instance;
        private static object _syncRoot = new object();

        internal FoundationCompositionWriter()
        {

        }

        public static FoundationCompositionWriter GetDefault()
        {
            if (_instance == null)
            {
                lock (_syncRoot)
                {
                    if (_instance == null)
                        _instance = new FoundationCompositionWriter();
                }
            }

            return _instance;
        }

        public async Task WriteFile(IStorageFile file, FoundationComposition comp)
        {
            var seri = await CreateXmlString(comp);
            await FileIO.WriteTextAsync(file, seri);
        }

        public async Task WriteFile(string path, FoundationComposition comp)
        {
            var file = await StorageFile.GetFileFromPathAsync(path);
            await WriteFile(file, comp);
        }

        public Task<string> CreateXmlString(FoundationComposition comp)
        {
            return Task.Run(() =>
            {
                var seri = new XmlSerializer(typeof(FoundationComposition));

                using (StringWriter textWriter = new StringWriter())
                {
                    seri.Serialize(textWriter, comp);
                    return textWriter.ToString();
                }
            });
        }
    }
}
