using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.WMO
{
    public class MOBA
    {
        public byte? LightMap;
        public byte? Texture;
        public short[,] BoundingBox; // 2,3
        public uint StartIndex;
        public ushort Count;
        public ushort MinIndex;
        public ushort MaxIndex;
        public byte Flags;
        public byte? MaterialId;
        public byte[] Unknown_0x18;

        public MOBA(BinaryReader br, uint version)
        {
            if (version == 14)
            {
                LightMap = br.ReadByte();
                Texture = br.ReadByte();
            }

            BoundingBox = br.ReadJaggedArray(2, 3, () => br.ReadInt16());
            StartIndex = version == 14 ? br.ReadUInt16() : br.ReadUInt32();
            Count = br.ReadUInt16();
            MinIndex = br.ReadUInt16();
            MaxIndex = br.ReadUInt16();
            Flags = br.ReadByte();

            MaterialId = br.ReadByte();
            if (version == 14)
                MaterialId = null;

            if (version == 16)
                Unknown_0x18 = br.ReadBytes(8);
        }

        public static int GetSize(uint version) => version == 16 ? 32 : 24;
    }
}


