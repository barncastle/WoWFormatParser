using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct C2iVector : IStringDescriptor
    {
        public int x;
        public int y;

        public override string ToString() => $"X: {x}, Y: {y}";
    }
}
