namespace GhostCore.SimpleIoC
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
                    IsInitialized = ServiceLocator.Instance.TryResolve(out _service);

                return _service;
            }
        }
    }
}
