using System;
using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.DB
{
    public sealed class DB : Format
    {
        public int FieldCount;
        public int ColumnCount;
        public string[,] Data; // hax

        public DB(BinaryReader br)
        {
            FieldCount = br.ReadInt32();
            ColumnCount = br.ReadInt32(); // includes header row
            Data = br.ReadJaggedArray(ColumnCount, FieldCount, () => ReadField(br));

            if (FieldCount != ColumnCount)
                throw new Exception("Verify Field and Column indices.");
        }

        private string ReadField(BinaryReader br)
        {
            string value = null;
            string type = br.ReadString(4).TrimEnd('\0');
            switch (type)
            {
                case "S":
                    // stringref like DBC
                    long pos = br.BaseStream.Position;
                    br.BaseStream.Position = br.ReadUInt32();
                    value = br.ReadCString();
                    br.BaseStream.Position = pos + 4;
                    break;
                case "F":
                    value = br.ReadSingle().ToString();
                    break;
                default:
                    throw new NotImplementedException($"Unknown DNC.DB type {type}");
            }

            return value;
        }
    }

}
