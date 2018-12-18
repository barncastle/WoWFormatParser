using WoWFormatParser.Serializer;
using WoWFormatParser.Structures.Meta;

namespace WoWFormatParser.Structures
{
    public interface IFormat
    {
        FileInfo FileInfo { get; }

        string FileName { get; }

        void Export(SerializerOptions options = null, string fileName = "");

        string ToJson(SerializerOptions options = null);
    }
}
