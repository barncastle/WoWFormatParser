using System;
using System.IO;
using WoWFormatParser.Serializer;

namespace WoWFormatParser.Helpers
{
    internal static class Utils
    {
        public static bool IsInvalidFile(string fullpath, Options options, string searchPattern = "*")
        {
            bool invalid = options.HasIgnoredDirectory(fullpath);
            invalid ^= options.HasIgnoredFormat(fullpath.GetExtensionExt());
            invalid ^= !fullpath.ContainsMask(searchPattern);

            if (!options.IncludeUnsupportedAndInvalidFiles)
                invalid ^= !Enum.TryParse<WoWFormat>(fullpath.GetExtensionExt(), true, out var dump);

            return invalid;
        }

        public static string CorrectSearchPattern(string searchPattern)
        {
            if (string.IsNullOrWhiteSpace(searchPattern) || searchPattern.Trim() == "*")
                return "*";

            return searchPattern;
        }

        public static DateTime GetLocalFileCreated(string filePath)
        {
            var created = File.GetCreationTimeUtc(filePath);
            var modified = File.GetLastWriteTimeUtc(filePath);
            return created < modified ? created : modified;
        }

        public static string GetExportFileName(string filePath, CompressionFormat compression)
        {
            switch (compression)
            {
                case CompressionFormat.Brotli:
                    return Path.ChangeExtension(filePath, ".brot");
                case CompressionFormat.GZip:
                    return Path.ChangeExtension(filePath, ".gz");
                default:
                    return Path.ChangeExtension(filePath, ".json");
            }
        }
    }
}
