using System;
using System.Collections.Generic;
using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.WDT;

namespace WoWFormatParser.Structures.WDL
{
    public sealed class WDL : Format
    {
        public uint Version;
        public string[] MapWorldModelObjects;
        public uint[] WorldModelObjectFilenameIndices;
        public MODF[] MapObjDefinitions;
        public uint[,] MapAreaOffsets;
        public IReadOnlyList<MARE> MapAreaVertices;
        public ushort[] MapAreaOcclusion;
        public ushort[] MapAreaHoles;

        public WDL(BinaryReader br)
        {
            List<MARE> _MapAreaVertices = new List<MARE>();

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
                    case "MWMO":
                        MapWorldModelObjects = br.ReadString(Size).Split('\0');
                        break;
                    case "MWID":
                        WorldModelObjectFilenameIndices = br.ReadStructArray<uint>(Size / 4);
                        break;
                    case "MODF":
                        MapObjDefinitions = br.ReadArray(Size / 64, () => new MODF(br));
                        break;
                    case "MAOF":
                        MapAreaOffsets = br.ReadJaggedArray(64, 64, () => br.ReadUInt32());
                        break;
                    case "MAOC":
                        MapAreaOcclusion = br.ReadStructArray<ushort>(Size / 2);
                        break;
                    case "MARE":
                        _MapAreaVertices.Add(new MARE(br));
                        break;
                    case "MAHO":
                        MapAreaHoles = br.ReadStructArray<ushort>(16);
                        break;
                    default:
                        throw new NotImplementedException("Unknown token " + Token);
                }
            }

            if (_MapAreaVertices.Count > 0)
                MapAreaVertices = _MapAreaVertices;
        }
    }
}
