using System.Runtime.InteropServices;

namespace WoWFormatParser.Structures.WMO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MOLD
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32768)]
        public byte[] Texels;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        private readonly byte[] Pad;
    }
}
