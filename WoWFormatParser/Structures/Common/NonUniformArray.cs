using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using WoWFormatParser.Helpers;
using WoWFormatParser.Structures.Interfaces;

namespace WoWFormatParser.Structures.Common
{
    public class NonUniformArray<T> : IStringDescriptor, IReadOnlyList<T[]> where T : struct
    {
        public readonly T[][] Records;

        public NonUniformArray(BinaryReader br, int rowCount, params int[] columnCount)
        {
            Records = new T[rowCount][];

            int colcount = columnCount.Length;
            for (int i = 0; i < rowCount; i++)
            {
                int cols = columnCount[i % colcount];
                Records[i] = br.ReadStructArray<T>(cols);
            }
        }

        public override string ToString()
        {
            int rowCount = Records.GetLength(0);
            int colCount = Records[0].Length;

            StringBuilder sb = new StringBuilder(colCount * rowCount * 2);
            sb.AppendLine("[");
            for (int i = 0; i < rowCount; i++)
                sb.AppendLine("[" + string.Join(", ", Records[i]) + "]");
            sb.Append("]");

            return sb.ToString();
        }


        public T[] this[int index] => Records[index];

        public int Count => Records.Length;

        public IEnumerator<T[]> GetEnumerator() => Records.AsEnumerable().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
