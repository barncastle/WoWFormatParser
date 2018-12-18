using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.MDX
{
    public interface IGEOS
    {
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct CVertex : IStringDescriptor
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public ushort[] Vertices;

        public override string ToString() => $"[{Vertices[0]}, {Vertices[1]}, {Vertices[2]}]";
    }
}
