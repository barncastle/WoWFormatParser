using System;
using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.WDT
{
    public class MODF
    {
        public uint NameId;
        public int UniqueId;
        public C3Vector Pos;
        public C3Vector Rot;
        public CAaBox Extents;
        public MODF_Flags Flags;
        public ushort DoodadSet;
        public ushort NameSet;
        public ushort Scale;

        public MODF(BinaryReader br)
        {
            NameId = br.ReadUInt32();
            UniqueId = br.ReadInt32();
            Pos = br.ReadStruct<C3Vector>();
            Rot = br.ReadStruct<C3Vector>();
            Extents = br.ReadStruct<CAaBox>();
            Flags = br.ReadEnum<MODF_Flags>();
            DoodadSet = br.ReadUInt16();
            NameSet = br.ReadUInt16();
            Scale = br.ReadUInt16();
        }
    }

    [Flags]
    public enum MODF_Flags : ushort
    {
        None = 0,
        Destructible = 1,
        UseLOD = 2,
        Unknown_0x4 = 4
    }
}
