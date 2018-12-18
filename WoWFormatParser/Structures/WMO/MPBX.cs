using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.WMO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MPBX : IStringDescriptor
    {
        public ushort StartIndex;
        public ushort Count;

        public override string ToString() => $"StartIndex: {StartIndex}, Count: {Count}";
    }
}
