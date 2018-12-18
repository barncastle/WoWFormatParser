using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct C3bVector : IStringDescriptor
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public byte[] Value;

        public override string ToString() => $"[{Value[0]}, {Value[1]}, {Value[2]}]";
    }
}
