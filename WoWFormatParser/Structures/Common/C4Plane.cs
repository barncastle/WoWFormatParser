using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.Common
{
    public struct C4Plane : IStringDescriptor
    {
        public C3Vector Normal;
        public float Distance;

        public override string ToString() => $"Normal: [{Normal}] Distance: {Distance}";
    }
}
