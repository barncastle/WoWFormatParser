using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.MDX
{
    public class GEOA
    {
        public int Size;
        public int GeosetId;
        public float Alpha;
        public CBGR Color;
        public bool HasColorKeys;
        public MDXTrack<float> AlphaKeys;
        public MDXTrack<CBGR> ColorKeys;

        public GEOA(BinaryReader br)
        {
            Size = br.ReadInt32();
            GeosetId = br.ReadInt32();
            Alpha = br.ReadSingle();
            Color = br.ReadStruct<CBGR>();
            HasColorKeys = br.ReadInt32() == 1;

            while (true)
            {
                string token = br.ReadString(4);
                br.BaseStream.Position -= 4;
                switch (token)
                {
                    case "KGAO": AlphaKeys = new MDXTrack<float>(br); break;
                    case "KGAC": ColorKeys = new MDXTrack<CBGR>(br); break;
                    default: return;
                }
            }
        }
    }
}
