using System;
using System.IO;
using System.Runtime.InteropServices;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.MDX
{
    public class GEOS_1500 : IGEOS
    {
        public int SectionCount;
        public MDLGEOSECTION[] Sections;
        public MDLPRIMITIVE[] Primitives;

        public GEOS_1500(BinaryReader br)
        {
            SectionCount = br.ReadInt32();
            Sections = br.ReadArray(SectionCount, () => new MDLGEOSECTION(br));

            if (SectionCount > 0)
            {
                Primitives = new MDLPRIMITIVE[SectionCount];
                for (int i = 0; i < SectionCount; i++)
                    Primitives[i] = new MDLPRIMITIVE(br, Sections[i].NumVertices);
            }
        }
    }

    public class MDLGEOSECTION
    {
        public int MaterialId;
        public C3Vector CenterBounds;
        public float BoundsRadius;
        public int SelectionGroup;
        public int GeosetIndex;
        public MDLGEOSECTIONFLAGS Flags;
        public int NumVertices;
        public int NumPrimitiveTypes;
        public int NumPrimitiveIndices;
        public int Unknown_0x28;
        public int Unknown_0x2C;

        public MDLGEOSECTION(BinaryReader br)
        {
            MaterialId = br.ReadInt32();
            CenterBounds = br.ReadStruct<C3Vector>();
            BoundsRadius = br.ReadSingle();
            SelectionGroup = br.ReadInt32();
            GeosetIndex = br.ReadInt32();

            // TODO MDLGEOSECTIONFLAGS
            Flags = (MDLGEOSECTIONFLAGS)br.ReadInt32();

            br.AssertTag("PVTX");
            NumVertices = br.ReadInt32(); // count of M2Vertex
            br.AssertTag("PTYP");
            NumPrimitiveTypes = br.ReadInt32();
            br.AssertTag("PVTX");
            NumPrimitiveIndices = br.ReadInt32();

            Unknown_0x28 = br.ReadInt32();
            Unknown_0x2C = br.ReadInt32();
        }
    }

    public class MDLPRIMITIVE
    {
        public MDLVERTEX[] Vertices;
        public int PrimitiveType;
        public int NumPrimitiveIndices;
        public int MaxPrimitiveVertex;
        public CVertex[] PrimitiveVertices;

        public MDLPRIMITIVE(BinaryReader br, int numVertices)
        {
            Vertices = br.ReadStructArray<MDLVERTEX>(numVertices);

            PrimitiveType = br.ReadInt32();  // 0x3 = Triangle, 
            br.ReadInt32();

            NumPrimitiveIndices = br.ReadInt32(); // matches MDLGEOSECTION
            MaxPrimitiveVertex = br.ReadInt32();
            PrimitiveVertices = br.ReadStructArray<CVertex>(NumPrimitiveIndices / 3);

            // alignment
            if (NumPrimitiveIndices % 8 != 0)
            {
                int count = 8 - (NumPrimitiveIndices % 8);
                br.BaseStream.Position += count * 2;
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MDLVERTEX
    {
        public C3Vector Position;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] BoneWeights;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] BoneIndices;
        public C3Vector Normal;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public C2Vector[] TexCoords;
    }

    [Flags]
    public enum MDLGEOSECTIONFLAGS : uint
    {
        Unselectable = 1,
        Unused = 2,
        Unknown_0x4 = 4,
        Unknown_0x8 = 8,
        Unknown_0x10 = 16,
        Project2d = 32,
        ShaderSkin = 64,
    }
}
