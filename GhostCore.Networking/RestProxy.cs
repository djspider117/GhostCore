using GhostCore.Utility;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.Networking
{
    public class RestProxy
    {
        #region Constants

        public const int SERVER_PING_TIMEOUT_MS = 60 * 1000;

        #endregion

        #region Fields

        protected string _baseUrl;
        protected string _urlOverride;
        protected string _endpoint;

        #endregion

        #region Properties

        public string LastErrorMessage { get; set; }
        public bool IgnoreSSL { get; set; }

        public RestProxyAuthenticationHandler AuthenticationHandler { get; set; }

        public virtual string CoreRequestURL
        {
            get
            {
                if (_urlOverride != null)
                    return _urlOverride;
                return string.Format("{0}/{1}", _baseUrl, _endpoint);
            }
        }

        #endregion

        #region Constructors and Initialization

        public RestProxy() : this(null, null, new RestProxyAuthenticationHandler())
        {

        }

        public RestProxy(string baseUrl, string endpoint)
            : this(baseUrl, endpoint, new RestProxyAuthenticationHandler())
        {
        }

        public RestProxy(string baseUrl, string endpoint, RestProxyAuthenticationHandler authHandler)
        {
            _baseUrl = baseUrl;
            _endpoint = endpoint;
            AuthenticationHandler = authHandler;

#if DEBUG
            IgnoreSSL = true;
#endif
        }


        #endregion

        #region Helpers


        public HttpContent CreateHttpContent(object data)
        {
            if (data == null)
                return null;

            if (data is string str)
                return new StringContent(str, Encoding.UTF8, "application/json");

            var seri = JsonConvert.SerializeObject(data);
            var content = new StringContent(seri, Encoding.UTF8, "application/json");
            return content;
        }

        public string GetUrlForRoute(string routeName)
        {
            if (string.IsNullOrWhiteSpace(routeName))
                return CoreRequestURL;
            return $"{CoreRequestURL}/{routeName}";
        }

        public void OverrideCoreRequestParams(string fullUrl)
        {
            var idx = fullUrl.IndexOf('?');
            _urlOverride = fullUrl.Substring(0, idx);
        }

        protected void SetError(string data)
        {
            var errorData = JsonConvert.DeserializeObject<ErrorResponse>(data);
            LastErrorMessage = errorData.ServerMessage;
        }

        #endregion

        #region HttpClient Factory

        internal HttpClientWrapper MakeHttpClientWrapper(bool ignoreSSL = false)
        {
            if (ignoreSSL)
            {
                var handler = new HttpClientHandler
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
                };

                var cli = new HttpClient(handler);
                return new HttpClientWrapper(cli, handler);
            }
            else
            {
                var cli = new HttpClient();
                return new HttpClientWrapper(cli);
            }
        }

        #endregion

        #region API

        protected async Task<ISafeTaskResult> SafePostAsync(string url, object req, bool useAuth = false)
        {
            using var cli = MakeHttpClientWrapper(IgnoreSSL);
            return await SafeExecuteAsync(cli, url, req, cli.PostAsync, useAuth);
        }
        protected async Task<ISafeTaskResult<T>> SafePostAsync<T>(string url, object req, bool useAuth = false)
        {
            using var cli = MakeHttpClientWrapper(IgnoreSSL);
            return await SafeExecuteAsync<T>(cli, url, req, cli.PostAsync, useAuth);
        }

        protected async Task<ISafeTaskResult> SafePutAsync(string url, object req, bool useAuth = false)
        {
            using var cli = MakeHttpClientWrapper(IgnoreSSL);
            return await SafeExecuteAsync(cli, url, req, cli.PutAsync, useAuth);
        }
        protected async Task<ISafeTaskResult<T>> SafePutAsync<T>(string url, object req, bool useAuth = false)
        {
            using var cli = MakeHttpClientWrapper(IgnoreSSL);
            return await SafeExecuteAsync<T>(cli, url, req, cli.PutAsync, useAuth);
        }

        protected async Task<ISafeTaskResult> SafeDeleteAsync(string url, bool useAuth = false)
        {
            using var cli = MakeHttpClientWrapper(IgnoreSSL);
            return await SafeExecuteAsync(cli, url, null, cli.DeleteAsync, useAuth);
        }
        protected async Task<ISafeTaskResult<T>> SafeDeleteAsync<T>(string url, bool useAuth = false)
        {
            using var cli = MakeHttpClientWrapper(IgnoreSSL);
            return await SafeExecuteAsync<T>(cli, url, null, cli.DeleteAsync, useAuth);
        }

        protected async Task<ISafeTaskResult> SafeGetAsync(string url, bool useAuth = false)
        {
            using var cli = MakeHttpClientWrapper(IgnoreSSL);

            if (useAuth)
                await AuthenticationHandler.AddAuthenticationHeader(cli.Client);

            var httpResponse = await cli.GetAsync(url);

            if (!httpResponse.IsSuccessStatusCode)
            {
                if (httpResponse.Content != null)
                {
                    var data = await httpResponse.Content.ReadAsStringAsync();
                    return new SafeTaskResult(data, (int)httpResponse.StatusCode);
                }

                return new SafeTaskResult($"Failed with status code {httpResponse.StatusCode}", (int)httpResponse.StatusCode);
            }
            else
            {
                return SafeTaskResult.Ok;
            }
        }
        protected async Task<ISafeTaskResult<T>> SafeGetAsync<T>(string url, bool useAuth = false)
        {
            using var cli = MakeHttpClientWrapper(IgnoreSSL);

            if (useAuth)
                await AuthenticationHandler.AddAuthenticationHeader(cli.Client);

            var httpResponse = await cli.GetAsync(url);

            if (!httpResponse.IsSuccessStatusCode)
            {
                if (httpResponse.Content != null)
                {
                    var data = await httpResponse.Content.ReadAsStringAsync();
                    return new SafeTaskResult<T>(data, (int)httpResponse.StatusCode);
                }

                return new SafeTaskResult<T>($"Failed with status code {httpResponse.StatusCode}", (int)httpResponse.StatusCode);
            }
            else
            {
                var respString = await httpResponse.Content.ReadAsStringAsync();
                var data = await Task.Run(() => JsonConvert.DeserializeObject<T>(respString));

                return new SafeTaskResult<T>(data);
            }
        }


        protected async Task<T> PostAsync<T>(string url, object req, bool useAuth = false)
        {
            using var cli = MakeHttpClientWrapper(IgnoreSSL);

            if (useAuth)
                await AuthenticationHandler.AddAuthenticationHeader(cli.Client);

            var content = CreateHttpContent(req);
            string requestUri = url;
            var httpResponse = await cli.PostAsync(requestUri, content);

            if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                if (httpResponse.Content != null)
                {
                    var data = await httpResponse.Content.ReadAsStringAsync();
                    LastErrorMessage = data;
                    return default(T);
                }

                return default(T);
            }
            else
            {
                var respString = await httpResponse.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<T>(respString);

                return data;
            }
        }

        protected async Task<T> GetAsync<T>(string url, bool useAuth = false)
        {
            using var cli = MakeHttpClientWrapper(IgnoreSSL);

            if (useAuth)
                await AuthenticationHandler.AddAuthenticationHeader(cli.Client);

            var httpResponse = await cli.GetAsync(url);

            if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                if (httpResponse.Content != null)
                {
                    var data = await httpResponse.Content.ReadAsStringAsync();
                    LastErrorMessage = data;
                }
                else
                {
                    LastErrorMessage = httpResponse.StatusCode.ToString();
                }
                return default(T);
            }
            else
            {
                var respString = await httpResponse.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<T>(respString);

                return data;
            }
        }

        protected async Task<byte[]> GetRawAsync(string url, bool useAuth = false)
        {
            using var cli = MakeHttpClientWrapper(IgnoreSSL);

            if (useAuth)
                await AuthenticationHandler.AddAuthenticationHeader(cli.Client);

            var httpResponse = await cli.GetAsync(url);

            if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                if (httpResponse.Content != null)
                {
                    var data = await httpResponse.Content.ReadAsStringAsync();
                    LastErrorMessage = data;
                }
                else
                {
                    LastErrorMessage = httpResponse.StatusCode.ToString();
                }
                return null;
            }
            else
            {
                var data = await httpResponse.Content.ReadAsByteArrayAsync();

                return data;
            }
        }

        protected virtual async Task<string> DownloadFileAsync(string url, string downloadLocation, string fileNameWithoutExtension, bool useAuth = false)
        {
            using var cli = MakeHttpClientWrapper(IgnoreSSL);

            if (useAuth)
                await AuthenticationHandler.AddAuthenticationHeader(cli.Client);

            var httpResponse = await cli.GetAsync(url);

            if (!httpResponse.IsSuccessStatusCode)
            {
                if (httpResponse.Content != null)
                {
                    var data = await httpResponse.Content.ReadAsStringAsync();
                    LastErrorMessage = data;
                }
                else
                {
                    LastErrorMessage = httpResponse.StatusCode.ToString();
                }

                return null;
            }

            var responseData = await httpResponse.Content.ReadAsByteArrayAsync();

            var fileExtension = httpResponse.Content.Headers.ContentType.MediaType.ResolveExtensionFromMimeType();
            var filePath = System.IO.Path.Combine(downloadLocation, $"{fileNameWithoutExtension}{fileExtension}");

            System.IO.Directory.CreateDirectory(downloadLocation);
            System.IO.File.Create(filePath, responseData.Length).Close();

            System.IO.File.WriteAllBytes(filePath, responseData);

            return filePath;
        }

        protected virtual Task<bool> IsServerReachable()
        {
            return Task.FromResult(NetworkInterface.GetIsNetworkAvailable());
        }

        #endregion

        #region Internal

        internal async Task<ISafeTaskResult> SafeExecuteAsync(HttpClientWrapper cli, string url, object req, AsyncFunc<string, HttpContent, HttpResponseMessage> executor, bool useAuth = false)
        {
            try
            {
                if (useAuth)
                    await AuthenticationHandler.AddAuthenticationHeader(cli.Client);

                var content = CreateHttpContent(req);
                string requestUri = url;
                var httpResponse = await executor(requestUri, content);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    if (httpResponse.Content != null)
                    {
                        var data = await httpResponse.Content.ReadAsStringAsync();
                        return new SafeTaskResult(data, (int)httpResponse.StatusCode);
                    }

                    return new SafeTaskResult($"Failed with status code {httpResponse.StatusCode}", (int)httpResponse.StatusCode);
                }
                else
                {
                    return SafeTaskResult.Ok;
                }
            }
            catch (Exception ex)
            {
                return new SafeTaskResult(ex.Message, ex);
            }
        }
        internal async Task<ISafeTaskResult<T>> SafeExecuteAsync<T>(HttpClientWrapper cli, string url, object req, AsyncFunc<string, HttpContent, HttpResponseMessage> executor, bool useAuth = false)
        {
            try
            {
                if (useAuth)
                    await AuthenticationHandler.AddAuthenticationHeader(cli.Client);

                var content = CreateHttpContent(req);
                string requestUri = url;
                var httpResponse = await executor(requestUri, content);

                if (!httpResponse.IsSuccessStatusCode)
                {
                    if (httpResponse.Content != null)
                    {
                        var data = await httpResponse.Content.ReadAsStringAsync();
                        return new SafeTaskResult<T>(data, (int)httpResponse.StatusCode);
                    }

                    return new SafeTaskResult<T>($"Failed with status code {httpResponse.StatusCode}", (int)httpResponse.StatusCode);
                }
                else
                {
                    var respString = await httpResponse.Content.ReadAsStringAsync();
                    var data = await Task.Run(() => JsonConvert.DeserializeObject<T>(respString));

                    return new SafeTaskResult<T>(data);
                }
            }
            catch (Exception ex)
            {
                return new SafeTaskResult<T>(ex.Message, ex);
            }
        }

        #endregion
    }
}
