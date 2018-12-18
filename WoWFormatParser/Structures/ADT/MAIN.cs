using System;
using System.Runtime.InteropServices;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.ADT
{
    [StructLayout(LayoutKind.Sequential)]
    public struct MAIN : IStringDescriptor
    {
        public uint Offset;
        public uint Size;
        public MAIN_Flags Flags;
#pragma warning disable 0169
        private readonly uint AsyncId;
#pragma warning restore 0169

        public override string ToString() => $"Offset: {Offset}, Size: {Size}, Flags: {Flags}";
    }

    [Flags]
    public enum MAIN_Flags : uint
    {
        None = 0,
        HasADT = 1,
        Loaded = 2,
    }
}
