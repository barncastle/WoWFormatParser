using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.MDX
{
    public class ATCH : GenObject
    {
        public uint Size;
        public int AttachmentId;
        public string Path;
        public MDXTrack<float> VisibilityKeys;

        public ATCH(BinaryReader br)
        {
            Size = br.ReadUInt32();
            Load(br);

            AttachmentId = br.ReadInt32();
            br.ReadByte(); // confirmed padding         
            Path = br.ReadString(260).TrimEnd('\0');

            while (true)
            {
                string token = br.ReadString(4);
                br.BaseStream.Position -= 4;
                switch (token)
                {
                    case "KVIS": VisibilityKeys = new MDXTrack<float>(br); break;
                    default: return;
                }
            }
        }
    }
}
