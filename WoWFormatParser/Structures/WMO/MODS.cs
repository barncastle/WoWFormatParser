using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.WMO
{
    public class MODS : IStringDescriptor
    {
        public string Name;
        public uint StartIndex;
        public uint Count;

        public MODS(BinaryReader br)
        {
            Name = br.ReadString(20).TrimEnd('\0');
            StartIndex = br.ReadUInt32();
            Count = br.ReadUInt32();
            br.BaseStream.Position += 4; // pad
        }

        public override string ToString() => $"Name: {Name}, StartIndex: {StartIndex}, Count: {Count}";
    }
}
