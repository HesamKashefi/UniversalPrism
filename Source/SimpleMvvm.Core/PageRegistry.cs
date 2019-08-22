using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleMvvm.Core
{
    public static class PageRegistry
    {
        static readonly Dictionary<string, (Type View, Type ViewModel)> Cache
            = new Dictionary<string, (Type View, Type ViewModel)>();

        public static void Register(string key, (Type View, Type ViewModel) info)
        {
            Cache.Add(key, info);
        }

        public static bool TryGetRegistration(string key, out (string Key, Type View, Type ViewModel) info)
        {
            if (Cache.ContainsKey(key))
            {
                info = (key, Cache[key].View, Cache[key].ViewModel);
                return true;
            }
            else
            {
                info = (null, null, null);
                return false;
            }
        }

        public static bool TryGetRegistration(Type view, out (string Key, Type View, Type ViewModel) info)
        {
            if (Cache.Any(x => x.Value.View == view))
            {
                var cache = Cache.FirstOrDefault(x => x.Value.View == view);
                info = (cache.Key, view, cache.Value.ViewModel);
                return true;
            }
            else if (TryGetRegistration(view.Name, out info))
            {
                return true;
            }
            else
            {
                info = (null, null, null);
                return false;
            }
        }
    }
}