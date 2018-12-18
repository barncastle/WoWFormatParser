using FileParser.FileTypes.Serializers;
using WoWFormatParser.Structures;

namespace WoWFormatParser.Serializer
{
    public class SerializerOptions
    {
        /// <summary>
        /// Json Formatting to be indentated.
        /// </summary>
        public bool Indent { get; set; } = true;
        /// <summary>
        /// What information to be serialized.
        /// </summary>
        public ExportType ExportType { get; set; } = ExportType.Data;
        /// <summary>
        /// What compression, if any, to be applied.
        /// <para>Compressed JSON is base64 encoded.</para>
        /// </summary>
        public CompressionFormat Compression { get; set; } = CompressionFormat.None;
        public RenameIgnoreContractResolver RenameIgnoreResolver { get; private set; } = new RenameIgnoreContractResolver();
        /// <summary>
        /// Directory exports are saved to.
        /// </summary>
        public string OutputDirectory { get; set; }
        /// <summary>
        /// Ignore null values and empty strings/collections.
        /// </summary>
        public bool IgnoreNullOrEmpty
        {
            get => RenameIgnoreResolver.IgnoreNullOrEmpty;
            set => RenameIgnoreResolver.IgnoreNullOrEmpty = value;
        }


        internal void ValidateAndCheck()
        {
            if (!string.IsNullOrWhiteSpace(OutputDirectory))
                System.IO.Directory.CreateDirectory(OutputDirectory);

            if (!ExportType.HasFlag(ExportType.FileInfo))
                RenameIgnoreResolver.IgnoreProperty(typeof(IFormat), "FileInfo");
            else
                RenameIgnoreResolver.RemoveIgnoreProperty(typeof(IFormat), "FileInfo");
        }
    }

    public enum CompressionFormat
    {
        None,
        GZip,
        Brotli
    }
}
