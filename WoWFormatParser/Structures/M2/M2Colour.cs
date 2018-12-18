using System.IO;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.M2
{
    public class M2Color : IVersioned
    {
        public M2Track<CRGB> ColorTrack;
        public M2Track<FixedPoint_0_15> AlphaTrack;

        public M2Color(BinaryReader br, uint build)
        {
            ColorTrack = new M2Track<CRGB>(br, build);
            AlphaTrack = new M2Track<FixedPoint_0_15>(br, build);
        }
    }
}
