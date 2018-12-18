using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.M2
{
    public class M2Event : IVersioned
    {
        public string EventName;
        public uint Data;
        public uint BoneIndex;
        public C3Vector Position;
        public M2Track<bool> EventTrack;

        public M2Event(BinaryReader br, uint build)
        {
            EventName = br.ReadString(4);
            Data = br.ReadUInt32();
            BoneIndex = br.ReadUInt32();
            Position = br.ReadStruct<C3Vector>();
            EventTrack = new M2Track<bool>(br, build, true);
        }
    }
}
