using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CRange : IStringDescriptor
    {
        public float low;
        public float high;

        public override string ToString() => $"Low: {low}, High: {high}";
    }
}
