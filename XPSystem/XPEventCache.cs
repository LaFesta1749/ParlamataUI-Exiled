using System.Collections.Generic;
using Exiled.API.Features;

namespace ParlamataUI.XPSystem
{
    public static class XPEventCache
    {
        private static readonly Dictionary<string, HashSet<string>> _cache = new();

        public static bool HasDone(string userId, string eventKey)
        {
            if (!_cache.ContainsKey(userId))
                _cache[userId] = new HashSet<string>();

            return !_cache[userId].Add(eventKey); // Add връща false, ако вече го има
        }

        public static void Clear()
        {
            _cache.Clear();
        }

        public static void OnPlayerLeft(string userId)
        {
            _cache.Remove(userId);
        }
    }
}
