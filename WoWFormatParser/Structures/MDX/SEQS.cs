using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.MDX
{
    public class SEQS
    {
        public string Name;
        public CiRange Time;
        public float MoveSpeed;
        public bool NonLooping;
        public CExtent Bounds;
        public float Frequency;
        public CiRange Replay;
        public uint BlendTime;

        public SEQS(BinaryReader br)
        {
            Name = br.ReadString(80).TrimEnd('\0');
            Time = br.ReadStruct<CiRange>();
            MoveSpeed = br.ReadSingle();
            NonLooping = br.ReadInt32() == 1;
            Bounds = br.ReadStruct<CExtent>();
            Frequency = br.ReadSingle();
            Replay = br.ReadStruct<CiRange>();
            BlendTime = br.ReadUInt32();
        }
    }
}
