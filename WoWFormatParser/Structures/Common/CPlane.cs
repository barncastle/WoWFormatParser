using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.Common
{
    public struct CPlane : IStringDescriptor
    {
        public float Length;
        public float Width;

        public override string ToString() => $"Length: {Length}, Width: {Width}";
    }
}
