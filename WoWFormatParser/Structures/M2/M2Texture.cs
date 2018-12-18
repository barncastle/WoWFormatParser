using System;
using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.M2
{
    public class M2Texture : IVersioned
    {
        public TexComponent ReplaceableId;
        public Tex_Flags Flags;
        public string Image;

        public M2Texture(BinaryReader br, uint build)
        {
            ReplaceableId = br.ReadEnum<TexComponent>();
            Flags = br.ReadEnum<Tex_Flags>();
            Image = br.ReadM2Array<byte>(build)?.AsString();
        }
    }

    [Flags]
    public enum Tex_Flags : uint
    {
        WrapWidth = 1,
        WrapHeight = 2
    }

    public enum TexComponent : uint
    {
        None = 0x0,
        Skin = 0x1,
        ObjectSkin = 0x2,
        WeaponBlade = 0x3,
        WeaponHandle = 0x4,
        Environment = 0x5,
        CharHair = 0x6,
        CharFacialHair = 0x7,
        SkinExtra = 0x8,
        UISkin = 0x9,
        TaurenMane = 0xa,
        Monster1 = 0xb,
        Monster2 = 0xc,
        Monster3 = 0xd,
        ItemIcon = 0xe,
    };
}
