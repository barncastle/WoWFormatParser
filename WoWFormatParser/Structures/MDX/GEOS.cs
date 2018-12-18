using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.MDX
{
    public class GEOS : IGEOS
    {
        public uint Size;
        public C3Vector[] Vertices;
        public C3Vector[] Normals;
        public C2Vector[] TextureCoords;
        public byte[] PrimitiveTypes;
        public int[] PrimitiveGroups;
        public CVertex[] PrimitiveVertices;
        public byte[] VertexGroupIndices;
        public int[] GroupMatrixCounts;
        public int[] Matrices;
        public int[] BoneIndices;
        public int[] BoneWeights;
        public int MaterialId;
        public int SelectionGroup;
        public CExtent Bounds;
        public bool Unselectable;
        public CExtent[] SequenceBounds;

        public GEOS(BinaryReader br)
        {
            Size = br.ReadUInt32();

            if (br.HasTag("VRTX"))
                Vertices = br.ReadStructArray<C3Vector>(br.ReadInt32());

            if (br.HasTag("NRMS"))
                Normals = br.ReadStructArray<C3Vector>(br.ReadInt32());

            if (br.HasTag("UVAS"))
                TextureCoords = br.ReadStructArray<C2Vector>(br.ReadInt32() * Normals.Length);

            if (br.HasTag("PTYP"))
                PrimitiveTypes = br.ReadBytes(br.ReadInt32());

            if (br.HasTag("PCNT"))
                PrimitiveGroups = br.ReadStructArray<int>(br.ReadInt32());

            if (br.HasTag("PVTX"))
                PrimitiveVertices = br.ReadStructArray<CVertex>(br.ReadInt32() / 3);

            if (br.HasTag("GNDX"))
                VertexGroupIndices = br.ReadBytes(br.ReadInt32());

            if (br.HasTag("MTGC"))
                GroupMatrixCounts = br.ReadStructArray<int>(br.ReadInt32());

            if (br.HasTag("MATS"))
                Matrices = br.ReadStructArray<int>(br.ReadInt32());

            if (br.HasTag("BIDX"))
                BoneIndices = br.ReadStructArray<int>(br.ReadInt32());

            if (br.HasTag("BWGT"))
                BoneWeights = br.ReadStructArray<int>(br.ReadInt32());

            MaterialId = br.ReadInt32();
            SelectionGroup = br.ReadInt32();
            Unselectable = br.ReadUInt32() == 1;
            Bounds = br.ReadStruct<CExtent>();
            SequenceBounds = br.ReadStructArray<CExtent>(br.ReadInt32());
        }
    }
}
