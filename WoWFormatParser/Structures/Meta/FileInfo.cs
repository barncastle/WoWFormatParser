using System;
using System.IO;
using System.Linq;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.Meta
{
    public sealed class FileInfo
    {
        private string _name;
        public uint Build;
        public uint Size;
        public DateTime? Created;
        public string Checksum;
        public WoWFormat Format { get; private set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                CalculateFormat();
            }
        }

        private void CalculateFormat()
        {
            Format = WoWFormat.Unsupported;

            if (Enum.TryParse<WoWFormat>(_name.GetExtensionExt(), true, out var format))
                Format = format;

            string fileending = Path.GetFileNameWithoutExtension(_name).Split("_")[^1];

            // overrides
            switch (Format)
            {
                case WoWFormat.WMO when ushort.TryParse(fileending, out _):
                    Format = WoWFormat.WMOGROUP;
                    break;
                case WoWFormat.ADT:
                    switch (fileending)
                    {
                        case "LOD":
                            break;
                        case "OBJ0":
                        case "OBJ1":
                            break;
                        case "TEX0":
                        case "TEX1":
                            break;
                    }
                    break;
                case WoWFormat.WDT:
                    switch (fileending)
                    {
                        case "OCC":
                            break;
                        case "LGT":
                            break;
                        case "FOGS":
                            break;
                        case "MPV":
                            break;
                        default:
                            Format = Build < 3592 ? WoWFormat.WDTADT : WoWFormat.WDT;
                            break;
                    }
                    break;
            }
        }
    }
}
