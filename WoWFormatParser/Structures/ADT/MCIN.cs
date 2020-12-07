using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.ADT
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MCIN : IStringDescriptor
    {
        public uint Offset;
        public uint Size;
#pragma warning disable 0169
        private readonly uint Flags;
        private readonly uint Pad;
#pragma warning restore 0169

        public override string ToString() => $"Offset: {Offset}, Size: {Size}";

    }
}
