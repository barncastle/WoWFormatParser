using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.M2
{
    using System.Collections;
    using global::WoWFormatParser.Structures.Interfaces;
    using ReaderFunc = Dictionary<Type, Func<BinaryReader, int, Array>>;

    public class M2Array<T> : IReadOnlyList<T>, IVersioned
    {
        public int Count;
        public int Offset;
        public T[] Values;
        [JsonIgnore]
        public int Size;

        private readonly Type UnderlyingType;

        private readonly ReaderFunc _readerFuncs = new ReaderFunc()
        {
            { typeof(byte), (br, c) => br.ReadBytes(c) },
            { typeof(char), (br, c) => br.ReadChars(c) },
            { typeof(ushort), (br, c) => br.ReadStructArray<ushort>(c) },
            { typeof(short), (br, c) => br.ReadStructArray<short>(c) },
            { typeof(uint), (br, c) => br.ReadStructArray<uint>(c) },
            { typeof(int), (br, c) => br.ReadStructArray<int>(c) },
            { typeof(float), (br, c) => br.ReadStructArray<float>(c) },
            { typeof(bool), (br, c) => br.ReadArray(c, () => br.ReadBoolean()) },
        };


        public M2Array(BinaryReader br, uint version)
        {
            Count = br.ReadInt32();
            Offset = br.ReadInt32();

            if (Count > 0)
            {
                long pos = br.BaseStream.Position;
                br.BaseStream.Position = Offset;

                UnderlyingType = typeof(T);
                if (UnderlyingType.IsEnum)
                    UnderlyingType = UnderlyingType.GetEnumUnderlyingType();

                Values = Read(br, Count, version);
                Size = (int)(br.BaseStream.Position - Offset);

                br.BaseStream.Position = pos;
            }
        }


        private T[] Read(BinaryReader br, int count, uint version)
        {
            // pre-defined func
            if (_readerFuncs.TryGetValue(UnderlyingType, out var readfunc))
                return readfunc(br, count) as T[];

            // read struct
            if (UnderlyingType.IsValueType)
                return br.ReadStructArray<T>(count);

            // create instance with usual args
            bool isVersioned = typeof(IVersioned).IsAssignableFrom(typeof(T));

            object[] args = isVersioned ? new object[] { br, version } : new[] { br };
            return br.ReadArray(count, () => (T)Activator.CreateInstance(typeof(T), args));
        }

        public string AsString()
        {
            if (UnderlyingType == typeof(byte))
                return Encoding.UTF8.GetString(Values as byte[]).TrimEnd('\0');

            if (UnderlyingType == typeof(char))
                return new string(Values as char[]).TrimEnd('\0');

            throw new Exception("Invalid type");
        }

        #region IReadOnlyList Interface

        int IReadOnlyCollection<T>.Count => Count;

        public T this[int index] => Values[index];

        public IEnumerator<T> GetEnumerator() => Values?.Cast<T>().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
