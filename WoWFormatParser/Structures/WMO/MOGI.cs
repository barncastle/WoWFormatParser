using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.WMO
{
    public class MOGI
    {
        public uint? Offset;
        public uint? Size;
        public MOGP_Flags Flags;
        public CAaBox AaBox;
        public int NameIndex;

        public MOGI(BinaryReader br, uint version)
        {
            if (version == 14)
            {
                Offset = br.ReadUInt32();
                Size = br.ReadUInt32();
            }

            Flags = br.ReadEnum<MOGP_Flags>();
            AaBox = br.ReadStruct<CAaBox>();
            NameIndex = br.ReadInt32();
        }

        public static int GetSize(uint version) => version == 14 ? 40 : 32;
    }
}
