using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.MDX
{
    public class MDXTrack<T> where T : struct
    {
        public int Count;
        public TrackType InterpolationType;
        public int GlobalSequenceId;
        public MDXKeyFrame<T>[] Keys;

        public MDXTrack(BinaryReader br)
        {
            string name = br.ReadString(4);
            Count = br.ReadInt32();
            InterpolationType = (TrackType)br.ReadUInt32();
            GlobalSequenceId = br.ReadInt32();
            Keys = br.ReadArray(Count, () => new MDXKeyFrame<T>(br, InterpolationType));
        }
    }

    public class MDXKeyFrame<T> where T : struct
    {
        public uint Time;
        public T Value;
        public T? InTangent;
        public T? OutTangent;

        public MDXKeyFrame(BinaryReader br, TrackType type)
        {
            Time = br.ReadUInt32();
            Value = br.ReadStruct<T>();
            if (type > TrackType.Linear)
            {
                InTangent = br.ReadStruct<T>();
                OutTangent = br.ReadStruct<T>();
            }
        }
    }

    public enum TrackType
    {
        NoInterp = 0x0,
        Linear = 0x1,
        Hermite = 0x2,
        Bezier = 0x3,
    }
}
