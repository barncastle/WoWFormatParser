using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.MDX
{
    public class MDXTrack<T> where T : struct
    {
        public int Count;
        public MDLTRACKTYPE InterpolationType;
        public int GlobalSequenceId;
        public MDXKeyFrame<T>[] Keys;

        public MDXTrack(BinaryReader br)
        {
            string name = br.ReadString(4);
            Count = br.ReadInt32();
            InterpolationType = (MDLTRACKTYPE)br.ReadUInt32();
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

        public MDXKeyFrame(BinaryReader br, MDLTRACKTYPE type)
        {
            Time = br.ReadUInt32();
            Value = br.ReadStruct<T>();
            if (type > MDLTRACKTYPE.TRACK_LINEAR)
            {
                InTangent = br.ReadStruct<T>();
                OutTangent = br.ReadStruct<T>();
            }
        }
    }

    public enum MDLTRACKTYPE
    {
        TRACK_NO_INTERP = 0x0,
        TRACK_LINEAR = 0x1,
        TRACK_HERMITE = 0x2,
        TRACK_BEZIER = 0x3,
        NUM_TRACK_TYPES = 0x4,
    }
}
