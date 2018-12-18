using System;
using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.WMO
{
    public class MFOG
    {
        public MFOG_Flags Flags;
        public C3Vector Position;
        public float Start;
        public float End;
        public Fog[] Fogs;

        public MFOG(BinaryReader br)
        {
            Flags = br.ReadEnum<MFOG_Flags>();
            Position = br.ReadStruct<C3Vector>();
            Start = br.ReadSingle();
            End = br.ReadSingle();
            Fogs = br.ReadStructArray<Fog>(2);
        }
    }

    public struct Fog : IStringDescriptor
    {
        public float End;
        public float StartScalar;
        public CImVector Color;

        public override string ToString() => $"End: {End}, StartScalar: {StartScalar}, Color: [{Color}]";
    }

    [Flags]
    public enum MFOG_Flags : uint
    {
        None = 0,
        InteriorExteriorBlend = 1
    }

    public enum EFogs : uint
    {
        Fog,
        UnderWaterFog
    }
}
