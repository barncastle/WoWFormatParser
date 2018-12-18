using System;
using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.ADT
{
    public class MDDF
    {
        public uint NameId;
        public int UniqueId;
        public C3Vector Position;
        public C3Vector Rotation;
        public ushort Scale;
        public MODDF_Flags Flags;

        public MDDF(BinaryReader br)
        {
            NameId = br.ReadUInt32();
            UniqueId = br.ReadInt32();
            Position = br.ReadStruct<C3Vector>();
            Rotation = br.ReadStruct<C3Vector>();
            Scale = br.ReadUInt16();
            Flags = br.ReadEnum<MODDF_Flags>();
        }
    }

    [Flags]
    public enum MODDF_Flags : ushort
    {
        None = 0,
        Biodome = 1,
        Shrubbery = 2
    }
}
