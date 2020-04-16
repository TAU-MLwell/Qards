using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qards
{
    public class ShortTermMemory
    {
        private readonly IMemoryCache _cache;

        public int expiration_period { get; set; } = 20;
        public ShortTermMemory(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void Set(string key, object value)
        {
            _cache.Set(key, value, TimeSpan.FromMinutes(expiration_period));
        }

        public object Get(string key)
        {
            return _cache.Get(key);
        }
    }
}
