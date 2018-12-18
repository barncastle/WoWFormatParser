using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.Common
{
    public struct C4Vector : IStringDescriptor
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public override string ToString() => $"X: {X}, Y: {Y}, Z: {Z}, W: {W}";
    }
}
