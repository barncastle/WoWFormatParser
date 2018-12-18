using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.MDX
{
    public class EVTS : GenObject
    {
        public uint Size;
        public MDXSimpleTrack EventKeys;

        public EVTS(BinaryReader br)
        {
            Size = br.ReadUInt32();
            Load(br);

            while (true)
            {
                string token = br.ReadString(4);
                br.BaseStream.Position -= 4;
                switch (token)
                {
                    case "KEVT": EventKeys = new MDXSimpleTrack(br); break;
                    default: return;
                }
            }
        }
    }
}
