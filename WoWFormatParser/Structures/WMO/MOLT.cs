using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.WMO
{
    public class MOLT
    {
        public LightType Type;
        public byte UseAtten;
        public byte[] Unknown_0x2; // 2
        public CImVector Color;
        public C3Vector Position;
        public float Intensity;
        public float AttenStart;
        public float AttenEnd;
        public float[] Unknown_0x18;

        public MOLT(BinaryReader br, uint version)
        {
            Type = br.ReadEnum<LightType>();
            UseAtten = br.ReadByte();
            Unknown_0x2 = br.ReadBytes(2);
            Color = br.ReadStruct<CImVector>();
            Position = br.ReadStruct<C3Vector>();
            Intensity = br.ReadSingle();

            if (version != 14)
                Unknown_0x18 = br.ReadArray(4, () => br.ReadSingle());

            AttenStart = br.ReadSingle();
            AttenEnd = br.ReadSingle();
        }

        public static int GetSize(uint version) => version == 14 ? 0x20 : 48;
    }

    public enum LightType : byte
    {
        Omnidirectional = 0,
        Spot = 1,
        Direct = 2,
        Ambient = 3,
    }
}
