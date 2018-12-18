using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.M2
{
    public class M2Track<T> : IVersioned
    {
        public InterpolationType Type;
        public short GlobalSeqId;

        public M2Array<CiRange> InterpolationRanges;
        public M2Array<uint> LegacyTimestamps;
        public M2Array<T> LegacyValues;

        public M2Array<M2Array<uint>> Timestamps;
        public M2Array<M2Array<T>> Values;

        public M2Track(BinaryReader br, uint version, bool simple = false)
        {
            Type = br.ReadEnum<InterpolationType>();
            GlobalSeqId = br.ReadInt16();

            if (version < 264)
            {
                InterpolationRanges = br.ReadM2Array<CiRange>(version);
                LegacyTimestamps = br.ReadM2Array<uint>(version);
            }
            else
            {
                Timestamps = br.ReadM2Array<M2Array<uint>>(version);
            }

            if (simple)
                return;

            if (version < 264)
                LegacyValues = br.ReadM2Array<T>(version);
            else
                Values = br.ReadM2Array<M2Array<T>>(version);
        }
    }

    public enum InterpolationType : ushort
    {
        None = 0,
        Linear = 1,
        Bezier = 2,
        Hermite = 3
    }
}
