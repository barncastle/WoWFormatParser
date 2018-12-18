using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.ADT
{
    public class MCNK : IVersioned
    {
        public MCNK_Flags Flags;               // See SMChunkFlags
        public uint IndexX;
        public uint IndexY;
        public float? Radius;
        public int NLayers;
        public int NDoodadRefs;
        public uint OffsHeight;                // MCVT
        public uint OffsNormal;                // MCNR
        public uint OffsLayer;                 // MCLY
        public uint OffsRefs;                  // MCRF
        public uint OffsAlpha;                 // MCAL
        public int SizeAlpha;
        public uint OffsShadow;                // MCSH
        public int SizeShadow;
        public uint Areaid;
        public int NMapObjRefs;
        public ushort Holes;
        public ushort Unk_0x12;
        public ushort[] PredTex; //[8];
        public byte[] NoEffectDoodad; //[8];
        public uint OffsSndEmitters;           // MCSE
        public int NSndEmitters;
        public uint OffsLiquid;                // MLIQ
        public uint? SizeLiquid;

        public NonUniformArray<float> HeightMap;
        public NonUniformArray<C3bVector> Normals;
        public uint[] AlphaMaps;
        public MCLY[] TextureLayers;
        public uint[] MapObjReferences;
        public uint[] DoodadReferences;
        public int[] Shadows;
        public MCLQ[] Liquids;
        public MCSE[] SoundEmitters;


        public MCNK(BinaryReader br, uint build, int size)
        {
            long endPos = br.BaseStream.Position + size;
            long relativeStart = br.BaseStream.Position;
            long relativeEnd = endPos - relativeStart;
            bool isAlpha = build < 3592;

            Flags = br.ReadEnum<MCNK_Flags>();
            IndexX = br.ReadUInt32();
            IndexY = br.ReadUInt32();
            if (isAlpha)
                Radius = br.ReadSingle();
            NLayers = br.ReadInt32();
            NDoodadRefs = br.ReadInt32();
            OffsHeight = br.ReadUInt32();
            OffsNormal = br.ReadUInt32();
            OffsLayer = br.ReadUInt32();
            OffsRefs = br.ReadUInt32();
            OffsAlpha = br.ReadUInt32();
            SizeAlpha = br.ReadInt32();
            OffsShadow = br.ReadUInt32();
            SizeShadow = br.ReadInt32();
            Areaid = br.ReadUInt32();
            NMapObjRefs = br.ReadInt32();
            Holes = br.ReadUInt16();
            Unk_0x12 = br.ReadUInt16();
            PredTex = br.ReadStructArray<ushort>(8);
            NoEffectDoodad = br.ReadBytes(8);
            OffsSndEmitters = br.ReadUInt32();
            NSndEmitters = br.ReadInt32();
            OffsLiquid = br.ReadUInt32();
            SizeLiquid = br.ReadUInt32();
            br.BaseStream.Position += 20; // padding

            if (build <= 3368)
                SizeLiquid = null;

            // alpha build's offsets are exclusive of header data
            if (isAlpha)
            {
                relativeStart = br.BaseStream.Position;
                relativeEnd = endPos - relativeStart;
            }

            Read(br, build, relativeStart, relativeEnd);
        }

        public void Read(BinaryReader br, uint build, long relativeStart, long relativeEnd)
        {
            bool hasLiquids = (Flags & MCNK_Flags.HasLiquid) != 0;
            bool isAlpha = build < 3592;

            foreach (var (Offset, Token) in GetOffsets(relativeEnd, isAlpha))
            {
                string token = Token;
                br.BaseStream.Position = relativeStart + Offset;

                // use chunk headers when possible
                if (!isAlpha || Token == "MCRF" || Token == "MCLY")
                {
                    var chunk = br.ReadIffChunk(true);
                    token = chunk.Token;

                    // welcome to the world of blizzard
                    bool isliquid = Token == "MCLQ" && hasLiquids;
                    if (chunk.Size <= 0 && !isliquid)
                        continue;
                }

                switch (token)
                {
                    case "MCVT":
                        HeightMap = ReadVTNR<float>(br, isAlpha);
                        break;
                    case "MCNR":
                        Normals = ReadVTNR<C3bVector>(br, isAlpha);
                        break;
                    case "MCAL":
                        AlphaMaps = br.ReadStructArray<uint>(SizeAlpha / 4);
                        break;
                    case "MCLY":
                        TextureLayers = br.ReadArray(NLayers, () => new MCLY(br));
                        break;
                    case "MCRF":
                        DoodadReferences = NDoodadRefs > 0 ? br.ReadStructArray<uint>(NDoodadRefs) : null;
                        MapObjReferences = NMapObjRefs > 0 ? br.ReadStructArray<uint>(NMapObjRefs) : null;
                        break;
                    case "MCSH":
                        Shadows = br.ReadStructArray<int>(SizeShadow / 4);
                        break;
                    case "MCLQ":
                        Liquids = GetLiquidFlags().Select(flag => new MCLQ(br, flag))?.ToArray();
                        break;
                    case "MCSE":
                        SoundEmitters = br.ReadStructArray<MCSE>(NSndEmitters);
                        break;
                    default:
                        throw new Exception();
                }
            }
        }

        private NonUniformArray<T> ReadVTNR<T>(BinaryReader br, bool isAlpha) where T : struct
        {
            if (isAlpha)
            {
                int[] cols = new[] { 9, 9, 9, 9, 9, 9, 9, 9, 9, 8, 8, 8, 8, 8, 8, 8, 8 };
                return new NonUniformArray<T>(br, 17, cols);
            }
            else
            {
                return new NonUniformArray<T>(br, 17, 9, 8);
            }
        }

        private List<(long Offset, string Token)> GetOffsets(long relativeEnd, bool isAlpha)
        {
            int offset = !isAlpha ? 8 : 0;

            var offsets = new List<(long Offset, string Token)>
            {
                (OffsHeight - offset, "MCVT"),
                (OffsNormal - offset, "MCNR"),
                (OffsLayer - offset, "MCLY"),
                (OffsRefs - offset, "MCRF"),
            };

            if (SizeAlpha > 0)
                offsets.Add((OffsAlpha - offset, "MCAL"));

            if (SizeShadow > 0)
                offsets.Add((OffsShadow - offset, "MCSH"));

            if (NSndEmitters > 0)
                offsets.Add((OffsSndEmitters - offset, "MCSE"));

            if ((Flags & MCNK_Flags.HasLiquid) != 0 || SizeLiquid > 8)
                offsets.Add((OffsLiquid - offset, "MCLQ"));

            offsets.RemoveAll(x => x.Offset >= relativeEnd);
            offsets.Sort((x, y) => x.Offset.CompareTo(y.Offset));

            return offsets;
        }

        private IEnumerable<MCNK_Flags> GetLiquidFlags()
        {
            for (int i = 0; i < 4; i++)
            {
                MCNK_Flags flag = (MCNK_Flags)(1 << (2 + i));
                if (Flags.HasFlag(flag))
                    yield return flag;
            }
        }
    }

    [Flags]
    public enum MCNK_Flags : uint
    {
        None = 0,
        HasBakedShadows = 1,
        Impassible = 2,
        IsRiver = 4,
        IsOcean = 8,
        IsMagma = 16,
        IsSlime = 32,
        HasVertexShading = 64,
        Unknown_0x80 = 128,
        DoNotRepairAlphaMaps = 32768,
        UsesHighResHoles = 65536,
        HasLiquid = IsRiver | IsOcean | IsMagma | IsSlime
    }
}
