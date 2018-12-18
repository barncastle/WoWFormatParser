using System;
using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.M2
{
    public class M2Bone : IVersioned
    {
        public KeyBone KeyBoneId;
        public M2Bone_Flags Flags;
        public short Parent;
        public short SubmeshId;
        public M2Track<C3Vector> TransTrack;
        public M2Track<C4Quaternion> RotTrack;
        public M2Track<C3Vector> ScaleTrack;
        public C3Vector Pivot;

        public M2Bone(BinaryReader br, uint build)
        {
            KeyBoneId = (KeyBone)br.ReadInt32();
            Flags = br.ReadEnum<M2Bone_Flags>();
            Parent = br.ReadInt16();
            SubmeshId = br.ReadInt16();
            TransTrack = new M2Track<C3Vector>(br, build);
            RotTrack = new M2Track<C4Quaternion>(br, build);
            ScaleTrack = new M2Track<C3Vector>(br, build);
            Pivot = br.ReadStruct<C3Vector>();
        }
    }

    [Flags]
    public enum M2Bone_Flags : uint
    {
        Unknown_0x1 = 0x1,
        Unknown_0x2 = 0x2,
        Unknown_0x4 = 0x4,
        SphericalBillboard = 0x8,
        CylindricalBillboardLockedX = 0x10,
        CylindricalBillboardLockedY = 0x20,
        CylindricalBillboardLockedZ = 0x40,
        Unknown_0x80 = 0x80,
        Unknown_0x100 = 0x100,
        Transformed = 0x200
    }

    public enum KeyBone : short
    {
        Other = -1,
        ArmL = 0,
        ArmR,
        ShoulderL,
        ShoulderR,
        SpineLow,
        Waist,
        Head,
        Jaw,
        IndexFingerR,
        MiddleFingerR,
        PinkyFingerR,
        RingFingerR,
        ThumbR,
        IndexFingerL,
        MiddleFingerL,
        PinkyFingerL,
        RingFingerL,
        ThumbL,
        Bth,
        Csr,
        Csl,
        Breath,
        Name,
        NameMount,
        Chd,
        Cch,
        Root,
        Wheel1,
        Wheel2,
        Wheel3,
        Wheel4,
        Wheel5,
        Wheel6,
        Wheel7,
        Wheel8
    }
}
