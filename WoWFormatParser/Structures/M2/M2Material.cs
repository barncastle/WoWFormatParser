using System;
using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.M2
{
    public class M2Material : IVersioned
    {
        public RenderFlags Flags;
        public BlendingMode BlendMode;

        public M2Material(BinaryReader br, uint build)
        {
            Flags = br.ReadEnum<RenderFlags>();
            BlendMode = br.ReadEnum<BlendingMode>();
        }
    }

    [Flags]
    public enum RenderFlags : ushort
    {
        None = 0,
        Unlit = 0x1,
        NoFog = 0x2,
        TwoSided = 0x4,
        Unknown = 0x8,
        DisableZBuffering = 0x10
    }

    public enum BlendingMode : ushort
    {
        Opaque = 0,
        AlphaKey = 1,
        Alpha = 2,
        Additive = 3,
        Modulate = 4,
        Modulate2x = 5,
        ModulateAdditive = 6,
        InvertedSourceAlphaAdditive = 7,
        InvertedSourceAlphaOpaque = 8,
        SourceAlphaOpaque = 9,
        NoAlphaAdditive = 10,
        ConstantAlpha = 11,
        Screen = 12,
        BlendAdditive = 13
    }
}
