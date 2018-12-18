using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.M2
{
    [StructLayout(LayoutKind.Sequential)]
    public struct M2Vertex
    {
        public C3Vector Position;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] BoneWeights;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] BoneIndicies;
        public C3Vector Normal;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public C3Vector[] TexCoords;
    }
}
