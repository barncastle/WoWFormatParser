using System;

namespace WoWFormatParser.Helpers
{
    internal class Singleton<T> where T : class, new()
    {
        protected Singleton() { }

        protected static readonly Lazy<T> instance = new Lazy<T>(() => new T());

        internal static T Instance => instance.Value;
    }
}
