using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.WLX
{
    public sealed class WLX : Format
    {
        public string Magic;
        public string Version;
        public ushort Flags;
        public ushort Unk_0xA;
        public int Block_Count_1;
        public WLX_Block[] Blocks_1;
        public int Block_Count_2;
        public WLX_Block_2[] Blocks_2;
        public byte Unk_0x12;

        public WLX(BinaryReader br)
        {
            Magic = br.ReadString(4).FastReverse();
            Flags = br.ReadUInt16();
            Version = br.ReadBytes(4).ToHex().TrimStart('0');
            Unk_0xA = br.ReadUInt16();

            Block_Count_1 = br.ReadInt32();
            if (Block_Count_1 > 0)
                Blocks_1 = br.ReadArray(Block_Count_1, () => new WLX_Block(br));

            Block_Count_2 = br.ReadInt32();
            if (Block_Count_2 > 0)
                Blocks_2 = br.ReadArray(Block_Count_2, () => new WLX_Block_2(br));

            if ((Flags & 1) == 1)
                Unk_0x12 = br.ReadByte();

            if (br.BaseStream.Position != br.BaseStream.Length)
                throw new UnreadContentException();
        }
    }

    public class WLX_Block
    {
        public C3Vector[] Heights; // 16,3
        public C2Vector Coord; // 2
        public ushort[] Data;

        public WLX_Block(BinaryReader br)
        {
            Heights = br.ReadStructArray<C3Vector>(16);
            Coord = br.ReadStruct<C2Vector>();
            Data = br.ReadArray(0x50, () => br.ReadUInt16());
        }
    }

    public class WLX_Block_2
    {
        public C3Vector Unk_0x0;
        public C2Vector Unk_0xC;
        public byte[] Unk_0x14; // [56]

        public WLX_Block_2(BinaryReader br)
        {
            Unk_0x0 = br.ReadStruct<C3Vector>();
            Unk_0xC = br.ReadStruct<C2Vector>();
            Unk_0x14 = br.ReadBytes(0x38);
        }
    }
}
