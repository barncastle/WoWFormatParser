using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.WLX
{
    public sealed class WLQ : Format
    {
        public string Magic;
        public string Version;
        public ushort[] Unk_0x8;
        public int Block_Count;
        public WLX_Block[] Blocks;

        public WLQ(BinaryReader br)
        {
            Magic = br.ReadString(4).FastReverse();
            Version = br.ReadBytes(4).ToHex().TrimStart('0');
            Unk_0x8 = br.ReadArray(13, () => br.ReadUInt16());
            Block_Count = br.ReadInt32();

            if (Block_Count > 0)
                Blocks = br.ReadArray(Block_Count, () => new WLX_Block(br));

            if (br.BaseStream.Position != br.BaseStream.Length)
                throw new UnreadContentException();
        }
    }
}
