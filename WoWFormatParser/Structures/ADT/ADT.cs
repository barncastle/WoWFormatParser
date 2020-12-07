using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.ADT
{
    public sealed class ADT : Format, IVersioned
    {
        [JsonIgnore]
        private readonly bool IsAlphaFormat = false;

        public uint Version;
        public string[] WorldModelFileNames;
        public string[] TextureFileNames;
        public string[] ModelFileNames;
        public uint[] ModelFileNameIndices;
        public uint[] WorldModelFileNameIndices;
        public WDT.MODF[] MapObjDefinitions;
        public MDDF[] MapModelDefinitions;
        public MHDR MapHeader;
        public MCIN[,] ChunkInfo;
        public IReadOnlyList<MCNK> MapChunks;

        public ADT(BinaryReader br, uint build)
        {
            IsAlphaFormat = build < 3592;

            List<MCNK> _MapChunks = new List<MCNK>();

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
                        WorldModelFileNames = br.ReadString(Size).Split('\0', StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case "MTEX":
                        TextureFileNames = br.ReadString(Size).Split('\0', StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case "MMDX":
                        ModelFileNames = br.ReadString(Size).Split('\0', StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case "MMID":
                        ModelFileNameIndices = br.ReadStructArray<uint>(Size / 4);
                        break;
                    case "MWID":
                        WorldModelFileNameIndices = br.ReadStructArray<uint>(Size / 4);
                        break;
                    case "MODF":
                        MapObjDefinitions = br.ReadArray(Size / 64, () => new WDT.MODF(br));
                        break;
                    case "MHDR":
                        MapHeader = br.ReadStruct<MHDR>();
                        break;
                    case "MCIN":
                        ChunkInfo = br.ReadJaggedArray(16, 16, () => br.ReadStruct<MCIN>());
                        break;
                    case "MDDF":
                        MapModelDefinitions = br.ReadArray(Size / 0x24, () => new MDDF(br));
                        break;
                    case "MCNK":
                        _MapChunks.Add(new MCNK(br, build, Size));
                        break;
                    default:
                        throw new NotImplementedException("Unknown token " + Token);
                }
            }

            ValidateIsRead(br, br.BaseStream.Length);

            if (_MapChunks.Count > 0)
                MapChunks = _MapChunks;
        }


        private void ValidateIsRead(BinaryReader br, long length)
        {
            if (br.BaseStream.Position != length)
            {
                if (!IsAlphaFormat)
                {
                    while (br.BaseStream.Position < length)
                        if (br.ReadIffChunk().Size > 0)
                            throw new UnreadContentException();
                }
                else
                {
                    throw new UnreadContentException();
                }
            }
        }
    }
}
