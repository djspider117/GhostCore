using System;
using GhostCore;
using GhostCore.Foundation;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using GhostCore.Utility;

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
        }


        #endregion

        #region Helpers


        public HttpContent CreateHttpContent(object data)
        {
            if (data is string str)
                return new StringContent(str, Encoding.UTF8, "application/json");

            var seri = JsonConvert.SerializeObject(data);
            var content = new StringContent(seri, Encoding.UTF8, "application/json");
            return content;
        }

        public string GetUrlForRoute(string routeName)
        {
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

            if (useAuth)
                await AuthenticationHandler.AddAuthenticationHeader(cli.Client);

            var content = CreateHttpContent(req);
            string requestUri = url;
            var httpResponse = await cli.PostAsync(requestUri, content);

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
        protected async Task<ISafeTaskResult<T>> SafePostAsync<T>(string url, object req, bool useAuth = false)
        {
            using var cli = MakeHttpClientWrapper(IgnoreSSL);

            if (useAuth)
                await AuthenticationHandler.AddAuthenticationHeader(cli.Client);

            var content = CreateHttpContent(req);
            string requestUri = url;
            var httpResponse = await cli.PostAsync(requestUri, content);

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
    }

    public class RestProxyAuthenticationHandler
    {
        public virtual Task AddAuthenticationHeader(HttpClient cli)
        {
            return Task.CompletedTask;
        }
    }

    public class ErrorResponse
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public int ServerCode { get; set; }
    }
}
