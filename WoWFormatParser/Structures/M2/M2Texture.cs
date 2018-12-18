using System;
using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.M2
{
    public class M2Texture : IVersioned
    {
        public TEX_COMPONENT ReplaceableId;
        public TEXFLAGS Flags;
        public string Image;

        public M2Texture(BinaryReader br, uint build)
        {
            ReplaceableId = br.ReadEnum<TEX_COMPONENT>();
            Flags = br.ReadEnum<TEXFLAGS>();
            Image = br.ReadM2Array<byte>(build)?.AsString();
        }
    }

    [Flags]
    public enum TEXFLAGS : uint
    {
        WRAPWIDTH = 1,
        WRAPHEIGHT = 2
    }

    public enum TEX_COMPONENT : uint
    {
        TEX_COMPONENT_NONE = 0x0,
        TEX_COMPONENT_SKIN = 0x1,
        TEX_COMPONENT_OBJECT_SKIN = 0x2,
        TEX_COMPONENT_WEAPON_BLADE = 0x3,
        TEX_COMPONENT_WEAPON_HANDLE = 0x4,
        TEX_COMPONENT_ENVIRONMENT = 0x5,
        TEX_COMPONENT_CHAR_HAIR = 0x6,
        TEX_COMPONENT_CHAR_FACIAL_HAIR = 0x7,
        TEX_COMPONENT_SKIN_EXTRA = 0x8,
        TEX_COMPONENT_UI_SKIN = 0x9,
        TEX_COMPONENT_TAUREN_MANE = 0xA,
        TEX_COMPONENT_MONSTER_1 = 0xB,
        TEX_COMPONENT_MONSTER_2 = 0xC,
        TEX_COMPONENT_MONSTER_3 = 0xD,
        TEX_COMPONENT_ITEM_ICON = 0xE,
        NUM_REPLACEABLE_MATERIAL_IDS = 0xF,
    };
}
