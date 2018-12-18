using System;
using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.M2
{
    public class SplineKey<T>
    {
        public T Value;
        public T InTangent;
        public T OutTangent;

        public SplineKey(BinaryReader br)
        {
            Value = Read(br);
            InTangent = Read(br);
            OutTangent = Read(br);
        }

        private T Read(BinaryReader br)
        {
            if (typeof(T).IsValueType)
            {
                return br.ReadStruct<T>();
            }
            else
            {
                return (T)Activator.CreateInstance(typeof(T), br);
            }
        }
    }
}
