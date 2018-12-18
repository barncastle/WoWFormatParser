using System;
using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.MDX
{
    public class MTLS
    {
        public uint Size;
        public int PriorityPlane;
        public int LayerCount;
        public TexLayer[] TexLayers;

        public MTLS(BinaryReader br)
        {
            Size = br.ReadUInt32();
            PriorityPlane = br.ReadInt32();
            LayerCount = br.ReadInt32();
            TexLayers = br.ReadArray(LayerCount, () => new TexLayer(br));
        }
    }

    public class TexLayer
    {
        public uint Size;
        public MDLTexOp BlendMode;
        public MDLGeo Flags;
        public int TextureId;
        public int TextureAnimationId;
        public int CoordId;
        public float Alpha;
        public MDXSimpleTrack FlipKeys;
        public MDXTrack<float> AlphaKeys;

        public TexLayer(BinaryReader br)
        {
            Size = br.ReadUInt32();

            BlendMode = (MDLTexOp)br.ReadInt32();
            Flags = (MDLGeo)br.ReadUInt32();
            TextureId = br.ReadInt32();
            TextureAnimationId = br.ReadInt32();
            CoordId = br.ReadInt32();
            Alpha = br.ReadSingle();

            while (true)
            {
                string token = br.ReadString(4);
                br.BaseStream.Position -= 4;
                switch (token)
                {
                    case "KMTA": AlphaKeys = new MDXTrack<float>(br); break;
                    case "KMTF": FlipKeys = new MDXSimpleTrack(br); break;
                    default: return;
                }
            }
        }

    }

    public enum MDLTexOp
    {
        Load = 0x0,
        Transparent = 0x1,
        Blend = 0x2,
        Add = 0x3,
        AddAlpha = 0x4,
        Modulate = 0x5,
        Modulate2X = 0x6,
    }

    [Flags]
    public enum MDLGeo
    {
        Unshaded = 0x1,
        SphereEnvMap = 0x2,
        WrapWidth = 0x4,
        WrapHeight = 0x8,
        TwoSided = 0x10,
        Unfogged = 0x20,
        NoDepthTest = 0x40,
        NoDepthSet = 0x80,
        Unknown_0x100 = 0x100, // ShaderSkin? waterfalls only
    }
}
