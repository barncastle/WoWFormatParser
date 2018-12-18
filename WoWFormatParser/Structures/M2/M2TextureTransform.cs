using System.IO;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.M2
{
    public class M2TextureTransform : IVersioned
    {
        public M2Track<C3Vector> TransTrack;
        public M2Track<C4Vector> RotTrack;
        public M2Track<C3Vector> ScaleTrack;

        public M2TextureTransform(BinaryReader br, uint build)
        {
            TransTrack = new M2Track<C3Vector>(br, build);
            RotTrack = new M2Track<C4Vector>(br, build);
            ScaleTrack = new M2Track<C3Vector>(br, build);
        }
    }
}
