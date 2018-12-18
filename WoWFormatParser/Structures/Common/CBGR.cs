using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CBGR : IStringDescriptor
    {
        public float b;
        public float g;
        public float r;

        public override string ToString() => $"R: {r}, G: {g}, B: {b}";
    }
}
