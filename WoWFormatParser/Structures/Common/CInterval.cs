using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.Common
{
    public struct CInterval : IStringDescriptor
    {
        public int Start;
        public int End;
        public int Repeat;

        public override string ToString() => $"Start: {Start}, End: {End}, Repeat: {Repeat}";
    }
}
