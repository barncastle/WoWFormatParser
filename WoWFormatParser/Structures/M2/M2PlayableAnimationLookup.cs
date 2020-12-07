using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.M2
{
    public struct M2PlayableAnimationLookup : IStringDescriptor
    {
        public short FallbackAnimId;
        public M2PlayableAnimation_Flags Flags;

        public override string ToString() => $"FallbackAnimId: {FallbackAnimId}, Flags: {Flags}";
    }

    public enum M2PlayableAnimation_Flags : ushort
    {
        Standard = 0,
        Reversed = 1,
        Unknown_0x2 = 2,
        Freeze = 3
    }
}
