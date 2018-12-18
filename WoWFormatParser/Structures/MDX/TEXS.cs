using System;
using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.MDX
{
    public class TEXS
    {
        public int ReplaceableId;
        public string Image;
        public Tex_Flags Flags;

        public TEXS(BinaryReader br)
        {
            ReplaceableId = br.ReadInt32();
            Image = br.ReadString(260).TrimEnd('\0');
            Flags = (Tex_Flags)br.ReadUInt32();
        }
    }

    [Flags]
    public enum Tex_Flags : uint
    {
        WrapWidth = 1,
        WrapHeight = 2
    }
}
