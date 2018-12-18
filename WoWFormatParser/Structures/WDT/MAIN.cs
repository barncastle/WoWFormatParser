using System;
using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.WDT
{
    public class MAIN
    {
        public MAIN_Flags Flags;
        public uint AsyncId;

        public MAIN(BinaryReader br)
        {
            Flags = br.ReadEnum<MAIN_Flags>();
            AsyncId = br.ReadUInt32();
        }
    }

    [Flags]
    public enum MAIN_Flags : uint
    {
        None = 0,
        HasADT = 1,
        Loaded = 2,
    }
}
