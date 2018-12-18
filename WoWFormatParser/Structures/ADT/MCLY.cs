using System;
using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.ADT
{
    public class MCLY : IStringDescriptor
    {
        public uint TextureId;
        public object Props;
        public uint OffsAlpha;
        public int EffectId;

        public MCLY(BinaryReader br)
        {
            TextureId = br.ReadUInt32();
            Props = br.ReadEnum<MCLY_Flags>();
            OffsAlpha = br.ReadUInt32();
            EffectId = br.ReadInt32();
        }

        public override string ToString() => $"TextureId: {TextureId}, Props: {Props}, OffsAlpha: {OffsAlpha}, EffectId: {EffectId}";
    }

    [Flags]
    public enum MCLY_Flags : uint
    {
        None = 0,
        Animated45RotationPerTick = 0x001,
        Animated90RotationPerTick = 0x002,
        Animated180RotationPerTick = 0x004,
        AnimSpeed1 = 0x008,
        AnimSpeed2 = 0x010,
        AnimSpeed3 = 0x020,
        AnimationEnabled = 0x040,
        EmissiveLayer = 0x080,
        UseAlpha = 0x100,
        CompressedAlpha = 0x200,
        UseCubeMappedReflection = 0x400,
    }
}
