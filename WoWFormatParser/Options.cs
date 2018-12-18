using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WoWFormatParser.Serializer;

namespace WoWFormatParser
{
    public class Options
    {
        /// <summary>
        /// Determines what information to be parsed.
        /// </summary>
        public ParseMode ParseMode { get; set; } = ParseMode.Data;
        /// <summary>
        /// Directories that will be excluded.
        /// </summary>
        public HashSet<string> IgnoredDirectories { get; private set; }
        /// <summary>
        /// Formats that will be excluded.
        /// </summary>
        public HashSet<WoWFormat> IgnoredFormats { get; private set; }
        /// <summary>
        /// Return basic File Info for non-supported types.
        /// </summary>
        public bool IncludeUnsupportedAndInvalidFiles { get; set; } = false;
        /// <summary>
        /// Options for JSON serialization of parsed formats.
        /// </summary>
		public SerializerOptions SerializerOptions { get; private set; }
        /// <summary>
        /// Maximum size of files that will be parsed.
        /// </summary>
        public uint MaxFileSize { get; set; } = 0;


        public Options()
        {
            IgnoredDirectories = new HashSet<string>(StringComparer.InvariantCulture);
            IgnoredFormats = new HashSet<WoWFormat>();
            SerializerOptions = new SerializerOptions();
        }


        public bool HasIgnoredDirectory(string path)
        {
            return path.Split(Path.DirectorySeparatorChar).Any(x => IgnoredDirectories.Contains(x));
        }

        public bool HasIgnoredFormat(string extension)
        {
            if (Enum.TryParse<WoWFormat>(extension, true, out var format))
                return IgnoredFormats.Contains(format);

            return !IncludeUnsupportedAndInvalidFiles;
        }

        public bool HasIgnoredFormat(WoWFormat format)
        {
            bool ignored = IgnoredFormats.Contains(format);
            if (!IncludeUnsupportedAndInvalidFiles)
                ignored = format == WoWFormat.Unsupported;

            return ignored;
        }
    }
}
