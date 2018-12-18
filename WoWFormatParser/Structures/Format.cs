using WoWFormatParser.Helpers;
using WoWFormatParser.Serializer;
using WoWFormatParser.Structures.Meta;

namespace WoWFormatParser.Structures
{
    public abstract class Format : IFormat
    {
        /// <summary>
        /// File Information e.g. Created date, checksum etc.
        /// </summary>
        public FileInfo FileInfo { get; internal set; } = null;
        public string FileName { get; internal set; }


        /// <summary>
        /// Exports the jsonified parsed content of the file.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="fileName"></param>
        public virtual void Export(SerializerOptions options = null, string fileName = "")
        {
            options = options ?? new SerializerOptions();

            if (string.IsNullOrWhiteSpace(fileName))
                fileName = Utils.GetExportFileName(FileName, options.Compression);

            var _serializer = new Serializer.Serializer(options);
            _serializer.Export(fileName, this);
        }
        /// <summary>
        /// Returns the jsonified parsed content of the file.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public virtual string ToJson(SerializerOptions options = null)
        {
            options = options ?? new SerializerOptions();

            var _serializer = new Serializer.Serializer(options);
            return _serializer.Serialize(this);
        }
    }
}
