using System;
using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.M2
{
    public sealed class M2 : Format
    {
        public string Magic;
        public uint Version;
        public string InternalName;
        public GlobalFlags GlobalFlags;
        public M2Array<uint> GlobalSequenceTimestamps;
        public M2Array<M2Sequence> Sequences;
        public M2Array<short> SequenceLookup;
        public M2Array<M2PlayableAnimationLookup> PlayableAnimationLookup;
        public M2Array<M2Bone> Bones;
        public M2Array<KeyBone> KeyBoneLookup;
        public M2Array<M2Vertex> Vertices;
        public M2Array<M2SkinProfile> SkinProfiles;
        public M2Array<M2Color> Colors;
        public M2Array<M2Texture> Textures;
        public M2Array<FixedPoint_0_15> TextureWeights;
        public M2Array<byte> TextureFlipbook;
        public M2Array<M2TextureTransform> TextureTransforms;
        public M2Array<short> TextureReplacementLookup;
        public M2Array<M2Material> Materials;
        public M2Array<short> BoneLookup;
        public M2Array<short> TextureLookup;
        public M2Array<short> TexUnitLookup;
        public M2Array<short> TransparencyLookup;
        public M2Array<short> TexTransformsLookup;
        public CExtent BoundingBox;
        public CExtent CollisionBox;
        public M2Array<ushort> CollisionTriangles;
        public M2Array<C3Vector> CollisionVertices;
        public M2Array<C3Vector> CollisionNormals;
        public M2Array<M2Attachment> Attachments;
        public M2Array<ushort> AttachmentLookupTable;
        public M2Array<M2Event> Events;
        public M2Array<M2Light> Lights;
        public M2Array<M2Camera> Cameras;
        public M2Array<short> CameraLookupTable;
        public M2Array<M2Ribbon> Ribbons;
        public M2Array<M2Particle> Particles;

        public M2(BinaryReader br)
        {
            Magic = br.ReadString(4);
            Version = br.ReadUInt32();

            InternalName = br.ReadM2Array<byte>(Version)?.AsString();
            GlobalFlags = br.ReadEnum<GlobalFlags>();
            GlobalSequenceTimestamps = br.ReadM2Array<uint>(Version);
            Sequences = br.ReadM2Array<M2Sequence>(Version);
            SequenceLookup = br.ReadM2Array<short>(Version);
            PlayableAnimationLookup = br.ReadM2Array<M2PlayableAnimationLookup>(Version);
            Bones = br.ReadM2Array<M2Bone>(Version);
            KeyBoneLookup = br.ReadM2Array<KeyBone>(Version);
            Vertices = br.ReadM2Array<M2Vertex>(Version);
            SkinProfiles = br.ReadM2Array<M2SkinProfile>(Version);
            Colors = br.ReadM2Array<M2Color>(Version);
            Textures = br.ReadM2Array<M2Texture>(Version);
            TextureWeights = br.ReadM2Array<FixedPoint_0_15>(Version);
            TextureFlipbook = br.ReadM2Array<byte>(Version);
            TextureTransforms = br.ReadM2Array<M2TextureTransform>(Version);
            TextureReplacementLookup = br.ReadM2Array<short>(Version);
            Materials = br.ReadM2Array<M2Material>(Version);
            BoneLookup = br.ReadM2Array<short>(Version);
            TextureLookup = br.ReadM2Array<short>(Version);
            TexUnitLookup = br.ReadM2Array<short>(Version);
            TransparencyLookup = br.ReadM2Array<short>(Version);
            TexTransformsLookup = br.ReadM2Array<short>(Version);
            BoundingBox = br.ReadStruct<CExtent>();
            CollisionBox = br.ReadStruct<CExtent>();
            CollisionTriangles = br.ReadM2Array<ushort>(Version);
            CollisionVertices = br.ReadM2Array<C3Vector>(Version);
            CollisionNormals = br.ReadM2Array<C3Vector>(Version);
            Attachments = br.ReadM2Array<M2Attachment>(Version);
            AttachmentLookupTable = br.ReadM2Array<ushort>(Version);
            Events = br.ReadM2Array<M2Event>(Version);
            Lights = br.ReadM2Array<M2Light>(Version);
            Cameras = br.ReadM2Array<M2Camera>(Version);
            CameraLookupTable = br.ReadM2Array<short>(Version);
            Ribbons = br.ReadM2Array<M2Ribbon>(Version);
            Particles = br.ReadM2Array<M2Particle>(Version);

            if (TextureFlipbook != null)
                throw new Exception("TextureFlipbook has data");
        }
    }

    [Flags]
    public enum GlobalFlags : uint
    {
        None = 0,
        TiltX = 0x1,
        TiltY = 0x2,
        Unknown_0x4 = 0x4,
        HasBlendModeOverrides = 0x8,
    }

}
