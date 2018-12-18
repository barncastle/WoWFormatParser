using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.DEF
{
    public sealed class DEF : Format
    {
        public IReadOnlyList<string> WorldMapNames;

        public DEF(BinaryReader br)
        {
            List<string> _WorldMapNames = new List<string>();

            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                var (Token, Size) = br.ReadIffChunk(true);
                if (Size <= 0)
                    continue;

                switch (Token)
                {
                    case "DMAP":
                        _WorldMapNames.Add(Encoding.UTF8.GetString(br.ReadBytes(Size)));
                        break;
                    default:
                        throw new NotImplementedException("Unknown token " + Token);
                }
            }

            if (_WorldMapNames.Count > 0)
                WorldMapNames = _WorldMapNames;
        }
    }
}
