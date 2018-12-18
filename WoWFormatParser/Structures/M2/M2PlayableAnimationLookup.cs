using System;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.M2
{
    public struct M2PlayableAnimationLookup : IStringDescriptor
    {
        public short FallbackAnimId;
        public MDXPlayableAnimationFlags Flags;

        public override string ToString() => $"FallbackAnimId: {FallbackAnimId}, Flags: {Flags}";
    }

    [Flags]
    public enum MDXPlayableAnimationFlags : ushort
    {
        Standard = 0,
        Reversed = 1,
        Freeze = 3
    }
}
