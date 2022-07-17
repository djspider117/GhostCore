using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json.Serialization;
using System.Numerics;

namespace GhostCore
{
    public interface IAsyncSerializer
    {
        Task<string> SerializeAsync(object data);
        Task<object> DeserializeAsync(string str);
        Task<T> DeserializeAsync<T>(string str);
    }

    public class JsonNetAsyncSerializer : IAsyncSerializer
    {
        public Task<string> SerializeAsync(object data)
        {
            return Task.Run(() => JsonConvert.SerializeObject(data));
        }

        public Task<object> DeserializeAsync(string str)
        {
            return Task.Run(() => JsonConvert.DeserializeObject(str));
        }

        public Task<T> DeserializeAsync<T>(string str)
        {
            return Task.Run(() => JsonConvert.DeserializeObject<T>(str));
        }
    }

    public class XmlAsyncSerializer : IAsyncSerializer
    {
        public Task<string> SerializeAsync(object data)
        {
            return Task.Run(() =>
            {
                using (var textWriter = new StringWriter())
                {
                    XmlSerializer serializer = new XmlSerializer(data.GetType());
                    serializer.Serialize(textWriter, data);

                    return textWriter.ToString();
                }
            });
        }

        public Task<object> DeserializeAsync(string str)
        {
            throw new NotImplementedException();
            //return Task.Run(() => JsonConvert.DeserializeObject(str));
        }

        public Task<T> DeserializeAsync<T>(string str)
        {
            return Task.Run(() =>
            {
                using (var strReader = new StringReader(str))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    var obj = serializer.Deserialize(strReader);

                    return (T)obj;
                }
            });
        }
    }


    public class InheritSafeAsyncSerializer : IAsyncSerializer
    {
        private JsonSerializerSettings _seriSettings;

        public static readonly InheritSafeAsyncSerializer SharedInstance = new InheritSafeAsyncSerializer();

        public InheritSafeAsyncSerializer()
        {
            _seriSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full,
                NullValueHandling = NullValueHandling.Ignore,
            };

#if DEBUG
            _seriSettings.Formatting = Formatting.Indented;
#endif
        }

        public Task<string> SerializeAsync(object data)
        {
            return Task.Run(() => JsonConvert.SerializeObject(data, _seriSettings));
        }

        public Task<object> DeserializeAsync(string str)
        {
            return Task.Run(() => JsonConvert.DeserializeObject(str, _seriSettings));
        }

        public Task<T> DeserializeAsync<T>(string str)
        {
            //megahack because WHAT THE FUCK????
            str = str.Replace("System.Numerics.Vector2, System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e",
                "System.Numerics.Vector2, System.Numerics.Vectors, Version=4.1.3.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a");

            //var res = JsonConvert.DeserializeObject<T>(str, _seriSettings);
            //return Task.FromResult(res);
            return Task.Run(() => JsonConvert.DeserializeObject<T>(str, _seriSettings));
        }
    }
}
