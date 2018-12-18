using System;
using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.WMO
{
    public class MODD
    {
        public uint NameIndex;
        public MODD_Flags Flags;
        public C3Vector Position;
        public C4Quaternion Orientation;
        public float Scale;
        public CImVector Color;

        public MODD(BinaryReader br)
        {
            uint data = br.ReadUInt32();
            NameIndex = data & 0xFFFFFFu;
            Flags = (MODD_Flags)(data >> 24);
            Position = br.ReadStruct<C3Vector>();
            Orientation = br.ReadStruct<C4Quaternion>();
            Scale = br.ReadSingle();
            Color = br.ReadStruct<CImVector>();
        }
    }

    [Flags]
    public enum MODD_Flags : byte
    {
        None = 0,
        AcceptProjTex = 0x1,
        Unknown_0x2 = 0x2,
        Unknown_0x4 = 0x4,
        Unknown_0x8 = 0x8,
    }
}
