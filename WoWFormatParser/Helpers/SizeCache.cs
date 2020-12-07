using System;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;

namespace WoWFormatParser.Helpers
{
    internal static class SizeCache
    {
        private static readonly ConcurrentDictionary<Type, int> _sizeCache = new ConcurrentDictionary<Type, int>();

        public static int GetSize<T>() where T : struct
        {
            if (!_sizeCache.TryGetValue(typeof(T), out int size))
            {
                size = Marshal.SizeOf<T>();
                _sizeCache[typeof(T)] = size;
            }

            return size;
        }

        public static int GetSize(Type type)
        {
            if (!type.IsValueType)
                throw new NotImplementedException("Type is not a struct.");

            if (!_sizeCache.TryGetValue(type, out int size))
            {
                size = Marshal.SizeOf(type);
                _sizeCache[type] = size;
            }

            return size;
        }
    }
}
