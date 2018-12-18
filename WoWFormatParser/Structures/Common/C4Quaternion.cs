using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct C4Quaternion : IStringDescriptor
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public override string ToString() => $"X: {X}, Y: {Y}, Z: {Z}, W: {W}";
    }
}
