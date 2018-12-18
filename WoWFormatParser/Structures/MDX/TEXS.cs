using System;
using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.MDX
{
    public class TEXS
    {
        public int ReplaceableId;
        public string Image;
        public TEXFLAGS Flags;

        public TEXS(BinaryReader br)
        {
            ReplaceableId = br.ReadInt32();
            Image = br.ReadString(260).TrimEnd('\0');
            Flags = (TEXFLAGS)br.ReadUInt32();
        }
    }

    [Flags]
    public enum TEXFLAGS : uint
    {
        WrapWidth = 1,
        WrapHeight = 2
    }
}
