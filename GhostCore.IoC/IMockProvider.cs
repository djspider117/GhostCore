using System.Threading.Tasks;

namespace GhostCore.IoC
{
    public interface IMockProvider
    {
        void Initialize(IServiceCollection serviceCollection);
        void Close();
    }
    public interface IAsyncMockProvider
    {
        Task InitializeAsync(IServiceCollection serviceCollection);
        Task CloseAsync();
    }

}
