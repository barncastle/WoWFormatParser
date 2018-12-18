using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.M2
{
    public class M2Ribbon : IVersioned
    {
        public int RibbonId;
        public uint BoneIndex;
        public C3Vector Position;
        public M2Array<short> TextureLookup;
        public M2Array<short> MaterialLookup;
        public M2Track<CRGB> ColorTrack;
        public M2Track<FixedPoint_0_15> AlphaTrack;
        public M2Track<float> HeightAboveTrack;
        public M2Track<float> HeightBelowTrack;
        public float EdgesPerSecond;
        public float EdgeLifetime;
        public float Gravity;
        public ushort TextureRows;
        public ushort TextureCols;
        public M2Track<ushort> TextureSlotTrack;
        public M2Track<bool> VisbilityTrack;

        public M2Ribbon(BinaryReader br, uint build)
        {
            RibbonId = br.ReadInt32();
            BoneIndex = br.ReadUInt32();
            Position = br.ReadStruct<C3Vector>();
            TextureLookup = br.ReadM2Array<short>(build);
            MaterialLookup = br.ReadM2Array<short>(build);
            ColorTrack = new M2Track<CRGB>(br, build);
            AlphaTrack = new M2Track<FixedPoint_0_15>(br, build);
            HeightAboveTrack = new M2Track<float>(br, build);
            HeightBelowTrack = new M2Track<float>(br, build);
            EdgesPerSecond = br.ReadSingle();
            EdgeLifetime = br.ReadSingle();
            Gravity = br.ReadSingle();
            TextureRows = br.ReadUInt16();
            TextureCols = br.ReadUInt16();
            TextureSlotTrack = new M2Track<ushort>(br, build);
            VisbilityTrack = new M2Track<bool>(br, build);
        }
    }
}
