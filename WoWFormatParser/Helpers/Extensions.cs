using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using WoWFormatParser.Archives.MPQ;
using WoWFormatParser.Structures.M2;

namespace WoWFormatParser.Helpers
{
    internal static class Extensions
    {
        public static string ReadString(this BinaryReader br, int size)
        {
            return Encoding.UTF8.GetString(br.ReadBytes(size));
        }

        public static string ReadCString(this BinaryReader br)
        {
            StringBuilder sb = new StringBuilder(0x80);

            char tmpChar;
            while ((tmpChar = br.ReadChar()) != '\0')
                sb.Append(tmpChar);

            return sb.ToString();
        }

        public static unsafe string FastReverse(this string s)
        {
            char[] sarr = new char[s.Length];
            fixed (char* c = s)
            fixed (char* d = sarr)
            {
                char* c1 = c;
                char* d1 = d + s.Length;
                while (d1 > d)
                    *--d1 = *c1++;
            }

            return new string(sarr);
        }

        public static string WoWNormalize(this string s)
        {
            return s.Replace('/', '\\').ToUpperInvariant();
        }

        public static string ToHex(this byte[] bytes, bool reverse = false, int max = 32)
        {
            if (reverse)
                Array.Reverse(bytes, 0, bytes.Length);

            int maxlen = Math.Min(max, bytes.Length);
            char[] c = new char[maxlen * 2];

            int b;
            for (int i = 0; i < maxlen; i++)
            {
                b = bytes[i] >> 4;
                c[i * 2] = (char)(55 + b + (((b - 10) >> 31) & -7));
                b = bytes[i] & 0xF;
                c[i * 2 + 1] = (char)(55 + b + (((b - 10) >> 31) & -7));
            }

            return new string(c);
        }

        public static string ToChecksum(this byte[] bytes)
        {
            return bytes.ToHex().Substring(0, 32);
        }

        public static T[] ReadArray<T>(this BinaryReader br, int count, Func<T> func)
        {
            if (count <= 0)
                return null;

            var result = new T[count];
            for (int i = 0; i < count; i++)
                result[i] = func();

            return result;
        }

        public static T[,] ReadJaggedArray<T>(this BinaryReader br, int xcount, int ycount, Func<T> func)
        {
            if (xcount <= 0 || ycount <= 0)
                return null;

            var result = new T[xcount, ycount];

            for (int x = 0; x < xcount; x++)
                for (int y = 0; y < ycount; y++)
                    result[x, y] = func();

            return result;
        }

        public static T ReadStruct<T>(this BinaryReader reader)
        {
            if (!typeof(T).IsValueType)
                throw new NotSupportedException();

            byte[] buffer = reader.ReadBytes(SizeCache.GetSize(typeof(T)));
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            T result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return result;
        }

        public static T[] ReadStructArray<T>(this BinaryReader reader, int count)
        {
            if (!typeof(T).IsValueType)
                throw new NotSupportedException();

            if (count <= 0)
                return null;

            int size = SizeCache.GetSize(typeof(T));
            byte[] buffer = reader.ReadBytes(size * count);
            GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            IntPtr addr = handle.AddrOfPinnedObject();

            T[] result = new T[count];
            for (int i = 0; i < count; i++)
                result[i] = (T)Marshal.PtrToStructure(addr + (i * size), typeof(T));

            handle.Free();
            return result;
        }

        public static M2Array<T> ReadM2Array<T>(this BinaryReader reader, uint version)
        {
            var array = new M2Array<T>(reader, version);
            return array.Count > 0 ? array : null;
        }

        public static bool ContainsMask(this string input, string mask)
        {
            const StringComparison comp = StringComparison.OrdinalIgnoreCase;

            if (string.IsNullOrWhiteSpace(mask) || mask.Trim() == "*")
                return true;

            string[] parts = mask.Replace("/", "\\").Split('*', StringSplitOptions.RemoveEmptyEntries);

            // check starts with
            if (mask[0] != '*' && !input.StartsWith(parts[0], comp))
                return false;

            // check ends with
            if (mask[mask.Length - 1] != '*' && !input.EndsWith(parts[parts.Length - 1], comp))
                return false;

            // check order of parts
            int previousIndex = 0;
            for (int i = 0; i < parts.Length; i++)
            {
                int index = input.IndexOf(parts[i], previousIndex, comp);
                if (index == -1)
                    return false;

                previousIndex = index;
            }

            return true;
        }

        public static string GetExtensionExt(this string path)
        {
            return Path.GetExtension(path).TrimStart('.').ToUpperInvariant();
        }

        public static (string Token, int Size) ReadIffChunk(this BinaryReader br, bool reversed = false)
        {
            string token = br.ReadString(4);
            if (reversed)
                token = token.FastReverse();

            return (token, br.ReadInt32());
        }

        public static string GetFileName(this BinaryReader br)
        {
            if (br.BaseStream is MpqFileStream baseStream)
                return baseStream.FileName;

            if (br.BaseStream is BufferedStream bufferedStream)
                if (bufferedStream.UnderlyingStream is MpqFileStream underlyingStream)
                    return underlyingStream.FileName;

            return "";
        }

        public static void AssertTag(this BinaryReader br, string tag)
        {
            string _tag = br.ReadString(4);
            if (_tag != tag)
                throw new Exception($"Expected '{tag}' at {br.BaseStream.Position - 4} got '{_tag}'.");
        }

        public static bool HasTag(this BinaryReader br, string tag)
        {
            bool match = tag == br.ReadString(4);
            if (!match)
                br.BaseStream.Position -= 4;
            return match;
        }
    }
}
