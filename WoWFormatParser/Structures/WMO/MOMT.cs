using System;
using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.WMO
{
    public class MOMT
    {
        public uint? Version;
        public MOMT_Flags Flags;
        public ShaderTypes Shader;
        public BlendMode BlendMode;
        public uint DiffuseNameIndex;
        public Common.CImVector SidnColor;
        public Common.CImVector FrameSidnColor;
        public uint EnvNameIndex;
        public Common.CImVector DiffColor;
        public uint GroundType;
        public uint? UnknownTexture;
        public Common.CImVector? UnknownColor;
        public MOMT_Flags? UnknownFlags;

        public MOMT(BinaryReader br, uint version)
        {
            if (version == 14)
                Version = br.ReadUInt32();
            Flags = br.ReadEnum<MOMT_Flags>();
            if (version != 14)
                Shader = br.ReadEnum<ShaderTypes>();
            BlendMode = br.ReadEnum<BlendMode>();
            DiffuseNameIndex = br.ReadUInt32();
            SidnColor = br.ReadStruct<Common.CImVector>();
            FrameSidnColor = br.ReadStruct<Common.CImVector>();
            EnvNameIndex = br.ReadUInt32();
            DiffColor = br.ReadStruct<Common.CImVector>();
            GroundType = br.ReadUInt32();

            if (version == 14)
            {
                br.BaseStream.Position += 8;
            }
            else
            {
                UnknownTexture = br.ReadUInt32();
                UnknownColor = br.ReadStruct<Common.CImVector>();
                UnknownFlags = br.ReadEnum<MOMT_Flags>();
                br.BaseStream.Position += 16;
            }
        }

        public static int GetSize(uint version) => version == 14 ? 44 : 64;
    }

    [Flags]
    public enum MOMT_Flags : uint
    {
        None = 0,
        Unlit = 0x1,
        Unfogged = 0x2,
        Unculled = 0x4,
        ExteriorLit = 0x8,
        SelfIlluminatedDayNight = 0x10,
        Window = 0x20,
        ClampSAddress = 0x40,
        ClampTAddress = 0x80,
        Unknown_0x100 = 0x100
    }

    public enum BlendMode : uint
    {
        Opaque = 0x0,
        AlphaKey = 0x1,
        Alpha = 0x2,
        Add = 0x3,
        Mod = 0x4,
        Mod2x = 0x5,
        ModAdd = 0x6,
        InvSrcAlphaAdd = 0x7,
        InvSrcAlphaOpaque = 0x8,
        SrcAlphaOpaque = 0x9,
        NoAlphaAdd = 0xA,
        ConstantAlpha = 0xB,
        Screen = 0xC,
        BlendAdd = 0xD,
    }

    public enum ShaderTypes : uint
    {
        Diffuse = 0,
        Specular = 1,
        Metal = 2,
        Env = 3,
        Opaque = 4,
        EnvMetal = 5,
        TwoLayerDiffuse = 6,
        TwoLayerEnvMetal = 7,
        TwoLayerTerrain = 8,
        DiffuseEmissive = 9,
        Unknown_0xA = 0xA,
        MaskedEnvMetal = 0xB,
        EnvMetalEmissive = 0xC,
        TwoLayerDiffuseOpaque = 0xD,
        TwoLayerDiffuseEmissive = 0xE,
        Unknown_0xF = 0xF,
        DiffuseTerrain = 0x10,
    }

}
