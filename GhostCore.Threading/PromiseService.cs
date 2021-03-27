using GhostCore.IoC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GhostCore.Threading
{
    [ServiceImplementation(typeof(IPromiseService))]
    public class PromiseService : IPromiseService
    {
        private static readonly object _lock = new object();
        private List<Promise> _items;

        public PromiseService()
        {
            _items = new List<Promise>();
        }

        public Promise GetExistingPromiseForId(int id)
        {
            var existing = _items.FirstOrDefault(x => x.FutureObjectId == id);
            return existing;
        }

        public Promise GetOrCreatePromiseForId(int id)
        {

            return GetOrCreatePromiseForId(id, null);

        }

        public Promise GetOrCreatePromiseForId(int id, Action<object> handler)
        {
            lock (_lock)
            {
                var existing = _items.FirstOrDefault(x => x.FutureObjectId == id);
                if (existing != null)
                {
                    existing.AddHandler(handler);
                    return existing;
                }

                var prom = new Promise(id, handler);
                prom.Disposing += Prom_Disposing;
                _items.Add(prom);

                return prom;
            }
        }

        public void ReleasePromiseForId(int id)
        {
            var obj = _items.FirstOrDefault(x => x.FutureObjectId == id);
            obj?.Dispose();
        }

        private void Prom_Disposing(object sender, EventArgs e)
        {
            lock (_lock)
            {
                var prom = sender as Promise;
                prom.Disposing -= Prom_Disposing;
                _items.Remove(prom);
            }
        }
    }
}
