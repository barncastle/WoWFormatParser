using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.WMO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MOLM : IStringDescriptor
    {
        public byte X;
        public byte Y;
        public byte Width;
        public byte Height;

        public override string ToString() => $"X: {X}, Y: {Y}, Width: {Width}, Height: {Height}";
    }
}
