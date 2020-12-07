using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.WDT
{
    public sealed class WDT : Format, IVersioned
    {
        [JsonIgnore]
        private readonly bool IsAlphaFormat = false;

        public uint Version;
        public MPHD[] MapHeader;
        public MAIN[,] AreaInfo;
        public MODF[] MapObjDefinitions;
        public string[] WorldModelFileNames;
        public string[] DoodadFileNames;
        public ADT.ADT[] Tiles;

        public WDT(BinaryReader br, uint build)
        {
            IsAlphaFormat = build < 3592;

            List<ADT.ADT> _Tiles = new List<ADT.ADT>();

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
                    case "MPHD":
                        MapHeader = br.ReadArray(IsAlphaFormat ? 1 : Size / 32, () => new MPHD(br, build));
                        break;
                    case "MAIN":
                        AreaInfo = br.ReadJaggedArray(64, 64, () => new MAIN(br, build));
                        break;
                    case "MODF":
                        MapObjDefinitions = br.ReadArray(Size / 64, () => new MODF(br));
                        break;
                    case "MONM":
                    case "MWMO":
                        WorldModelFileNames = br.ReadString(Size).Split('\0', StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case "MDNM":
                        DoodadFileNames = br.ReadString(Size).Split('\0', StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case "MHDR":
                        _Tiles.Add(ReadTile(br, build));
                        break;
                    default:
                        throw new NotImplementedException("Unknown token " + Token);
                }
            }

            if (br.BaseStream.Position != br.BaseStream.Length)
                throw new UnreadContentException();

            Tiles = _Tiles.ToArray();
        }

        private ADT.ADT ReadTile(BinaryReader br, uint build)
        {
            // reset offset
            br.BaseStream.Position -= 8;

            // calculate total ADT size
            var size = GetADTSize(br);

            // pass into the ADT reader
            using var stream = new SubStream(br.BaseStream, size);
            return new ADT.ADT(stream.GetBinaryReader(), build);
        }

        private long GetADTSize(BinaryReader br)
        {
            int offset = (int)br.BaseStream.Position;

            for (var x = 0; x < AreaInfo.GetLength(0); x++)
                for (var y = 0; y < AreaInfo.GetLength(1); y++)
                    if (AreaInfo[x, y].Offset > offset)
                        return AreaInfo[x, y].Offset - offset;

            return br.BaseStream.Length - offset;
        }
    }
}
