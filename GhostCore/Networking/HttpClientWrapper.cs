using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GhostCore.Networking
{
    internal class HttpClientWrapper : IDisposable
    {
        public HttpClient Client { get; internal set; }
        public HttpClientHandler Handler { get; internal set; }

        public HttpClientWrapper(HttpClient cli, HttpClientHandler handler = null)
        {
            Client = cli ?? throw new ArgumentNullException(nameof(cli));
            Handler = handler;
        }

        public Task<HttpResponseMessage> DeleteAsync(Uri requestUri, CancellationToken cancellationToken) => Client.DeleteAsync(requestUri, cancellationToken);
        public Task<HttpResponseMessage> DeleteAsync(string requestUri, CancellationToken cancellationToken) => Client.DeleteAsync(requestUri, cancellationToken);
        public Task<HttpResponseMessage> DeleteAsync(string requestUri) => Client.DeleteAsync(requestUri);
        public Task<HttpResponseMessage> DeleteAsync(Uri requestUri) => Client.DeleteAsync(requestUri);

        public Task<HttpResponseMessage> GetAsync(Uri requestUri) => Client.GetAsync(requestUri);
        public Task<HttpResponseMessage> GetAsync(string requestUri) => Client.GetAsync(requestUri);
        public Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption) => Client.GetAsync(requestUri, completionOption);
        public Task<HttpResponseMessage> GetAsync(string requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken) => Client.GetAsync(requestUri, completionOption, cancellationToken);
        public Task<HttpResponseMessage> GetAsync(string requestUri, CancellationToken cancellationToken) => Client.GetAsync(requestUri, cancellationToken);

        public Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption) => Client.GetAsync(requestUri, completionOption);
        public Task<HttpResponseMessage> GetAsync(Uri requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken) => Client.GetAsync(requestUri, completionOption, cancellationToken);
        public Task<HttpResponseMessage> GetAsync(Uri requestUri, CancellationToken cancellationToken) => Client.GetAsync(requestUri, cancellationToken);

        public Task<byte[]> GetByteArrayAsync(string requestUri) => Client.GetByteArrayAsync(requestUri);
        public Task<byte[]> GetByteArrayAsync(Uri requestUri) => Client.GetByteArrayAsync(requestUri);

        public Task<Stream> GetStreamAsync(string requestUri) => Client.GetStreamAsync(requestUri);
        public Task<Stream> GetStreamAsync(Uri requestUri) => Client.GetStreamAsync(requestUri);

        public Task<string> GetStringAsync(string requestUri) => Client.GetStringAsync(requestUri);
        public Task<string> GetStringAsync(Uri requestUri) => Client.GetStringAsync(requestUri);

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content) => Client.PostAsync(requestUri, content);
        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content, CancellationToken cancellationToken) => Client.PostAsync(requestUri, content, cancellationToken);
        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content) => Client.PostAsync(requestUri, content);
        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken) => Client.PostAsync(requestUri, content, cancellationToken);

        public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content) => Client.PutAsync(requestUri, content);
        public Task<HttpResponseMessage> PutAsync(string requestUri, HttpContent content, CancellationToken cancellationToken) => Client.PutAsync(requestUri, content, cancellationToken);
        public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content) => Client.PutAsync(requestUri, content);
        public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content, CancellationToken cancellationToken) => Client.PutAsync(requestUri, content, cancellationToken);

        public void Dispose()
        {
            if (Client != null)
            {
                Client.CancelPendingRequests();
                Client.Dispose();
            }

            if (Handler != null)
            {
                Handler.Dispose();
            }
        }
    }
}
