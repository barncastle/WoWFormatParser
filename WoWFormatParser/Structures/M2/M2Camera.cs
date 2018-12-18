using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.M2
{
    public class M2Camera : IVersioned
    {
        public CameraType Type;
        public float FieldOfView;
        public float FarClip;
        public float NearClip;
        public M2Track<SplineKey<C3Vector>> TransTrack;
        public C3Vector Pivot;
        public M2Track<SplineKey<C3Vector>> TargetTransTrack;
        public C3Vector TargetPivot;
        public M2Track<SplineKey<float>> RollTrack;

        public M2Camera(BinaryReader br, uint build)
        {
            Type = br.ReadEnum<CameraType>();
            FieldOfView = br.ReadSingle();
            FarClip = br.ReadSingle();
            NearClip = br.ReadSingle();
            TransTrack = new M2Track<SplineKey<C3Vector>>(br, build);
            Pivot = br.ReadStruct<C3Vector>();
            TargetTransTrack = new M2Track<SplineKey<C3Vector>>(br, build);
            TargetPivot = br.ReadStruct<C3Vector>();
            RollTrack = new M2Track<SplineKey<float>>(br, build);
        }
    }

    public enum CameraType : int
    {
        Other = -1,
        Portrait = 0,
        CharacterInfo = 1
    }
}
