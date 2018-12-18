using System;
using System.IO;
using System.Runtime.InteropServices;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.M2
{
    public class M2SkinProfile : IVersioned
    {
        public M2Array<ushort> VertexIndicies;
        public M2Array<ushort> Triangles;
        public M2Array<C4bVector> VertexProperties;
        public M2Array<M2SkinSection> Sections;
        public M2Array<M2Batch> RenderBatches;
        public uint BoneCountMax;

        public M2SkinProfile(BinaryReader br, uint build)
        {
            VertexIndicies = br.ReadM2Array<ushort>(build);
            Triangles = br.ReadM2Array<ushort>(build);
            VertexProperties = br.ReadM2Array<C4bVector>(build);
            Sections = br.ReadM2Array<M2SkinSection>(build);
            RenderBatches = br.ReadM2Array<M2Batch>(build);
            BoneCountMax = br.ReadUInt32();
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct M2SkinSection
    {
        public ushort SkinSectionId;
        public ushort Level;
        public ushort VertexStart;
        public ushort VertexCount;
        public ushort IndexStart;
        public ushort IndexCount;
        public ushort BoneCount;
        public ushort BoneComboIndex;
        public ushort BoneInfluences;
        public ushort CenterBoneIndex;
        public C3Vector CenterPosition;
    }

    public class M2Batch
    {
        public M2Batch_Flags Flags;
        public byte PriorityPlane;
        public ushort ShaderId;
        public ushort SkinSectionIndex;
        public ushort GeosetIndex;
        public ushort ColorIndex;
        public ushort MaterialIndex;
        public ushort MaterialLayer;
        public ushort TextureCount;
        public ushort TextureComboIndex;
        public ushort TextureCoordComboIndex;
        public ushort TextureWeightComboIndex;
        public ushort TextureTransformComboIndex;

        public M2Batch(BinaryReader br)
        {
            Flags = br.ReadEnum<M2Batch_Flags>();
            PriorityPlane = br.ReadByte();
            ShaderId = br.ReadUInt16();
            SkinSectionIndex = br.ReadUInt16();
            GeosetIndex = br.ReadUInt16();
            ColorIndex = br.ReadUInt16();
            MaterialIndex = br.ReadUInt16();
            MaterialLayer = br.ReadUInt16();
            TextureCount = br.ReadUInt16();
            TextureComboIndex = br.ReadUInt16();
            TextureCoordComboIndex = br.ReadUInt16();
            TextureWeightComboIndex = br.ReadUInt16();
            TextureTransformComboIndex = br.ReadUInt16();
        }
    }

    [Flags]
    public enum M2Batch_Flags : byte
    {
        Animated = 0x0,
        Invert = 0x1,
        Transform = 0x2,
        Projected = 0x4,
        Unknown_0x8 = 0x8,
        Static = 0x10,
        Projected2 = 0x20,
        Weighted = 0x40
    }
}
