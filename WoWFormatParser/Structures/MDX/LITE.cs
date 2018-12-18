using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.MDX
{
    public class LITE : GenObject
    {
        public uint Size;
        public LIGHT_TYPE Type;
        public float AttenuationStart;
        public float AttenuationEnd;
        public CBGR Color;
        public float Intensity;
        public CBGR AmbientColor;
        public float AmbientIntensity;
        public MDXTrack<float> AttenuationStartKeys;
        public MDXTrack<float> AttenuationEndKeys;
        public MDXTrack<CBGR> ColorKeys;
        public MDXTrack<float> IntensityKeys;
        public MDXTrack<CBGR> AmbientColorKeys;
        public MDXTrack<float> AmbientIntensityKeys;
        public MDXTrack<float> VisibilityKeys;

        public LITE(BinaryReader br)
        {
            Size = br.ReadUInt32();
            Load(br);

            Type = (LIGHT_TYPE)br.ReadInt32();
            AttenuationStart = br.ReadSingle();
            AttenuationEnd = br.ReadSingle();
            Color = br.ReadStruct<CBGR>();
            Intensity = br.ReadSingle();
            AmbientColor = br.ReadStruct<CBGR>();    // added at version 700
            AmbientIntensity = br.ReadSingle(); // added at version 700

            while (true)
            {
                string token = br.ReadString(4);
                br.BaseStream.Position -= 4;
                switch (token)
                {
                    case "KLAI": IntensityKeys = new MDXTrack<float>(br); break;
                    case "KLBI": AmbientIntensityKeys = new MDXTrack<float>(br); break;
                    case "KVIS": VisibilityKeys = new MDXTrack<float>(br); break;
                    case "KLAC": ColorKeys = new MDXTrack<CBGR>(br); break;
                    case "KLBC": AmbientColorKeys = new MDXTrack<CBGR>(br); break;
                    case "KLAS": AttenuationStartKeys = new MDXTrack<float>(br); break;
                    case "KLAE": AttenuationEndKeys = new MDXTrack<float>(br); break;
                    default: return;
                }
            }
        }
    }

    public enum LIGHT_TYPE
    {
        Omni = 0x0,
        Direct = 0x1,
        Ambient = 0x2,
    }
}
