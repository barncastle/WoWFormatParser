using System;
using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.WDT
{
    public class MAIN
    {
        public int Offset;
        public int Size;
        public MAIN_Flags Flags;

        public MAIN(BinaryReader br, uint build)
        {
            if(build <  3592)
            {
                Offset = br.ReadInt32();
                Size = br.ReadInt32();
            }

            Flags = br.ReadEnum<MAIN_Flags>();
            br.ReadUInt32(); // AsyncId
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
