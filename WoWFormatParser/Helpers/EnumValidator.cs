using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WoWFormatParser.Helpers
{
    public static class EnumValidator
    {
        private const bool DoValidation = false;

        private static readonly ConcurrentDictionary<Type, EnumInfo> _enumCache = new ConcurrentDictionary<Type, EnumInfo>();

        private static readonly Dictionary<Type, Func<BinaryReader, long>> _valueConverter = new Dictionary<Type, Func<BinaryReader, long>>()
        {
            { typeof(byte), (br) => br.ReadByte() },
            { typeof(sbyte), (br) => br.ReadSByte() },
            { typeof(short), (br) => br.ReadInt16() },
            { typeof(ushort), (br) => br.ReadUInt16() },
            { typeof(int), (br) => br.ReadInt32() },
            { typeof(uint), (br) => br.ReadUInt32() },
        };

        public static T ReadEnum<T>(this BinaryReader br) where T : struct, IConvertible
        {
            var info = GetOrCreate<T>();

            long value = _valueConverter[info.UnderlyingType](br);
            if (DoValidation)
                Validate<T>(value);
            return (T)Convert.ChangeType(value, info.UnderlyingType);
        }

        public static void Validate<T>(long value) where T : struct, IConvertible
        {
            string name = typeof(T).Name;
            var info = GetOrCreate<T>();

            if (value > info.MaxValue)
                throw new Exception($"Value exceeds maximum {name} {value}");

            if (info.HasFlags && !GetFlags(value).All(x => info.Values.Contains(x)))
                throw new Exception($"Missing flag {name} 0x{value:X}");

            if (!info.HasFlags && !info.Values.Contains(value))
                throw new Exception($"Missing value {name} 0x{value:X}");
        }

        private static EnumInfo GetOrCreate<T>() where T : struct, IConvertible
        {
            if (!_enumCache.TryGetValue(typeof(T), out EnumInfo info))
            {
                var type = Enum.GetUnderlyingType(typeof(T));

                info = new EnumInfo()
                {
                    HasFlags = typeof(T).IsDefined(typeof(FlagsAttribute), false),
                    Size = SizeCache.GetSize(type),
                    UnderlyingType = type,
                    Values = ((T[])Enum.GetValues(typeof(T))).Select(x => (long)Convert.ChangeType(x, typeof(long))).ToHashSet(),
                };

                info.MaxValue = info.HasFlags ? info.Values.Sum(x => x) : info.Values.Max();

                _enumCache[typeof(T)] = info;
            }

            return info;
        }

        private static IEnumerable<uint> GetFlags(long value)
        {
            for (int i = 0; i < 32; i++)
                if ((value & (1u << i)) != 0)
                    yield return 1u << i;
        }

        internal class EnumInfo
        {
            public bool HasFlags;
            public int Size;
            public Type UnderlyingType;
            public HashSet<long> Values;
            public long MaxValue;
        }
    }
}
