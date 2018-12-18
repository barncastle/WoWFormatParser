using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.MDX
{
    public class CAMS
    {
        public uint Size;
        public string Name;
        public C3Vector Pivot;
        public float FieldOfView;
        public float FarClip;
        public float NearClip;
        public C3Vector TargetPivot;
        public MDXTrack<C3Vector> TranslationKeys;
        public MDXTrack<C3Vector> TargetTranslationKeys;
        public MDXTrack<float> RotationKeys;
        public MDXTrack<float> VisibilityKeys;

        public CAMS(BinaryReader br)
        {
            Size = br.ReadUInt32();
            Name = br.ReadString(80).TrimEnd('\0');
            Pivot = br.ReadStruct<C3Vector>();
            FieldOfView = br.ReadSingle();
            FarClip = br.ReadSingle();
            NearClip = br.ReadSingle();
            TargetPivot = br.ReadStruct<C3Vector>();

            while (true)
            {
                string token = br.ReadString(4);
                br.BaseStream.Position -= 4;
                switch (token)
                {
                    case "KCTR": TranslationKeys = new MDXTrack<C3Vector>(br); break;
                    case "KCRL": RotationKeys = new MDXTrack<float>(br); break;
                    case "KTTR": TargetTranslationKeys = new MDXTrack<C3Vector>(br); break;
                    case "KVIS": VisibilityKeys = new MDXTrack<float>(br); break;
                    default: return;
                }
            }
        }
    }
}
