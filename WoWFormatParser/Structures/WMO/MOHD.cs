using System;
using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.WMO
{
    public class MOHD
    {
        public uint NTextures;
        public uint NGroups;
        public uint NPortals;
        public uint NLights;
        public uint NDoodadNames;
        public uint NDoodadDefs;
        public uint NDoodadSets;
        public CImVector AmbColor;
        public uint WmoID;
        public CAaBox BoundingBox;
        public MOHD_Flags? Flags;

        public MOHD(BinaryReader br, uint version)
        {
            NTextures = br.ReadUInt32();
            NGroups = br.ReadUInt32();
            NPortals = br.ReadUInt32();
            NLights = br.ReadUInt32();
            NDoodadNames = br.ReadUInt32();
            NDoodadDefs = br.ReadUInt32();
            NDoodadSets = br.ReadUInt32();
            AmbColor = br.ReadStruct<CImVector>();
            WmoID = br.ReadUInt32();

            if (version == 14)
            {
                br.BaseStream.Position += 0x1C;
            }
            else
            {
                BoundingBox = br.ReadStruct<CAaBox>();
                Flags = br.ReadEnum<MOHD_Flags>();
            }

        }
    }

    [Flags]
    public enum MOHD_Flags : uint
    {
        None = 0,
        DontAttenuateVerticesBasedOnPortalDistance = 0x01,
        UseUnifiedRenderPath = 0x02,
        UseLiquidTypeDBC = 0x04,
        DoNotFixVertexColorAlpha = 0x08,
    }
}
