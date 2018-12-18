using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.M2
{
    public class M2Light : IVersioned
    {
        public LightType Type;
        public ushort BoneIndex;
        public C3Vector Position;
        public M2Track<C3Vector> AmbColorTrack;
        public M2Track<float> AmbIntensityTrack;
        public M2Track<C3Vector> ColorTrack;
        public M2Track<float> IntensityTrack;
        public M2Track<float> AttenStartTrack;
        public M2Track<float> AttenEndTrack;
        public M2Track<bool> VisibilityTrack;

        public M2Light(BinaryReader br, uint build)
        {
            Type = br.ReadEnum<LightType>();
            BoneIndex = br.ReadUInt16();
            Position = br.ReadStruct<C3Vector>();
            AmbColorTrack = new M2Track<C3Vector>(br, build);
            AmbIntensityTrack = new M2Track<float>(br, build);
            ColorTrack = new M2Track<C3Vector>(br, build);
            IntensityTrack = new M2Track<float>(br, build);
            AttenStartTrack = new M2Track<float>(br, build);
            AttenEndTrack = new M2Track<float>(br, build);
            VisibilityTrack = new M2Track<bool>(br, build);
        }
    }

    public enum LightType : ushort
    {
        Directional = 0,
        Point = 1
    }
}
