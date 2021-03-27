using System.Net.Http;
using System.Threading.Tasks;

namespace GhostCore.Networking
{
    public class RestProxyAuthenticationHandler
    {
        public virtual Task AddAuthenticationHeader(HttpClient cli)
        {
            return Task.CompletedTask;
        }
    }
}
