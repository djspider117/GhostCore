using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
}
