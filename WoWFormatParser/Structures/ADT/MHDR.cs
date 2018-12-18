using System.Runtime.InteropServices;

namespace WoWFormatParser.Structures.ADT
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MHDR
    {
        public uint OffsInfo;    // MCIN
        public uint OffsTex;     // MTEX
        public uint SizeTex;
        public uint OffsDoo;     // MDDF
        public uint SizeDoo;
        public uint OffsMob;     // MODF
        public uint SizeMob;

#pragma warning disable 0169
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 36)]
        private byte[] pad;
#pragma warning restore 0169
    }
}
