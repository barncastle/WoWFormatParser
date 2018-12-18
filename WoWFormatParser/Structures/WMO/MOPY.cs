using System;
using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.WMO
{
    public class MOPY
    {
        public MOPY_Flags Flags;
        public byte? LightmapTex;
        public byte MtlId;

        public MOPY(BinaryReader br, uint version)
        {
            Flags = br.ReadEnum<MOPY_Flags>();

            if (version != 17)
                LightmapTex = br.ReadByte();

            MtlId = br.ReadByte();

            if (version != 17)
                br.ReadByte(); // padding
        }

        public static int GetSize(uint version) => version != 17 ? 4 : 2;
    }

    [Flags]
    public enum MOPY_Flags : byte
    {
        None = 0,
        Unknown_0x1 = 0x01,
        NoCameraCollide = 0x02,
        Detail = 0x04,
        HasCollision = 0x08,
        Hint = 0x10,
        Render = 0x20,
        Unknown_0x40 = 0x40,
        CollideHit = 0x80
    }
}
