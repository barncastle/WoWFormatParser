using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CImVector : IStringDescriptor
    {
        public byte b;
        public byte g;
        public byte r;
        public byte a;

        public override string ToString() => $"B: {b}, G: {g}, R: {r}, A: {a}";
    }
}
