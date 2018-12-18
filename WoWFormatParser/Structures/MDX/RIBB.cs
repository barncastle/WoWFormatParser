using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.MDX
{
    public class RIBB : GenObject
    {
        public uint Size;
        public uint EmitterSize;
        public float HeightAbove;
        public float HeightBelow;
        public float Alpha;
        public CBGR Color;
        public float EdgeLifetime;
        public uint TextureSlot;
        public uint EdgesPerSecond;
        public uint TextureRows;
        public uint TextureColumns;
        public uint MaterialId;
        public float Gravity;

        public MDXTrack<float> HeightAboveKeys;
        public MDXTrack<float> HeightBelowKeys;
        public MDXTrack<float> AlphaKeys;
        public MDXTrack<float> VisibilityKeys;
        public MDXTrack<CBGR> ColorKeys;
        public MDXSimpleTrack TextureSlotKeys;

        public RIBB(BinaryReader br)
        {
            Size = br.ReadUInt32();
            Load(br);

            EmitterSize = br.ReadUInt32();
            HeightAbove = br.ReadSingle();
            HeightBelow = br.ReadSingle();
            Alpha = br.ReadSingle();
            Color = br.ReadStruct<CBGR>();
            EdgeLifetime = br.ReadSingle();
            TextureSlot = br.ReadUInt32();
            EdgesPerSecond = br.ReadUInt32();
            TextureRows = br.ReadUInt32();
            TextureColumns = br.ReadUInt32();
            MaterialId = br.ReadUInt32();
            Gravity = br.ReadSingle();

            while (true)
            {
                string token = br.ReadString(4);
                br.BaseStream.Position -= 4;
                switch (token)
                {
                    case "KRHA": HeightAboveKeys = new MDXTrack<float>(br); break;
                    case "KRHB": HeightBelowKeys = new MDXTrack<float>(br); break;
                    case "KRAL": AlphaKeys = new MDXTrack<float>(br); break;
                    case "KVIS": VisibilityKeys = new MDXTrack<float>(br); break;
                    case "KRCO": ColorKeys = new MDXTrack<CBGR>(br); break;
                    case "KRTX": TextureSlotKeys = new MDXSimpleTrack(br); break;
                    default: return;
                }
            }
        }
    }
}
