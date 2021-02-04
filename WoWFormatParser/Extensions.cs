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

        /// <summary>
        /// Finds the string based on its starting index (pointer) 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string FindByPointer(this string[] values, int index)
        {
            int l = 0, i = 0;
            for (; i < values.Length && l < index; i++)
                l += values[i].Length + 1;

            return l == index ? values[i] : "";
        }
    }
}
