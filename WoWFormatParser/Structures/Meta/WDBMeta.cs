using System;
using System.IO;
using System.Linq;
using System.Text;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.Meta
{
    public sealed class WDBMeta : Format, IMetaFormat
    {
        public string Magic;
        public string Version;
        public uint Build;
        public string Language;
        public uint RowLength;


        public WDBMeta(string name, uint build, Stream stream)
        {
            using (var br = new BinaryReader(stream))
            {
                FileName = name;
                Magic = br.ReadString(4).FastReverse();
                Build = br.ReadUInt32();

                // TODO figure out exact build this changed
                // set language if applicable
                byte[] data = br.ReadBytes(4);
                if (data.All(b => b >= 65 && b <= 90))
                {
                    Language = Encoding.UTF8.GetString(data).FastReverse();
                    RowLength = br.ReadUInt32();
                }
                else
                {
                    RowLength = BitConverter.ToUInt32(data, 0);
                }

                Version = br.ReadUInt32().ToString();
            }
        }
    }
}
