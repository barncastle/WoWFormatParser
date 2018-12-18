using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.WMO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MOVB : IStringDescriptor
    {
        public ushort FirstVertex;
        public ushort Count;

        public override string ToString() => $"FirstVertex: {FirstVertex}, Count: {Count}";
    }
}
