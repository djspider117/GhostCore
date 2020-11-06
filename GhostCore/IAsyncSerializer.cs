using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

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


    public class InheritSafeAsyncSerializer : IAsyncSerializer
    {
        private JsonSerializerSettings _seriSettings;

        public InheritSafeAsyncSerializer()
        {
            _seriSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                NullValueHandling = NullValueHandling.Ignore
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
            return Task.Run(() => JsonConvert.DeserializeObject<T>(str, _seriSettings));
        }
    }
}
