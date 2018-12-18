using System.IO;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Common;

namespace WoWFormatParser.Structures.MDX
{
    public class TXAN
    {
        public uint Size;
        public MDXTrack<C3Vector> TranslationKeys;
        public MDXTrack<C4QuaternionCompressed> RotationKeys;
        public MDXTrack<C3Vector> ScaleKeys;

        public TXAN(BinaryReader br)
        {
            Size = br.ReadUInt32();

            while (true)
            {
                string tagname = br.ReadString(4);
                br.BaseStream.Position -= 4;
                switch (tagname)
                {
                    case "KTAT": TranslationKeys = new MDXTrack<C3Vector>(br); break;
                    case "KTAR": RotationKeys = new MDXTrack<C4QuaternionCompressed>(br); break;
                    case "KTAS": ScaleKeys = new MDXTrack<C3Vector>(br); break;
                    default: return;
                }
            }
        }
    }
}
