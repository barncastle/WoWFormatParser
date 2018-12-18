using System;

namespace WoWFormatParser
{
    public class WoWBuild
    {
        public readonly byte Expansion;
        public readonly byte Major;
        public readonly byte Minor;
        public readonly ushort Build;

        public WoWBuild(byte expansion, byte major, byte minor, ushort build)
        {
            Expansion = expansion;
            Major = major;
            Minor = minor;
            Build = build;
        }

        public WoWBuild(string buildstring)
        {
            string[] parts = buildstring.Split('.');

            if (!(parts.Length == 4) ||
                !byte.TryParse(parts[0], out Expansion) ||
                !byte.TryParse(parts[1], out Major) ||
                !byte.TryParse(parts[2], out Minor) ||
                !ushort.TryParse(parts[3], out Build))
            {
                throw new ArgumentException("Invalid Build format.");
            }
        }

        public override string ToString() => $"{Expansion}.{Major}.{Minor}.{Build}";

        public static WoWBuild Parse(string buildstring) => new WoWBuild(buildstring);
    }
}
