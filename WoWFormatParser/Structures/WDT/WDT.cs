using System;
using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.WDT
{
    public sealed class WDT : Format
    {
        public uint Version;
        public MPHD[] MapHeader;
        public MAIN[,] AreaInfo;
        public MODF[] MapObjDefinitions;
        public string[] WorldModelFileNames;

        public WDT(BinaryReader br)
        {
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
                        MapHeader = br.ReadArray(Size / 32, () => new MPHD(br));
                        break;
                    case "MAIN":
                        AreaInfo = br.ReadJaggedArray(64, 64, () => new MAIN(br));
                        break;
                    case "MODF":
                        MapObjDefinitions = br.ReadArray(Size / 64, () => new MODF(br));
                        break;
                    case "MWMO":
                        WorldModelFileNames = br.ReadString(Size).Split('\0', StringSplitOptions.RemoveEmptyEntries);
                        break;
                    default:
                        throw new NotImplementedException("Unknown token " + Token);
                }
            }

            if (br.BaseStream.Position != br.BaseStream.Length)
                throw new UnreadContentException();
        }
    }
}
