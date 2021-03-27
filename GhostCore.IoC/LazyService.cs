using System.Threading.Tasks;

namespace GhostCore.IoC
{
    public struct LazyService<T>
    {
        private T _service;

        /// <summary>
        /// Returns true if the value is initialized from the ServiceLocator, false otherwise.
        /// </summary>
        public bool IsInitialized { get; set; }

        /// <summary>
        /// Returns the service if is initialized, false otherwise
        /// </summary>
        public T Value
        {
            get
            {
                if (_service == null && !IsInitialized)
                    IsInitialized = ServiceLocator.Instance.TryGet(out _service);

                return _service;
            }
        }

        public void Evaluate(bool tryInitialize = true)
        {
            IsInitialized = ServiceLocator.Instance.TryGet(out _service, tryInitialize);
        }

        public async Task EvaluateAsync(bool tryInitialize = true)
        {
            var t = await ServiceLocator.Instance.TryGetAsync<T>(tryInitialize);
            IsInitialized = t.Item1;
            _service = t.Item2;
        }
    }
}
