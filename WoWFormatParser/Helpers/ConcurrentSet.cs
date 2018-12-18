using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace WoWFormatParser.Helpers
{
    internal class ConcurentSet<T> : ISet<T>, IEnumerable<T>
    {
        private static readonly T[] emptyData = { };
        private HashSet<T> Set;
        private T[] safeCached;

        public ConcurentSet()
        {
            Set = new HashSet<T>();
        }

        public ConcurentSet(IEnumerable<T> collection)
        {
            Set = new HashSet<T>(collection);
        }

        public ConcurentSet(IEqualityComparer<T> comparer)
        {
            Set = new HashSet<T>(comparer);
        }

        public object Lock { get; } = new object();

        public int Count
        {
            get
            {
                lock (Lock)
                {
                    return Set.Count;
                }
            }
        }

        public bool IsReadOnly => false;

        public IEnumerable<T> Items
        {
            get
            {
                var items = ToArray();
                foreach (var x in items)
                    yield return x;
            }
        }

        public IEnumerable<T> ItemsUnsafe
        {
            get
            {
                foreach (var x in Set)
                    yield return x;
            }
        }

        public T[] ToArray()
        {
            T[] result;
            lock (Lock)
            {
                if (safeCached == null)
                    safeCached = Set.ToArray();

                result = safeCached;
            }
            return result;
        }

        public bool Add(T item)
        {
            lock (Lock)
            {
                safeCached = null;
                return Set.Add(item);
            }
        }

        public void Clear()
        {
            lock (Lock)
            {
                safeCached = emptyData;
                Set.Clear();
            }
        }

        public bool Contains(T item)
        {
            lock (Lock)
            {
                return Set.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            var temp = ToArray();
            Array.Copy(temp, 0, array, arrayIndex, temp.Length);
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            lock (Lock)
            {
                safeCached = null;
                Set.ExceptWith(other);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            lock (Lock)
            {
                safeCached = null;
                Set.IntersectWith(other);
            }
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            lock (Lock)
            {
                return Set.IsProperSubsetOf(other);
            }
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            lock (Lock)
            {
                return Set.IsProperSupersetOf(other);
            }
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            lock (Lock)
            {
                return Set.IsSubsetOf(other);
            }
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            lock (Lock)
            {
                return Set.IsSupersetOf(other);
            }
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            lock (Lock)
            {
                return Set.Overlaps(other);
            }
        }

        public bool Remove(T item)
        {
            lock (Lock)
            {
                safeCached = null;
                return Set.Remove(item);
            }
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            lock (Lock)
            {
                return Set.SetEquals(other);
            }
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            lock (Lock)
            {
                safeCached = null;
                Set.SymmetricExceptWith(other);
            }
        }

        public void UnionWith(IEnumerable<T> other)
        {
            lock (Lock)
            {
                safeCached = null;
                Set.UnionWith(other);
            }
        }

        void ICollection<T>.Add(T item) => Add(item);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
