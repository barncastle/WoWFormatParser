using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct C4bVector : IStringDescriptor
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Value;

        public override string ToString() => $"[{Value[0]}, {Value[1]}, {Value[2]}, {Value[3]}]";
    }
}
