using GhostCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GhostCore.Networking
{
    public class RestProxy
    {
        protected string _baseUrl;
        protected string _urlOverride;
        protected string _endpoint;

        public string LastErrorMessage { get; set; }

        public RestProxyAuthenicationHandler AuthenticationHandler { get; set; }

        public virtual string CoreRequestURL
        {
            get
            {
                if (_urlOverride != null)
                    return _urlOverride;
                return string.Format("{0}/{1}", _baseUrl, _endpoint);
            }
        }

        public RestProxy() : this(null, null, new RestProxyAuthenicationHandler())
        {

        }

        public RestProxy(string baseUrl, string endpoint)
            : this(baseUrl, endpoint, new RestProxyAuthenicationHandler())
        {

        }

        public RestProxy(string baseUrl, string endpoint, RestProxyAuthenicationHandler authHandler)
        {
            _baseUrl = baseUrl;
            _endpoint = endpoint;
            AuthenticationHandler = authHandler;
        }

        public HttpContent CreateHttpContent(object data)
        {
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

        protected async Task<T> PostAsync<T>(string url, object req, bool useAuth = false)
        {
            var cli = new HttpClient();

            if (useAuth)
            {
                AuthenticationHandler.AddAuthenticationHeader(cli);
            }

            var content = CreateHttpContent(req);
            string requestUri = url;
            var httpResponse = await cli.PostAsync(requestUri, content);

            if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                cli.Dispose();
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

                cli.Dispose();

                return data;
            }
        }

        protected async Task<T> GetAsync<T>(string url, bool useAuth = false)
        {
            var cli = new HttpClient();

            if (useAuth)
            {
                AuthenticationHandler.AddAuthenticationHeader(cli);
            }

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
                cli.Dispose();
                return default(T);
            }
            else
            {
                var respString = await httpResponse.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<T>(respString);

                cli.Dispose();
                return data;
            }
        }

        protected async Task<byte[]> GetRawAsync(string url, bool useAuth = false)
        {
            var cli = new HttpClient();

            if (useAuth)
            {
                AuthenticationHandler.AddAuthenticationHeader(cli);
            }

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
                cli.Dispose();
                return null;
            }
            else
            {
                var data = await httpResponse.Content.ReadAsByteArrayAsync();

                cli.Dispose();
                return data;
            }
        }
    }

    public class RestProxyAuthenicationHandler
    {
        public virtual void AddAuthenticationHeader(HttpClient cli)
        {

        }
    }

    public class ErrorResponse
    {
        public bool Success { get; set; }
        public string ServerMessage { get; set; }
        public int ServerCode { get; set; }
    }
}
