
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.Networking
{
    public static class RestUtility
    {
        public static async Task<ISafeTaskResult<T>> GetAsync<T>(string url)
        {
            using (var cli = new HttpClient())
            {
                var httpResponse = await cli.GetAsync(url);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    if (httpResponse.Content != null)
                    {
                        var data = await httpResponse.Content.ReadAsStringAsync();
                        return new SafeTaskResult<T>(data);
                    }
                    else
                    {
                        return new SafeTaskResult<T>(httpResponse.StatusCode.ToString());
                    }
                }
                else
                {
                    var respString = await httpResponse.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<T>(respString);

                    return new SafeTaskResult<T>(data);
                }
            }
        }
        public static async Task<ISafeTaskResult<byte[]>> GetAsyncRaw(string url)
        {
            using (var cli = new HttpClient())
            {
                var httpResponse = await cli.GetAsync(url);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    if (httpResponse.Content != null)
                    {
                        var data = await httpResponse.Content.ReadAsStringAsync();
                        return new SafeTaskResult<byte[]>(data);
                    }
                    else
                    {
                        return new SafeTaskResult<byte[]>(httpResponse.StatusCode.ToString());
                    }
                }
                else
                {
                    var data = await httpResponse.Content.ReadAsByteArrayAsync();
                    return new SafeTaskResult<byte[]>(data);
                }
            }
        }

        public static async Task<ISafeTaskResult> PostAsync(string url, object req)
        {
            using (var cli = new HttpClient())
            {
                var content = CreateHttpContent(req);
                string requestUri = url;
                var httpResponse = await cli.PostAsync(requestUri, content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    if (httpResponse.Content != null)
                    {
                        var data = await httpResponse.Content.ReadAsStringAsync();
                        return new SafeTaskResult(data);
                    }
                    else
                    {
                        return new SafeTaskResult(httpResponse.StatusCode.ToString());
                    }
                }
                else
                {
                    return SafeTaskResult.Ok;
                }
            }
        }

        public static async Task<ISafeTaskResult<T>> PostAsync<T>(string url, object req)
        {
            using (var cli = new HttpClient())
            {
                var content = CreateHttpContent(req);
                string requestUri = url;
                var httpResponse = await cli.PostAsync(requestUri, content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    if (httpResponse.Content != null)
                    {
                        var data = await httpResponse.Content.ReadAsStringAsync();
                        return new SafeTaskResult<T>(data);
                    }
                    else
                    {
                        return new SafeTaskResult<T>(httpResponse.StatusCode.ToString());
                    }
                }
                else
                {
                    var respString = await httpResponse.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<T>(respString);

                    return new SafeTaskResult<T>(data);
                }
            }
        }
        public static async Task<ISafeTaskResult<T>> PutAsync<T>(string url, object req)
        {
            using (var cli = new HttpClient())
            {
                var content = CreateHttpContent(req);
                string requestUri = url;
                var httpResponse = await cli.PutAsync(requestUri, content);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    if (httpResponse.Content != null)
                    {
                        var data = await httpResponse.Content.ReadAsStringAsync();
                        return new SafeTaskResult<T>(data);
                    }
                    else
                    {
                        return new SafeTaskResult<T>(httpResponse.StatusCode.ToString());
                    }
                }
                else
                {
                    var respString = await httpResponse.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<T>(respString);

                    return new SafeTaskResult<T>(data);
                }
            }
        }

        public static async Task<ISafeTaskResult<T>> DeleteAsync<T>(string url)
        {
            using (var cli = new HttpClient())
            {
                var httpResponse = await cli.DeleteAsync(url);

                if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    if (httpResponse.Content != null)
                    {
                        var data = await httpResponse.Content.ReadAsStringAsync();
                        return new SafeTaskResult<T>(data);
                    }
                    else
                    {
                        return new SafeTaskResult<T>(httpResponse.StatusCode.ToString());
                    }
                }
                else
                {
                    var respString = await httpResponse.Content.ReadAsStringAsync();
                    var data = JsonConvert.DeserializeObject<T>(respString);

                    return new SafeTaskResult<T>(data);
                }
            }
        }

        private static HttpContent CreateHttpContent(object data)
        {
            var seri = JsonConvert.SerializeObject(data);
            var content = new StringContent(seri, Encoding.UTF8, "application/json");
            return content;
        }
    }
}
