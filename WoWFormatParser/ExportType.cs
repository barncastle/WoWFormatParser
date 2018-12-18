using System;

namespace WoWFormatParser
{
    [Flags]
    public enum ExportType
    {
        /// <summary>
        /// Parsed Structure Data.
        /// </summary>
        Data = 1,
        /// <summary>
        /// File Information e.g. Created date, checksum etc.
        /// </summary>
        FileInfo = 2,
        /// <summary>
        /// Parsed Struture with File Information.
        /// </summary>
        Both = Data | FileInfo
    }
}
