using System;
using System.Collections.Generic;
using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.WMO
{
    public sealed class WMOGroup : Format, IVersioned
    {
        public uint Version;
        public uint GroupName;
        public uint DbgName;
        public MOGP_Flags Flags;
        public CAaBox BoundingBox;
        public int PortalStart;
        public uint PortalCount;
        public ushort? TransBatchCount;
        public ushort? IntBatchCount;
        public ushort? ExtBatchCount;
        public ushort? Padding;
        public byte[] FogIds;
        public uint GroupLiquid;
        public SMOGxBatch[] IntBatch; // 4
        public SMOGxBatch[] ExtBatch; // 4
        public int UniqueId;
        public uint? Flags2;
        public uint? Unknown_0x40;

        public MOPY[] Polygons;
        public ushort[] VertexIndices;
        public C3Vector[] Vertices;
        public C3Vector[] Normals;
        public C2Vector[] TextureVertices;
        public C2Vector[] LightMapVertices;
        public MOBA[] Batches;
        public ushort[] LightReferences;
        public ushort[] DoodadReferences;
        public MOBN[] BSPNodes;
        public ushort[] BSPFaceIndicies;
        public CImVector[] VertexColors;
        public IReadOnlyList<MLIQ> Liquids;
        public ushort[] TriangleStripIndices;
        public MOLM[] LightMaps;
        public MOLD[] LightMapTexels;
        public MPBX[] MPBV;
        public MPBX[] MPBP;
        public ushort[] MPBI;
        public C3Vector[] MPBG;

        public WMOGroup(BinaryReader br, uint build)
        {
            List<MLIQ> _Liquids = new List<MLIQ>();

            string prevToken = "";

            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                var (Token, Size) = br.ReadIffChunk(true);
                if (Size <= 0)
                    continue;

                switch (Token)
                {
                    case "MVER":
                        Version = br.ReadUInt32();
                        break;
                    case "MOGP":
                        Version = Version == 0 ? GetVersion(build) : Version;
                        ReadHeader(br);
                        continue;
                    case "MOPY":
                        Polygons = br.ReadArray(Size / MOPY.GetSize(Version), () => new MOPY(br, Version));
                        break;
                    case "MOVI":
                    case "MOIN":
                        VertexIndices = br.ReadStructArray<ushort>(Size / 2);
                        break;
                    case "MOVT":
                        Vertices = br.ReadStructArray<C3Vector>(Size / 12);
                        break;
                    case "MONR":
                        Normals = br.ReadStructArray<C3Vector>(Size / 12);
                        break;
                    case "MOTV":
                        TextureVertices = br.ReadStructArray<C2Vector>(Size / 8);
                        break;
                    case "MOLV":
                        LightMapVertices = br.ReadStructArray<C2Vector>(Size / 8);
                        break;
                    case "MOBA":
                        Batches = br.ReadArray(Size / MOBA.GetSize(Version), () => new MOBA(br, Version));
                        break;
                    case "MOLR":
                        LightReferences = br.ReadStructArray<ushort>(Size / 2);
                        break;
                    case "MODR":
                        DoodadReferences = br.ReadStructArray<ushort>(Size / 2);
                        break;
                    case "MOBN":
                        BSPNodes = br.ReadArray(Size / 16, () => new MOBN(br));
                        break;
                    case "MOBR":
                        BSPFaceIndicies = br.ReadStructArray<ushort>(Size / 2);
                        break;
                    case "MOCV":
                        VertexColors = br.ReadStructArray<CImVector>(Size / 4);
                        break;
                    case "MLIQ":
                        _Liquids.Add(new MLIQ(br));
                        break;
                    case "MORI":
                        TriangleStripIndices = br.ReadStructArray<ushort>(Size / 2);
                        break;
                    case "MOLM":
                        LightMaps = br.ReadStructArray<MOLM>(Size / 4);
                        break;
                    case "MOLD":
                        LightMapTexels = br.ReadStructArray<MOLD>(Size / 0x8004);
                        break;
                    case "MPBV":
                        MPBV = br.ReadStructArray<MPBX>(Size / 4);
                        break;
                    case "MPBP":
                        MPBP = br.ReadStructArray<MPBX>(Size / 4);
                        break;
                    case "MPBI":
                        MPBI = br.ReadStructArray<ushort>(Size / 2);
                        break;
                    case "MPBG":
                        MPBG = br.ReadStructArray<C3Vector>(Size / 12);
                        break;
                    default:
                        throw new NotImplementedException("Unknown token " + Token);
                }

                prevToken = Token;
            }

            if (br.BaseStream.Position != br.BaseStream.Length)
                throw new UnreadContentException();

            if (_Liquids.Count > 0)
                Liquids = _Liquids;
        }

        private void ReadHeader(BinaryReader br)
        {
            GroupName = br.ReadUInt32();
            DbgName = br.ReadUInt32();
            Flags = br.ReadEnum<MOGP_Flags>();
            BoundingBox = br.ReadStruct<CAaBox>();
            PortalStart = Version == 14 ? br.ReadInt32() : br.ReadInt16();
            PortalCount = Version == 14 ? br.ReadUInt32() : br.ReadUInt16();

            if (Version != 14)
            {
                TransBatchCount = br.ReadUInt16();
                IntBatchCount = br.ReadUInt16();
                ExtBatchCount = br.ReadUInt16();
                Padding = br.ReadUInt16();
                if (Padding.Value != 0)
                    throw new Exception("NOT PADDING");
            }

            FogIds = br.ReadBytes(4);
            GroupLiquid = br.ReadUInt32();

            if (Version == 14)
            {
                IntBatch = br.ReadStructArray<SMOGxBatch>(4);
                ExtBatch = br.ReadStructArray<SMOGxBatch>(4);
            }

            UniqueId = br.ReadInt32();

            if (Version != 14)
            {
                Flags2 = br.ReadUInt32();
                Unknown_0x40 = br.ReadUInt32();

                if (Flags2 > 0 || Unknown_0x40 > 0)
                    throw new Exception("NOT PADDING");
            }
            else
            {
                br.BaseStream.Position += 8; // pad
            }
        }

        private uint GetVersion(uint build)
        {
            if (build < 3592)
                return 14;
            if (build < 3980)
                return 16;
            return 17;
        }
    }

    public struct SMOGxBatch
    {
        public ushort VertStart;
        public ushort VertCount;
        public ushort BatchStart;
        public ushort BatchCount;
    }

    [Flags]
    public enum MOGP_Flags : uint
    {
        None = 0,
        HasBSP = 0x1,
        HasLightmap = 0x2,
        HasVertexColors = 0x4,
        IsExterior = 0x8,
        Unknown_0x10 = 0x10,
        Unknown_0x20 = 0x20,
        IsExteriorLit = 0x40,
        Unreachable = 0x80,
        Unknown_0x100 = 0x100,
        HasLights = 0x200,
        HasMPBX = 0x400,
        HasDoodads = 0x800,
        HasLiquids = 0x1000,
        IsInterior = 0x2000,
        Unknown_0x4000 = 0x4000,
        Unknown_0x8000 = 0x8000,
        AlwaysDraw = 0x10000,
        HasTriangleStrips = 0x20000,
        ShowSkybox = 0x40000,
        IsOceanicWater = 0x80000,
        Unknown_0x100000 = 0x100000,
        IsMountAllowed = 0x200000,
        Unknown_0x400000 = 0x400000,
        Unknown_0x800000 = 0x800000,
        HasTwoVertexShadingSets = 0x1000000,
        HasTwoTextureCoordinateSets = 0x2000000,
        IsAntiportal = 0x4000000,
        Unknown_0x8000000 = 0x8000000,
        Unknown_0x10000000 = 0x10000000,
        ExteriorCull = 0x20000000,
        HasThreeTextureCoordinateSets = 0x40000000,
        Unknown_0x80000000 = 0x80000000
    }
}
