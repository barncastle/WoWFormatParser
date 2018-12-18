using System.IO;
using WoWFormatParser.Helpers;

namespace WoWFormatParser.Structures.WDL
{
    public class MARE
    {
        public short[,] HighResVertices;
        public short[,] LowResVertices;

        public MARE(BinaryReader br)
        {
            HighResVertices = br.ReadJaggedArray(17, 17, () => br.ReadInt16());
            LowResVertices = br.ReadJaggedArray(16, 16, () => br.ReadInt16());
        }
    }
}
