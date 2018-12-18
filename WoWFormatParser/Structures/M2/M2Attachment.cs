using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.M2
{
    public class M2Attachment : IVersioned
    {
        public GeoComponentLink AttachmentId;
        public ushort BoneIndex;
        public ushort Unknown_0x6;
        public C3Vector Position;
        public M2Track<bool> AnimTrack;

        public M2Attachment(BinaryReader br, uint build)
        {
            AttachmentId = br.ReadEnum<GeoComponentLink>();
            BoneIndex = br.ReadUInt16();
            Unknown_0x6 = br.ReadUInt16();
            Position = br.ReadStruct<C3Vector>();
            AnimTrack = new M2Track<bool>(br, build);
        }
    }

    public enum GeoComponentLink : int
    {
        ATTACH_NONE = -1,
        ATTACH_SHIELD = 0x0,
        ATTACH_HANDR = 0x1,
        ATTACH_HANDL = 0x2,
        ATTACH_ELBOWR = 0x3,
        ATTACH_ELBOWL = 0x4,
        ATTACH_SHOULDERR = 0x5,
        ATTACH_SHOULDERL = 0x6,
        ATTACH_KNEER = 0x7,
        ATTACH_KNEEL = 0x8,
        ATTACH_HIPR = 0x9,
        ATTACH_HIPL = 0xA,
        ATTACH_HELM = 0xB,
        ATTACH_BACK = 0xC,
        ATTACH_SHOULDERFLAPR = 0xD,
        ATTACH_SHOULDERFLAPL = 0xE,
        ATTACH_TORSOBLOODFRONT = 0xF,
        ATTACH_TORSOBLOODBACK = 0x10,
        ATTACH_BREATH = 0x11,
        ATTACH_PLAYERNAME = 0x12,
        ATTACH_UNITEFFECT_BASE = 0x13,
        ATTACH_UNITEFFECT_HEAD = 0x14,
        ATTACH_UNITEFFECT_SPELLLEFTHAND = 0x15,
        ATTACH_UNITEFFECT_SPELLRIGHTHAND = 0x16,
        ATTACH_UNITEFFECT_SPECIAL1 = 0x17,
        ATTACH_UNITEFFECT_SPECIAL2 = 0x18,
        ATTACH_UNITEFFECT_SPECIAL3 = 0x19,
        ATTACH_SHEATH_MAINHAND = 0x1A,
        ATTACH_SHEATH_OFFHAND = 0x1B,
        ATTACH_SHEATH_SHIELD = 0x1C,
        ATTACH_PLAYERNAMEMOUNTED = 0x1D,
        ATTACH_LARGEWEAPONLEFT = 0x1E,
        ATTACH_LARGEWEAPONRIGHT = 0x1F,
        ATTACH_HIPWEAPONLEFT = 0x20,
        ATTACH_HIPWEAPONRIGHT = 0x21,
        ATTACH_TORSOSPELL = 0x22,
        ATTACH_HANDARROW = 0x23,
        ATTACH_BULLET = 0x24,
        ATTACH_HANDOMNISPELL = 0x25,
        ATTACH_HANDDIRSPELL = 0x26,
    }
}
