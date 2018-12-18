using System;
using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.WMO
{
    public class MOBN
    {
        public MOBN_Flags Flags;
        public ushort NegChild;
        public ushort PosChild;
        public ushort NFaces;
        public uint FaceStart;
        public float PlaneDist;

        public MOBN(BinaryReader br)
        {
            Flags = br.ReadEnum<MOBN_Flags>();
            NegChild = br.ReadUInt16();
            PosChild = br.ReadUInt16();
            NFaces = br.ReadUInt16();
            FaceStart = br.ReadUInt32();
            PlaneDist = br.ReadSingle();
        }
    }

    [Flags]
    public enum MOBN_Flags : short
    {
        NoChild = -1,
        XAxis = 0x0,
        YAxis = 0x1,
        ZAxis = 0x2,
        Leaf = 0x4,
    }
}
