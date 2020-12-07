using System;
using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.WDT
{
    public class MPHD
    {
        public uint NDoodadNames;
        public uint OffsDoodadNames;   // MDNM
        public uint NMapObjNames;
        public uint OffsMapObjNames;   // MONM

        public MPHD_Flags Flags;
        public uint Unk_0x4;
        public uint[] Unk_0x8; // 6

        public MPHD(BinaryReader br, uint build)
        {
            if(build < 3592)
            {
                NDoodadNames = br.ReadUInt32();
                OffsDoodadNames = br.ReadUInt32();
                NMapObjNames = br.ReadUInt32();
                OffsMapObjNames = br.ReadUInt32();
                br.BaseStream.Position += 112;
            }
            else
            {
                Flags = br.ReadEnum<MPHD_Flags>();
                Unk_0x4 = br.ReadUInt32();
                Unk_0x8 = br.ReadStructArray<uint>(6);
            }
        }
    }

    [Flags]
    public enum MPHD_Flags : uint
    {
        None = 0,
        UsesGlobalModels = 0x01,
        UsesVertexShading = 0x02,
        UsesBigAlpha = 0x04,
        HasSortedDoodadrefs = 0x08,
    }

}
