using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.Common
{
    public struct C2Vector : IStringDescriptor
    {
        public float X;
        public float Y;

        public override string ToString() => $"X: {X}, Y: {Y}";
    }
}
