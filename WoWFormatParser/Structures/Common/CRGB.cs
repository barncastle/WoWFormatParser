using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CRGB : IStringDescriptor
    {
        public float r;
        public float g;
        public float b;

        public override string ToString() => $"R: {r}, G: {g}, B: {b}";
    }
}
