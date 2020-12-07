using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.WMO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MOPR : IStringDescriptor
    {
        public ushort PortalIndex;
        public ushort GroupIndex;
        public short Side;
        private readonly ushort Filler;

        public override string ToString() => $"PortalIndex: {PortalIndex}, GroupIndex: {GroupIndex}, Side: {Side}";
    }
}
