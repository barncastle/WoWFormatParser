using WoWFormatParser.Structures;
using WoWFormatParser.Structures.MDX;

namespace WoWFormatParser
{
    public static class Extensions
    {
        public static T Cast<T>(this IFormat format) where T : Format => format as T;
        public static bool Is<T>(this IFormat format) where T : Format => format is T;

        public static T Cast<T>(this IGEOS format) where T : class => format as T;
        public static bool Is<T>(this IGEOS format) where T : class => format is T;
    }
}
