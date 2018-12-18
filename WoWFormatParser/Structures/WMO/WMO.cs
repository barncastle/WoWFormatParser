using System;
using System.Collections.Generic;
using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.WMO
{
    public sealed class WMO : Format, IVersioned
    {
        public uint Version;
        public MOHD MapObjectHeader;
        public string[] TextureFileNames;
        public MOMT[] Materials;
        public string[] GroupNames;
        public MOGI[] GroupInfo;
        public string[] SkyboxFileNames;
        public C3Vector[] PortalVertices;
        public MOPT[] Portals;
        public MOPR[] PortalReferences;
        public C3Vector[] VisibleVertices;
        public MOVB[] VisibleBlocks;
        public MOLT[] Lights;
        public MODS[] DoodadSets;
        public string[] DoodadFileNames;
        public MODD[] DoodadDefinitions;
        public MFOG[] Fogs;
        public C4Plane[] ConvexVolumePlanes;
        public IReadOnlyList<WMOGroup> MapObjectGroups;

        public WMO(BinaryReader br, uint build)
        {
            List<WMOGroup> _Groups = new List<WMOGroup>();

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
                    case "MOMO": // technically incorrect but an unnecessary chunk
                        continue;
                    case "MOHD":
                        MapObjectHeader = new MOHD(br, Version);
                        break;
                    case "MOTX":
                        TextureFileNames = br.ReadString(Size).Split('\0', StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case "MOSB":
                        SkyboxFileNames = br.ReadString(Size).Split('\0', StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case "MOGN":
                        GroupNames = br.ReadString(Size).Split('\0', StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case "MODN":
                        DoodadFileNames = br.ReadString(Size).Split('\0', StringSplitOptions.RemoveEmptyEntries);
                        break;
                    case "MOMT":
                        Materials = br.ReadArray(Size / MOMT.GetSize(Version), () => new MOMT(br, Version));
                        break;
                    case "MOGI":
                        GroupInfo = br.ReadArray(Size / MOGI.GetSize(Version), () => new MOGI(br, Version));
                        break;
                    case "MOPV":
                        PortalVertices = br.ReadStructArray<C3Vector>(Size / 12);
                        break;
                    case "MOVV":
                        VisibleVertices = br.ReadStructArray<C3Vector>(Size / 12);
                        break;
                    case "MOPT":
                        Portals = br.ReadStructArray<MOPT>(Size / 20);
                        break;
                    case "MOPR":
                        PortalReferences = br.ReadStructArray<MOPR>(Size / 8);
                        break;
                    case "MOVB":
                        VisibleBlocks = br.ReadStructArray<MOVB>(Size / 4);
                        break;
                    case "MOLT":
                        Lights = br.ReadArray(Size / MOLT.GetSize(Version), () => new MOLT(br, Version));
                        break;
                    case "MODS":
                        DoodadSets = br.ReadArray(Size / 32, () => new MODS(br));
                        break;
                    case "MODD":
                        DoodadDefinitions = br.ReadArray(Size / 40, () => new MODD(br));
                        break;
                    case "MFOG":
                        Fogs = br.ReadArray(Size / 48, () => new MFOG(br));
                        break;
                    case "MCVP":
                        ConvexVolumePlanes = br.ReadStructArray<C4Plane>(Size / 16);
                        break;
                    case "MOGP":
                        _Groups.Add(ReadGroup(br, build, Size));
                        continue;
                    default:
                        throw new NotImplementedException("Unknown token " + Token);
                }
            }

            if (br.BaseStream.Position != br.BaseStream.Length)
                throw new UnreadContentException();

            if (_Groups.Count > 0)
                MapObjectGroups = _Groups;
        }

        private WMOGroup ReadGroup(BinaryReader br, uint build, int size)
        {
            // reset offset
            br.BaseStream.Position -= 8;

            // pass into the WMOGroup reader
            using (var ms = new MemoryStream(br.ReadBytes(size + 8)))
            using (var subbr = new BinaryReader(ms))
                return new WMOGroup(subbr, build);
        }
    }
}
