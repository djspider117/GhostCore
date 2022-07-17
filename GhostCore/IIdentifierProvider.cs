using System;
using System.Threading;
using System.Threading.Tasks;

namespace GhostCore
{
    public interface IIdentifierProvider
    {
        long GetNextId();

        Task InitializeAsync();
    }

    public class BasicIdentifierProvider : IIdentifierProvider
    {
        private long _curId;

        public long GetNextId() => Interlocked.Increment(ref _curId);

        public BasicIdentifierProvider()
        {
            _curId = DateTime.Now.Ticks;
        }

        public Task InitializeAsync() => Task.CompletedTask;
    }
}
