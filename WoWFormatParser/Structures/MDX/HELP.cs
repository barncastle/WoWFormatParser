using System.IO;

namespace WoWFormatParser.Structures.MDX
{
    public class HELP : GenObject
    {
        public HELP(BinaryReader br)
        {
            Load(br);
        }
    }
}
