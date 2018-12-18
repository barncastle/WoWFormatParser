using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.WMO
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MOPT
    {
        public ushort StartVertex;
        public ushort Count;
        public C4Plane Plane;
    }
}
