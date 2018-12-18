using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.Meta
{
    public sealed class DBCMeta : Format, IMetaFormat
    {
        public string Name;
        public string Magic;
        public uint Build;
        public uint RecordCount;
        public uint FieldCount;
        public uint RecordSize;
        public uint StringTableSize;


        public DBCMeta(string name, uint build, Stream stream)
        {
            using (var br = new BinaryReader(stream))
            {
                Name = name;
                Magic = br.ReadString(4);
                RecordCount = br.ReadUInt32();
                FieldCount = br.ReadUInt32();
                RecordSize = br.ReadUInt32();
                StringTableSize = br.ReadUInt32();
                Build = build;
            }
        }
    }
}
