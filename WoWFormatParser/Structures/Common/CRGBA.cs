using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.Common
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CRGBA : IStringDescriptor
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;

        public override string ToString() => $"R: {r}, G: {g}, B: {b}, A: {a}";
    }
}
